using Unity.Netcode;
using UnityEngine;

namespace Player
{
    public class PlayerAiming : NetworkBehaviour
    {
        [SerializeField] private LayerMask groundMask;
        
        private PlayerWeapon _weapon;
        private Camera _camera;
        
        public override void OnNetworkSpawn()
        {
            _weapon = GetComponent<PlayerWeapon>();
            
            if (!IsOwner)
            {
                enabled = false;
                return;
            }

            _camera = Camera.main;
            if (_camera == null)
            {
                Debug.LogError($"카메라를 찾을 수 없습니다.");
            }
        }

        private void Update()
        {
            if (!IsOwner || _camera == null)
            {
                return;
            }
            
            HandleRotation();
            HandleInput();
        }
        
        private void HandleRotation()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask))
            {
                Vector3 aimPoint = hit.point;
                Vector3 direction = aimPoint - transform.position;
                direction.y = 0;

                if (direction.magnitude > 0.1f)
                {
                    transform.rotation = Quaternion.LookRotation(direction);
                }
            }
        }

        private void HandleInput()
        {
            if (Input.GetMouseButton(0))
            {
                _weapon.Shoot();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                _weapon.Reload();
            }
        }
    }
}
using Unity.Netcode;
using UnityEngine;

namespace Player
{
    public class PlayerAiming : NetworkBehaviour
    {
        [SerializeField] private LayerMask groundMask;
        
        private Camera _playerCamera;
        
        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                enabled = false;
                return;
            }

            _playerCamera = Camera.main;
            if (_playerCamera == null)
            {
                Debug.LogError($"카메라를 찾을 수 없습니다.");
            }
        }

        private void Update()
        {
            if (!IsOwner || _playerCamera == null)
                return;

            Ray ray = _playerCamera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask))
            {
                return;
            }
            
            Vector3 aimPoint = hit.point;
            Vector3 direction = aimPoint - transform.position;
            direction.y = 0;

            if (!(direction.magnitude > 0.1f))
            {
                return;
            }
            
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }
}
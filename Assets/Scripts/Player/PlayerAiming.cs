using Unity.Netcode;
using UnityEngine;

namespace Player
{
    public class PlayerAiming : NetworkBehaviour
    {
        [SerializeField] private LayerMask groundMask;
        
        [Header("Bullet")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform firePoint;
        
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
            {
                return;
            }
            
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

            if (Input.GetMouseButtonDown(0))
            {
                FireServerRpc();
            }
        }
        
        [ServerRpc]
        private void FireServerRpc()
        {
            GameObject bullet = Instantiate
            (
                bulletPrefab, 
                firePoint.position, 
                NetworkObject.transform.rotation
            );
            
            if (bullet.TryGetComponent<NetworkObject>(out var bulletNetObj))
            {
                bulletNetObj.Spawn();
            }
            
            if (bullet.TryGetComponent<Bullet.Bullet>(out var bulletComponent))
            {
                bulletComponent.InitLayer(isPlayerSide: true);
            }
        }
    }
}
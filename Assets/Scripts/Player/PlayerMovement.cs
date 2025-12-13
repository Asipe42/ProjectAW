using Unity.Netcode;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : NetworkBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        
        public override void OnNetworkSpawn()
        {
            if (IsOwner) 
            {
                
            }
        }
        
        private void Update()
        {
            if (!IsOwner) 
                return;
            
            var horizontalInput = Input.GetAxis("Horizontal");
            var verticalInput = Input.GetAxis("Vertical");
            var moveDirection = new Vector3(horizontalInput, 0, verticalInput);

            if (moveDirection.magnitude > 1f)
            {
                moveDirection.Normalize();
            }

            if (moveDirection.magnitude > 0)
            {
                transform.position += moveDirection * (moveSpeed * Time.deltaTime);
            }
        }
    }
}
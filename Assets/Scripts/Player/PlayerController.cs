using Unity.Netcode;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerHealth))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerAiming))]
    public class PlayerController : NetworkBehaviour
    {
        private PlayerHealth _health;
        private PlayerMovement _movement;
        private PlayerAiming _aiming;

        private void Awake()
        {
            _health = GetComponent<PlayerHealth>();
            _movement = GetComponent<PlayerMovement>();
            _aiming = GetComponent<PlayerAiming>();
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
            
            }
        }
    }
}

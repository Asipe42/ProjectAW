using Unity.Netcode;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : NetworkBehaviour
    {
        public readonly NetworkVariable<float> Health = new
        (
            100f, 
            NetworkVariableReadPermission.Everyone, 
            NetworkVariableWritePermission.Server
        );       
        
        public override void OnNetworkSpawn()
        {
            Health.OnValueChanged += HandleHealthChanged;
            HandleHealthChanged(Health.Value, Health.Value);
        }
        
        public override void OnNetworkDespawn()
        {
            Health.OnValueChanged -= HandleHealthChanged;
        }
        
        private void HandleHealthChanged(float oldValue, float newValue)
        {
            Debug.Log($"플레이어 {OwnerClientId}의 체력 변화: {oldValue} → {newValue}");
        }
    }
}
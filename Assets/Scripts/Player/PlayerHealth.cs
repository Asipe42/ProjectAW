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
        
        internal void TakeDamage(float damage) 
        {
            if (!IsServer)
            {
                Debug.LogError("클라이언트가 무단으로 데미지 함수를 호출했습니다.");
                return;
            }

            Health.Value -= damage;
            
            if (Health.Value <= 0)
            {
                Health.Value = 0;
                Die();
            }
        }
        
        private void HandleHealthChanged(float oldValue, float newValue)
        {
            Debug.Log($"플레이어 {OwnerClientId}의 체력 변화: {oldValue} → {newValue}");
        }
        
        private void Die()
        {
            Debug.Log($"플레이어 {OwnerClientId} 사망");
        }
    }
}
using Unity.Netcode;
using UnityEngine;

namespace Enemy
{
    public class EnemyHealth : NetworkBehaviour
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
        }
        
        public override void OnNetworkDespawn()
        {
            Health.OnValueChanged -= HandleHealthChanged;
        }
        
        private void HandleHealthChanged(float oldValue, float newValue)
        {
            Debug.Log($"적 체력 변경: {oldValue} -> {newValue}");

            if (newValue <= 0)
            {
                
            }
        }
        
        [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
        public void TakeDamageServerRpc(int damage)
        {
            if (Health.Value <= 0) 
                return;

            Health.Value -= damage;

            if (Health.Value <= 0)
            {
                GetComponent<NetworkObject>().Despawn();
            }
        }
    }
}
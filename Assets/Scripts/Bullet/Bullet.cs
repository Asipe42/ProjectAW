using System.Collections;
using Enemy;
using Player;
using Unity.Netcode;
using UnityEngine;

namespace Bullet
{
    public class Bullet : NetworkBehaviour
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private float lifeTime = 3f;
        
        private readonly NetworkVariable<ulong> _ownerClientId = new
        (
            0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );
        
        private Coroutine _despawnRoutine;
        
        private void Update()
        {
            if (IsServer)
            {
                transform.position += transform.forward * (speed * Time.deltaTime);
            }
        }
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsServer)
            {
                _despawnRoutine = StartCoroutine(DespawnAfterDelay(lifeTime));
            }
        }
        
        private IEnumerator DespawnAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (IsServer && NetworkObject.IsSpawned)
            {
                NetworkObject.Despawn();
            }
        }
        
        public void InitLayer(bool isPlayerSide)
        {
            int layerName = isPlayerSide 
                ? LayerMask.NameToLayer("PlayerBullet") 
                : LayerMask.NameToLayer("EnemyBullet");
            
            gameObject.layer = layerName;
            SetLayerClientRpc(layerName);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!IsServer)
            {
                return;
            }
            
            if (_despawnRoutine != null)
            {
                StopCoroutine(_despawnRoutine);
                _despawnRoutine = null;
            }
            
            if (other.TryGetComponent<EnemyHealth>(out var enemyHealth))
            {
                const int damageAmount = 10;
                enemyHealth.TakeDamageServerRpc(damageAmount);
            }
            
            if (NetworkObject.IsSpawned)
            {
                NetworkObject.Despawn();
            }
        }
        
        [ClientRpc]
        private void SetLayerClientRpc(int layer)
        {
            gameObject.layer = layer;
        }
    }
}
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Bullet
{
    public class Bullet : NetworkBehaviour
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private float lifeTime = 3f;
        
        private Coroutine _despawnRoutine;
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsServer)
            {
                _despawnRoutine = StartCoroutine(DespawnAfterDelay(lifeTime));
            }
        }

        private void Update()
        {
            if (IsServer)
            {
                transform.position += transform.forward * (speed * Time.deltaTime);
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
        
        private void OnTriggerEnter(Collider other)
        {
            if (!IsServer)
            {
                return;
            }

            if (_despawnRoutine != null)
            {
                StopCoroutine(_despawnRoutine);
                _despawnRoutine = null; // 참조 초기화
            }

            NetworkObject.Despawn();
        }
    }
}
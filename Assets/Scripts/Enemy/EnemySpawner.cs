using Unity.Netcode;
using UnityEngine;

namespace Enemy
{
    public class EnemySpawner : NetworkBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                SpawnEnemyAtCenter();
            }
        }

        private void SpawnEnemyAtCenter()
        {
            GameObject enemyInstance = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
            enemyInstance.GetComponent<NetworkObject>().Spawn();
        }
    }
}
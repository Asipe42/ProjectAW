using Unity.Netcode;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(EnemyHealth))]
    [RequireComponent(typeof(EnemyMovement))]
    public class EnemyController : NetworkBehaviour
    {
        private EnemyHealth _health;
        private EnemyMovement _movement;

        private void Awake()
        {
            _health = GetComponent<EnemyHealth>();
            _movement = GetComponent<EnemyMovement>();
        }
    }
}
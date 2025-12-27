using Unity.Netcode;
using UnityEngine;

namespace Game
{
    public class EscapeZone : NetworkBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!IsServer)
                return;

            if (other.CompareTag("Player"))
            {
                GameManager.Instance.EndGame(isWin: true);
            }
        }
    }
}
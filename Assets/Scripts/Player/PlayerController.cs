using Unity.Netcode;

namespace Player
{
    public class PlayerController : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
            
            }
        }
    }
}

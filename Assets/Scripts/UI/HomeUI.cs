using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HomeUI : MonoBehaviour
    {
        [SerializeField] private Button buttonServer;
        [SerializeField] private Button buttonHost;
        [SerializeField] private Button buttonClient;

        private void Awake()
        {
            buttonServer.onClick.AddListener(OnClickServer);
            buttonHost.onClick.AddListener(OnClickHost);
            buttonClient.onClick.AddListener(OnClickClient);
        }

        private void OnClickServer()
        {
            NetworkManager.Singleton.StartServer();
        }

        private void OnClickHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        private void OnClickClient()
        {
            NetworkManager.Singleton.StartClient();
        }
    }
}

using Unity.Netcode;
using UnityEngine;

namespace Game
{
    public class GameManager : NetworkBehaviour
    {
        public static GameManager Instance { get; private set; }

        public NetworkVariable<GameState> CurrentState = new(GameState.Playing);

        private void Awake()
        {
            if (Instance == null) 
                Instance = this;
            else 
                Destroy(gameObject);
        }

        public override void OnNetworkSpawn()
        {
            CurrentState.OnValueChanged += OnStateChanged;
        }
        
        public void EndGame(bool isWin)
        {
            if (!IsServer) 
                return;
            
            if (CurrentState.Value != GameState.Playing) 
                return;

            CurrentState.Value = isWin ? GameState.Success : GameState.Fail;
        }

        private void OnStateChanged(GameState previousValue, GameState newValue)
        {
            switch (newValue)
            {
                case GameState.Success:
                    Debug.Log("미션 완료");
                    break;
                case GameState.Fail:
                    Debug.Log("미션 실패");
                    break;
            }
        }
    }
}
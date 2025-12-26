using TMPro;
using UnityEngine;

namespace UI
{
    public class AmmoUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI ammoText;
        
        public static AmmoUI Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance == null) 
                Instance = this;
            else 
                Destroy(gameObject);
        }

        public void UpdateAmmo(int current, int max)
        {
            ammoText.text = $"{current} / {max}";
        }
    }
}
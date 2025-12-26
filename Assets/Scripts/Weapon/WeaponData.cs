using UnityEngine;

namespace Weapon
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon/Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        public string weaponName;
        public int maxAmmo;
        public float reloadTime;
        public float fireRate;
        public GameObject bulletPrefab;
    }
}
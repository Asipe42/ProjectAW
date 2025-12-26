using System;
using UnityEngine;

namespace Weapon
{
    [Serializable]
    public class Weapon
    {
        public WeaponData data;

        private int _currentAmmo;
        
        public int CurrentAmmo 
        { 
            get => _currentAmmo; 
            set => _currentAmmo = Mathf.Clamp(value, 0, data.maxAmmo); 
        }
        
        public bool CanFire
        {
            get { return _currentAmmo > 0; }
        }

        public bool IsFull
        {
            get { return _currentAmmo >= data.maxAmmo; }
        }

        public Weapon(WeaponData data)
        {
            this.data = data;
            _currentAmmo = data.maxAmmo;
        }
        
        public void Reload()
        {
            _currentAmmo = data.maxAmmo;
        }

        public void ConsumeAmmo()
        {
            if (_currentAmmo > 0) _currentAmmo--;
        }
    }
}
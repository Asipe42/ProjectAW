using System.Collections;
using Unity.Netcode;
using UnityEngine;
using Weapon;

namespace Player
{
    public class PlayerWeapon : NetworkBehaviour
    {
        [SerializeField] private Transform firePoint;
        [SerializeField] private WeaponData data;
        
        public NetworkVariable<int> currentAmmo = new();
        public NetworkVariable<bool> isReloading = new();
        
        private Weapon.Weapon _activeWeapon;
        private float _lastFireTime;

        public override void OnNetworkSpawn()
        {
            _activeWeapon = new Weapon.Weapon(data);

            if (IsOwner)
            {
                currentAmmo.OnValueChanged += OnAmmoChanged;
                UpdateLocalUI(currentAmmo.Value);
            }
            
            if (IsServer)
            {
                currentAmmo.Value = _activeWeapon.CurrentAmmo;
            }
        }

        public override void OnNetworkDespawn()
        {
            if (IsOwner)
            {
                currentAmmo.OnValueChanged -= OnAmmoChanged;
            }
        }

        private void UpdateLocalUI(int value)
        {
            if (UI.AmmoUI.Instance != null)
            {
                UI.AmmoUI.Instance.UpdateAmmo(value, _activeWeapon.data.maxAmmo);
            }
        }
        
        public void Shoot()
        {
            if (currentAmmo.Value > 0 && !isReloading.Value)
            {
                FireServerRpc();
            }
        }

        public void Reload()
        {
            if (currentAmmo.Value < _activeWeapon.data.maxAmmo && !isReloading.Value)
            {
                RequestReloadServerRpc();
            }
        }
        
        private void OnAmmoChanged(int previousValue, int newValue)
        {
            if (IsOwner)
            {
                UpdateLocalUI(newValue);
            }
        }
        
        [ServerRpc]
        private void FireServerRpc()
        {
            if (isReloading.Value || currentAmmo.Value <= 0) 
                return;
            
            if (Time.time < _lastFireTime + _activeWeapon.data.fireRate) 
                return;

            _lastFireTime = Time.time;
            _activeWeapon.ConsumeAmmo();
            currentAmmo.Value = _activeWeapon.CurrentAmmo;

            GameObject bullet = Instantiate
            (
                _activeWeapon.data.bulletPrefab, 
                firePoint.position, 
                transform.rotation
            );

            if (bullet.TryGetComponent<NetworkObject>(out var networkObject))
            {
                networkObject.Spawn();
            }

            if (bullet.TryGetComponent<Bullet.Bullet>(out var bulletComponent))
            {
                bulletComponent.InitLayer(isPlayerSide: true);
            }
        }

        [ServerRpc]
        private void RequestReloadServerRpc()
        {
            if (isReloading.Value || _activeWeapon.IsFull) 
                return;

            StartCoroutine(ReloadRoutine());
        }

        private IEnumerator ReloadRoutine()
        {
            isReloading.Value = true;
            Debug.Log($"{data.name}: 장전 시작");

            yield return new WaitForSeconds(_activeWeapon.data.reloadTime);

            _activeWeapon.Reload();
            currentAmmo.Value = _activeWeapon.CurrentAmmo;
            isReloading.Value = false;
            
            Debug.Log($"{data.name}: 장전 완료");
        }
    }
}
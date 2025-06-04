using System;
using UnityEngine;

namespace Orpheus.Core
{
    public class HideWeapon : MonoBehaviour
    {
        [field: SerializeField] private GameObject Lyre;
        [field: SerializeField] private GameObject Guitare;

        private void Start()
        {
            GameManager.Instance.WeaponSwap += OnSwapWeapon;
        }

        private void OnDestroy()
        {
            GameManager.Instance.WeaponSwap -= OnSwapWeapon;
        }

        private void OnSwapWeapon(int index)
        {
            switch (index)
            {
                case 0 : Lyre.SetActive(true);
                    Guitare.SetActive(false);
                    break;
                case 1 :Lyre.SetActive(false);
                    Guitare.SetActive(true);
                    break;
            }
        }
    }
}
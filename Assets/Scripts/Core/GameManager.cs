using System;
using System.Collections.Generic;
using System.Linq;
using Orpheus.Core.FightSystem.AbilityHolders.Items;
using Orpheus.Core.FightSystem.AbilityHolders.Items.Data;
using Orpheus.Core.Orbital.Player;
using UnityEngine;

namespace Orpheus.Core
{
    public class GameManager : MonoBehaviour
    {
        //for singleton behavior
        public static GameManager Instance { get; private set; }

        
        //related to the player
        [SerializeField] private PlayerOrbitalController player;
        private List<Weapon> weapons;
        private Weapon currentWeapon;
        
        protected void Awake()
        {
            //for singleton behavior
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            //for singleton behavior_end
        }
    
        // for singleton Ensures it's created automatically if accessed before existing
        public static GameManager GetInstance()
        {
            if (Instance == null)
            {
                GameObject managerObject = new GameObject("Game_Manager");
                Instance = managerObject.AddComponent<GameManager>();
                DontDestroyOnLoad(managerObject);
            }
            return Instance;
        }

        public void Start()
        {
            weapons = new List<Weapon>();
            foreach (var weaponData in GameController.GameDatabase.WeaponDatas)
            {
                weapons.Add(weaponData.GenerateWeapon());
            }

            if (weapons.Count > 0)
            {
                currentWeapon = weapons[0];
                currentWeapon.EquipItem(player);
            }
        }

        public void SwapWeapon(int index)
        {
            currentWeapon.UnequipItem(player);
            currentWeapon = weapons[index];
            currentWeapon.EquipItem(player);
        }
    }
    
    
}
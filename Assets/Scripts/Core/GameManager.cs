using System;
using System.Collections.Generic;
using System.Linq;
using Orpheus.Core.FightSystem;
using Orpheus.Core.FightSystem.AbilityHolders.Items;
using Orpheus.Core.FightSystem.AbilityHolders.Items.Data;
using Orpheus.Core.Orbital.Entities;
using Orpheus.Core.Orbital.Player;
using Orpheus.Core.Rings;
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
        
        
        //related to the generation
        [field :SerializeField] private GameObject[] enemiesSpawn;
        [field :SerializeField] private FloorData floorData;
        
        //related to the level
        [SerializeField] private Floor currentFloor;
        private List<AI_Entities> enemiesToKill;
        
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
            //Weaponery
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
            
            //enemies
            enemiesToKill = new List<AI_Entities>();
            SpawnEnemies();
        }

        private void GenerateRoom()
        {
            
        }
        private void SpawnEnemies()
        {
            GameObject enemyObject = Instantiate(floorData.EnemiesToSpawn[0], enemiesSpawn[0].transform);
            enemyObject.GetComponent<AI_Entities>().SetRing(currentFloor.rings[1]);
            enemiesToKill.Add(enemyObject.GetComponent<AI_Entities>());
        }

        public void OnEnemyKilled(AI_Entities enemy)
        {
            enemiesToKill.Remove(enemy);
            Destroy(enemy.gameObject);
        }

        public void SwapWeapon(int index)
        {
            if (0 <= index && index < weapons.Count)
            {
                currentWeapon.UnequipItem(player);
                currentWeapon = weapons[index];
                currentWeapon.EquipItem(player);
            }
        }
    }
}
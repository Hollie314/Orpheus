using System;
using System.Collections.Generic;
using System.Linq;
using Orpheus.Core.FightSystem;
using Orpheus.Core.FightSystem.AbilityHolders.Items;
using Orpheus.Core.FightSystem.AbilityHolders.Items.Data;
using Orpheus.Core.LevelGeneration;
using Orpheus.Core.Orbital.Entities;
using Orpheus.Core.Orbital.Player;
using Orpheus.Core.Rings;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Orpheus.Core
{
    public class GameManager : MonoBehaviour
    {
        //for singleton behavior
        public static GameManager Instance { get; private set; }
        
        //related to the player
        [SerializeField] private PlayerOrbitalController player;
        [SerializeField] private Ring hiddenRing;
        private List<Weapon> weapons;
        private Weapon currentWeapon;
        private List<DamageType> deathType;
        
        //related to the generation
        [SerializeField] private Floor currentFloor;
        [field :SerializeField] private FloorData floorData;
        [field :SerializeField] private BiomeName currentBiomeName;
        private List<GameObject> enemiesSpawn;
        
        //related to the level
        private List<AI_Entities> enemiesToKill;
        public int roomNumber { get; private set; } 
        
        
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
            enemiesSpawn = new List<GameObject>();
            deathType = new List<DamageType>();
            roomNumber = 1;
            HidePlayer();
        }

        private void GenerateRoom()
        {
            HidePlayer();
            int ringIndex = 0;
            
            //we do this for all ring size
            foreach (var ring in currentFloor.rings)
            {
                //clearing the list of spawn 
                enemiesSpawn.Clear();
                
                //destroy old ring body and set new ring
                DestroyRingChild(ring);
                GameObject ringAvatar = GetRandomeRing(ring);
                GameObject spawnRing = SpawnRing(ringAvatar, ring.transform);
                
                //now we will get all spawn in this ring
                foreach (Transform child in spawnRing.transform)
                {
                    if (child.name == "Spawn_Enemies")
                    {
                        Debug.Log("we found some spawn location");
                        enemiesSpawn.Add(child.gameObject);
                    }
                }
                // now we will get a few random spawn 
                if (enemiesSpawn.Count > 0)
                {
                    int lenght = enemiesSpawn.Count / 2;
                    for (int i = 0; i < lenght; i++)
                    {
                        GameObject spawn = GetRandomSpawn();
                        SpawnEnemies(ringIndex,spawn);
                        enemiesSpawn.Remove(spawn);
                    }
                }
                SetPlayerOnRing(currentFloor.rings[0]);
            }
        }

        private void DestroyRingChild(Ring ring)
        {
            foreach (Transform child in ring.gameObject.transform)
            {
                Destroy(child.gameObject);
            }
        }
        
        private GameObject SpawnRing(GameObject child, Transform parent)
        {
            GameObject newChild = Instantiate(child, parent);
            newChild.transform.localPosition = Vector3.zero;
            newChild.transform.localRotation = Quaternion.identity;
            newChild.transform.localScale = Vector3.one;
            return newChild;
        }

        private GameObject GetRandomeRing(Ring ring)
        {
            int random = Random.Range(0, ring.RingData.Avatar.Length-1);
            RingVariantData variant = ring.RingData.Avatar[random];
            return variant.GetVariant(currentBiomeName);
        }
        private void SpawnEnemies(int ringIndex, GameObject spawn)
        {
            GameObject enemyObject = Instantiate(GetRandomEnemie(), spawn.transform);
            enemyObject.GetComponent<AI_Entities>().SetRing(currentFloor.rings[ringIndex]);
            enemiesToKill.Add(enemyObject.GetComponent<AI_Entities>());
        }

        private GameObject GetRandomSpawn()
        {
            int random = Random.Range(0, enemiesSpawn.Count-1);
            return enemiesSpawn[random];
        }
        
        private GameObject GetRandomEnemie()
        {
            int random = Random.Range(0, floorData.EnemiesToSpawn.Length-1);
            return floorData.EnemiesToSpawn[random];
        }

        public void OnEnemyKilled(AI_Entities enemy)
        {
            enemiesToKill.Remove(enemy);
            Destroy(enemy.gameObject);
            if (enemiesToKill.Count == 0)
            {
                OnNewRoom();
            }
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

        private void HidePlayer()
        {
            player.SetRing(hiddenRing);
            SetPlayerOnRing(hiddenRing);
        }

        private void SetPlayerOnRing(Ring ring)
        {
            Vector3 ringPosition = ring.transform.position;
            player.transform.position =
                new Vector3(ringPosition.x + ring.RingData.Radius, ringPosition.y, ringPosition.z);
        }

        public void OnNewRoom()
        {
            roomNumber++;
            GenerateRoom();
        }

        public void OnPlayerDeath(DamageType damage, TargetTeam team)
        {
            //display death screen
            if (!deathType.Contains(damage))
            {
                Debug.Log("new death");
                deathType.Add(damage);
            }
            
        }
    }
}
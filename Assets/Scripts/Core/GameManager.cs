using System;
using System.Collections.Generic;
using System.Linq;
using Orpheus.Core.FightSystem;
using Orpheus.Core.FightSystem.AbilityHolders.Items;
using Orpheus.Core.FightSystem.AbilityHolders.Items.Data;
using Orpheus.Core.FightSystem.Trap;
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
        public event Action<int> WeaponSwap;
        
        //related to the room generation
        [SerializeField] private Floor currentFloor;
        [field :SerializeField] private FloorData[] floorData;
        [field :SerializeField] private BiomeName currentBiomeName;
        private List<GameObject> enemiesSpawn;
        private List<GameObject> trapSpawns;
        private List<GameObject> listOfChildren;
        [field :SerializeField] private List<Floor> floors;
        
        //related to the room
        private List<AI_Entities> enemiesToKill;
        public int roomNumber { get; private set; } 
        
        //related to the run
        public event Action PlayerDeath; 
        public int Money { get; private set;}
        private int difficulty;
        public event Action<BiomeName> ChangeBiome;
        
        
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
            
            DontDestroyOnLoad(this);
        }
        
        private void OnApplicationQuit()
        {
            Destroy(this); // Destroy the GameObject when quitting
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
                currentWeapon = weapons[1];
                currentWeapon.EquipItem(player);
                WeaponSwap?.Invoke(currentWeapon.weaponData.Index);
            }
            
            //list init
            enemiesToKill = new List<AI_Entities>();
            enemiesSpawn = new List<GameObject>();
            trapSpawns = new List<GameObject>();
            deathType = new List<DamageType>();
            listOfChildren = new List<GameObject>();
            
            //room
            roomNumber = 1;
            currentBiomeName = BiomeName.Elysee;
            ChangeBiome?.Invoke(currentBiomeName);
            HidePlayer();
        }

        public void StartRun()
        {
            player.Init();
            difficulty = 0;
            GenerateRoom();
        }

        private void GenerateRoom()
        {
            enemiesToKill.Clear();
            HidePlayer();
            foreach (var floor in floors)
            {
                int ringIndex = 0;
            
                //we do this for all ring size
                foreach (var ring in floor.rings)
                {
                    //clearing the list of spawn 
                    enemiesSpawn.Clear();
                    trapSpawns.Clear();
                
                    //destroy old ring body and set new random ring
                    DestroyRingChild(ring);
                    GameObject ringAvatar = GetRandomeRing(ring);
                    GameObject spawnRing = SpawnRing(ringAvatar, ring.transform);
                
                    //now we will get all spawn in this ring
                    listOfChildren.Clear();
                    GetChildRecursive(spawnRing);
                    foreach (var child in listOfChildren)
                    {
                        if (child.name == "Spawn_Enemies")
                        {
                            enemiesSpawn.Add(child.gameObject);
                        }
                        if (child.name == "Spawn_Trap")
                        {
                            trapSpawns.Add(child.gameObject);
                        }
                    }
                    // now we will spawn a few ennemies
                    if (enemiesSpawn.Count > 0&& floor.EnemiesToSpawn.Length>0)
                    {
                        int lenght = enemiesSpawn.Count/Mathf.CeilToInt((4f - difficulty) * 0.5f);
                        for (int i = 0; i < lenght; i++)
                        {
                            GameObject spawn = GetRandomSpawn();
                            SpawnEnemies(ringIndex,spawn, floor);
                            enemiesSpawn.Remove(spawn);
                        }
                    }
                    //and a few traps
                    if (trapSpawns.Count > 0 && floor.TrapToSpawn.Length>0)
                    {
                        int lenght = trapSpawns.Count/Mathf.CeilToInt((4f - difficulty) * 0.5f);
                        for (int i = 0; i < lenght; i++)
                        {
                            GameObject spawn = GetRandomSpawn();
                            SpawnTraps(ringIndex,spawn, floor);
                            trapSpawns.Remove(spawn);
                        }
                    }
                    ringIndex++;
                }
            }
            SetPlayerOnRing(currentFloor.rings[0]);
        }

        // a way to get all child because Salim put spawn transform in weird far away place so...
        private void GetChildRecursive(GameObject obj)
        {
            if (null == obj)
                return;

            foreach (Transform child in obj.transform)
            {
                if (null == child)
                    continue;
                listOfChildren.Add(child.gameObject);
                GetChildRecursive(child.gameObject);
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
            int random = Random.Range(0, ring.RingData.Avatar.Length);
            RingVariantData variant = ring.RingData.Avatar[random];
            return variant.GetVariant(currentBiomeName);
        }
        private void SpawnEnemies(int ringIndex, GameObject spawn, Floor floor)
        {
            GameObject enemyObject = Instantiate(GetRandomEnemie(), spawn.transform);
            enemyObject.GetComponent<AI_Entities>().SetRing(floor.rings[ringIndex]);
            enemyObject.transform.localPosition = Vector3.zero;
            enemyObject.transform.localRotation = Quaternion.identity;
            enemiesToKill.Add(enemyObject.GetComponent<AI_Entities>());
        }
        
        private void SpawnTraps(int ringIndex, GameObject spawn, Floor floor)
        {
            GameObject enemyObject = Instantiate(GetRandomTrap(), spawn.transform);
            enemyObject.GetComponent<Trap>().Initialize(floor.rings[ringIndex]);
            enemyObject.transform.localPosition = Vector3.zero;
            enemyObject.transform.localRotation = Quaternion.identity;
        }

        private GameObject GetRandomSpawn()
        {
            int random = Random.Range(0, enemiesSpawn.Count);
            return enemiesSpawn[random];
        }
        
        private GameObject GetRandomEnemie()
        {
            int random = Random.Range(0, currentFloor.EnemiesToSpawn.Length);
            return currentFloor.EnemiesToSpawn[random];
        }
        
        private GameObject GetRandomTrap()
        {
            int random = Random.Range(0, currentFloor.TrapToSpawn.Length);
            return currentFloor.TrapToSpawn[random];
        }

        public void OnEnemyKilled(AI_Entities enemy)
        {
            enemiesToKill.Remove(enemy);
            enemy.gameObject.SetActive(false);
            if (enemiesToKill.Count == 0)
            {
                OnNewRoom();
            }
        }

        public void SwapFloor(Floor floor)
        {
            if (floors.Contains(floor))
            {
                currentFloor = floor;
                SetPlayerOnRing(GetRingOfSameSize(floor, player.CurrentRing));
                Debug.Log("new floor");
            }
        }

        private Ring GetRingOfSameSize(Floor floor, Ring currentring)
        {
            foreach (var ring in floor.rings)
            {
                if (currentring.RingData.Size == ring.RingData.Size)
                {
                    return ring;
                }
            }
            return floor.rings[0];
        }

        public void SwapWeapon(int index)
        {
            if (0 <= index && index < weapons.Count)
            {
                currentWeapon.UnequipItem(player);
                currentWeapon = weapons[index];
                currentWeapon.EquipItem(player);
                player.SetWeapon(currentWeapon.weaponData.Index);
                WeaponSwap?.Invoke(currentWeapon.weaponData.Index);
            }
        }

        private void HidePlayer()
        {
            SetPlayerOnRing(hiddenRing);
        }

        private void SetPlayerOnRing(Ring ring)
        {
            Vector3 ringPosition = ring.transform.position;
            //player.transform.Translate(new Vector3(ringPosition.x + ring.RingData.Radius, ringPosition.y+1, ringPosition.z));
            //player.transform.position = new Vector3(ringPosition.x + ring.RingData.Radius, ringPosition.y+1, ringPosition.z);
            player.SetRing(ring);
        }

        public void OnNewRoom()
        {
            roomNumber++;
            if (roomNumber == 4)
            {
                difficulty++;
                currentBiomeName = BiomeName.ChampsDesChatiments;
                ChangeBiome?.Invoke(currentBiomeName);
            }
            if (roomNumber == 8)
            {
                difficulty++;
                currentBiomeName = BiomeName.Tartare;
                ChangeBiome?.Invoke(currentBiomeName);
            }
            GenerateRoom();
        }

        public void OnPlayerDeath(DamageType damage, TargetTeam team)
        {
            //display death screen
            if (!deathType.Contains(damage))
            {
                Debug.Log("new death");
                deathType.Add(damage);
                // faire logique de débloquage or whatever
            }
            Debug.Log("we dedge");
            PlayerDeath?.Invoke();
        }

        private void GetRoomReward()
        {
            
        }

        private void GetRunReward()
        {
            
        }

        private void EndRun()
        {
            
        }

        public void ChooseMuse(int museID)
        {
            
        }
    }
}
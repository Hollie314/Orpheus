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
        public event Action<int> WeaponSwap;
        
        //related to the room generation
        [SerializeField] private Floor currentFloor;
        [field :SerializeField] private FloorData floorData;
        [field :SerializeField] private BiomeName currentBiomeName;
        private List<GameObject> enemiesSpawn;
        private List<GameObject> listOfChildren;
        
        //related to the room
        private List<AI_Entities> enemiesToKill;
        public int roomNumber { get; private set; } 
        
        //related to the run
        public event Action PlayerDeath; 
        public int Money { get; private set;}
        private int difficulty;
        
        
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
                currentWeapon = weapons[1];
                currentWeapon.EquipItem(player);
                WeaponSwap?.Invoke(currentWeapon.weaponData.Index);
            }
            
            //list init
            enemiesToKill = new List<AI_Entities>();
            enemiesSpawn = new List<GameObject>();
            deathType = new List<DamageType>();
            listOfChildren = new List<GameObject>();
            
            //room
            roomNumber = 1;
            currentBiomeName = BiomeName.Elysee;
            HidePlayer();
        }

        public void StartRun()
        {
            player.Stats.setStat(FloatStats.Hp, player.Stats.getStat(FloatStats.HpMax));
            difficulty = 0;
            GenerateRoom();
        }

        private void GenerateRoom()
        {
            enemiesToKill.Clear();
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
                listOfChildren.Clear();
                GetChildRecursive(spawnRing);
                foreach (var child in listOfChildren)
                {
                    if (child.name == "Spawn_Enemies")
                    {
                        enemiesSpawn.Add(child.gameObject);
                    }
                }
                // now we will get a few random spawn 
                if (enemiesSpawn.Count > 0)
                {
                    int lenght = enemiesSpawn.Count/Mathf.CeilToInt((4f - difficulty) * 0.5f);
                    for (int i = 0; i < lenght; i++)
                    {
                        GameObject spawn = GetRandomSpawn();
                        SpawnEnemies(ringIndex,spawn);
                        enemiesSpawn.Remove(spawn);
                    }
                }
                ringIndex++;
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
        private void SpawnEnemies(int ringIndex, GameObject spawn)
        {
            GameObject enemyObject = Instantiate(GetRandomEnemie(), spawn.transform);
            enemyObject.GetComponent<AI_Entities>().SetRing(currentFloor.rings[ringIndex]);
            enemiesToKill.Add(enemyObject.GetComponent<AI_Entities>());
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

        public void OnEnemyKilled(AI_Entities enemy)
        {
            enemiesToKill.Remove(enemy);
            enemy.gameObject.SetActive(false);
            if (enemiesToKill.Count == 0)
            {
                AbilityManager.Instance.OnDisable();
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
            player.transform.position = new Vector3(ringPosition.x + ring.RingData.Radius, ringPosition.y+1, ringPosition.z);
            player.SetRing(ring);
        }

        public void OnNewRoom()
        {
            roomNumber++;
            if (roomNumber == 4)
            {
                difficulty++;
                currentBiomeName = BiomeName.ChampsDesChatiments;
            }
            if (roomNumber == 8)
            {
                difficulty++;
                currentBiomeName = BiomeName.Tartare;
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
    }
}
using System;
using System.Net.Mime;
using Orpheus.Core.Orbital.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Orpheus.Core.UI
{
    public class UI_Manager : MonoBehaviour
    {

        //for singleton behavior
        public static UI_Manager Instance { get; private set; }
        
        [field:SerializeField]
        private GameObject UI_HUB;
        [field:SerializeField]
        private GameObject UI_DeathScreen;
        
        [field:SerializeField]
        private Text  money;
        
        [field:SerializeField]
        private Image LifeIWishICouldUse;
        [field:SerializeField]
        private PlayerOrbitalController player;
        
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

            GameManager.Instance.PlayerDeath += OnPlayerDeath;
        }

        public void OnDestroy()
        {
            GameManager.Instance.PlayerDeath -= OnPlayerDeath;
        }

        // for singleton Ensures it's created automatically if accessed before existing
        public static UI_Manager GetInstance()
        {
            if (Instance == null)
            {
                GameObject managerObject = new GameObject("UI_Manager");
                Instance = managerObject.AddComponent<UI_Manager>();
                DontDestroyOnLoad(managerObject);
            }
            return Instance;
        }

        private void Update()
        {
            UpadateLife();
        }

        public void StartRun()
        {
            UI_HUB.SetActive(false);
        }

        private void OnPlayerDeath()
        {
            UI_DeathScreen.SetActive(true);
        }

        public void OpenHUB()
        {
            UI_HUB.SetActive(true);
        }

        public void UpdateMoney(int money)
        {
            this.money.text = money.ToString();
        }

        private void UpadateLife()
        {
            LifeIWishICouldUse.fillAmount = player.Stats.getStat(FloatStats.Hp)/player.Stats.getStat(FloatStats.HpMax);
        }
    }
}
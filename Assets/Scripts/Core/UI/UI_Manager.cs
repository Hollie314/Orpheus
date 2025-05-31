using System;
using UnityEngine;

namespace Orpheus.Core.UI
{
    public class UI_Manager : MonoBehaviour
    {

        //for singleton behavior
        public static UI_Manager Instance { get; private set; }
        
        [field:SerializeField]
        private GameObject UI_HUB;
        
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
        
        public void StartRun()
        {
            UI_HUB.SetActive(false);
        }

        private void OnPlayerDeath()
        {
            OpenHUB();
        }

        public void OpenHUB()
        {
            UI_HUB.SetActive(true);
        }
    }
}
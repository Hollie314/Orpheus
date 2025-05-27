using UnityEngine;

namespace Orpheus.Core
{
    public class GameController : MonoBehaviour
    {
        public static GameDatabase GameDatabase { get; private set; }
    
        //allow to load the class when the game start and before the scene load
        private void Start()
        {
        
        
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Load()
        {
            //generate the database
            GameDatabase = new GameDatabase();
        }
    }
}
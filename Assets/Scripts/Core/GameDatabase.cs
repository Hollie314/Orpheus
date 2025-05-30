using Orpheus.Core.FightSystem.AbilityHolders.Items.Data;
using UnityEngine;

namespace Orpheus.Core
{
    public class GameDatabase
    {
        public WeaponData[] WeaponDatas { get; private set; }
        
        public GameDatabase()
        {
            WeaponDatas = Resources.LoadAll<WeaponData>("Player/Weapons");
        }
    }
}

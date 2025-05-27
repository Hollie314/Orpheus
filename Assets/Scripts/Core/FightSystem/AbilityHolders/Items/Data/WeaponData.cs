using System.Collections.Generic;
using Orpheus.Core.FightSystem.Skills.Data;
using Orpheus.Core.FightSystem.Skills.Runtime;
using UnityEngine;

namespace Orpheus.Core.FightSystem.AbilityHolders.Items.Data
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Orpheus/FightSystem/WeaponData", order = 0)]
    public class WeaponData : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int Index { get; private set; }
        [field: SerializeField] public SkillData[] skills { get; private set; }

        public Weapon GenerateWeapon()
        {
            return new Weapon(this);
        }
    }
}
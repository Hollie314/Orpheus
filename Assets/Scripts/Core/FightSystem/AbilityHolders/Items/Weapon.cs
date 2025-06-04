using System.Collections.Generic;
using Orpheus.Core.FightSystem.AbilityHolders.Items.Data;
using Orpheus.Core.FightSystem.Skills.Runtime;

namespace Orpheus.Core.FightSystem.AbilityHolders.Items
{
    public class Weapon
    {
        public List<Skill> skills;
        public WeaponData weaponData { get; private set; }

        public Weapon(WeaponData data)
        {
            weaponData = data;
            skills = new List<Skill>();
        }
        
        public void EquipItem(IAbilityCaster caster)
        {
            foreach (var skill in weaponData.skills)
            {
                skills.Add(skill.GenerateAbility(caster));
            }

            for (int i = 0; i < skills.Count; i++)
            {
                caster.AddSkill(skills[i],i);
            }
        }
        
        public void UnequipItem(IAbilityCaster caster)
        {
            foreach (var skill in skills)
            {
                caster.RemoveSkill(skill);
            }
            skills.Clear();
        }
    }
}
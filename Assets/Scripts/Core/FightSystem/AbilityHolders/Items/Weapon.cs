using System.Collections.Generic;
using Orpheus.Core.FightSystem.AbilityHolders.Items.Data;
using Orpheus.Core.FightSystem.Skills.Runtime;

namespace Orpheus.Core.FightSystem.AbilityHolders.Items
{
    public class Weapon
    {
        public List<Skill> skills;
        private IAbilityCaster caster;
        public WeaponData weaponData { get; private set; }

        public Weapon(WeaponData data)
        {
            weaponData = data;
        }
        
        public void EquipItem(IAbilityCaster caster)
        {
            this.caster = caster;
            foreach (var skill in weaponData.skills)
            {
                skills.Add(skill.GenerateAbility(caster));
            }
            
            foreach (var skill in skills)
            {
                caster.AddSkill(skill);
            }
        }
        
        public void UnequipItem(IAbilityCaster caster)
        {
            foreach (var skill in skills)
            {
                caster.RemoveSkill(skill);
            }
        }
    }
}
namespace Orpheus.Core
{
    public enum RingSize
    {
        small, 
        medium, 
        large, 
        extraLarge
    }

    public enum FloatStats
    {
        PhysicalDamages,
        MagicalDamages,
        AttackSpeed,
        CritChance,
        CritDamages,
        ArmorPenetration,
        MagicPenetration,
        CritRes,
        HpRegen,
        HpMax,
        Hp,
        MagicalResistance,
        PhysicalResistance,
        DebuffResistance,
        Tenacity, 
        Mana,
        ManaMax,
        ManaRegen,
        CooldownReduction,
        BuffAugmentation,
        BuffMalus,
        SustainAugmentation,
        
    }

    public enum BoolStats
    {
        isTargetable,
        isDamageable,
        canUseAbilities,
        canMove,
    }

    public enum AfflictionType
    {
        Burn,
        Poison,
        Bleed,
        Heal,
        CrowdControl,
        Debuff,
        Buff
    }

    public enum DamageType
    {
        Physic,
        Fire,
        Wind,
        True,
        Crush,
    }
    
    public enum TargetTeam
    {
        Player,
        Enemy,
        Trap,
    }
}


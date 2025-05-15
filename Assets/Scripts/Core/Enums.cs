namespace Orpheus.Core
{
    public enum RingSize
    {
        small = 0, 
        medium = 1, 
        large = 2, 
        extraLarge = 3 
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
}


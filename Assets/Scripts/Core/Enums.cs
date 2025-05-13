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
        CritPercentage,
        CritDamages,
        ArmorPenetration,
        MagicPenetration,
        DebuffRes,
        CritRes,
        HpRegen,
        HpMax,
        Hp,
        MagicalResistance,
        PhysicalResistance,
        Tenacity, 
        SlowResistance, 
        BuffAugmentation,
        SustainAugmentation
    }

    public enum BoolStats
    {
        isTargetable,
        isDamageable
    }
}


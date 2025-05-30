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
        Power,
        AttackSpeed,
        CritChance,
        CritDamages,
        ResistancePenetration,
        CritRes,
        HpRegen,
        HpMax,
        Hp,
        Resistance,
        DebuffResistance,
        Tenacity, 
        CooldownReduction,
        BuffAugmentation,
        BuffMalus,
        SustainAugmentation,
        Speed
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
        Damage,
        Heal,
        CrowdControl,
        Debuff,
        Buff
    }

    public enum DamageType
    {
        Physic,
        Fire,
        Poison,
        Bleed,
        True,
        Crush,
    }
    
    public enum TargetTeam
    {
        Player,
        Enemy,
        Trap,
    }

    public enum BiomeName
    {
        ChampsDesChatiments,
        Elysee,
        Tartare,
        Styx
    }
}


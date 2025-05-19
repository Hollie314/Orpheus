namespace Orpheus.Core.FightSystem
{
    public interface IAbilityTarget
    {
        TargetTeam Team { get; }
        void ApplyDamage(int amount);
        void Heal(int amount);
    }
}
using Orpheus.Core.Rings;

namespace Orpheus.Core.FightSystem
{
    public interface IAbilityTarget
    {
        TargetTeam Team { get; }
        public Ring Ring { get; }
        void ApplyDamage(int amount);
        void Heal(int amount);
        void ApplyStatus();
    }
}
using Orpheus.Core.Rings;
using UnityEditor.UIElements;

namespace Orpheus.Core.FightSystem
{
    public interface IAbilityTarget
    {
        TargetTeam Team { get; }
        public OrbitalStats Stats { get; }
        public Ring Ring { get; }
        void ApplyDamage(int amount);
        

        void Heal(OrbitalStats casterstats, int amount,  int percentage, FloatStats stat)
        {
            StatsCalculus.Heal(casterstats, Stats, amount,percentage, stat);
        }
        void ApplyStatus();
    }
}
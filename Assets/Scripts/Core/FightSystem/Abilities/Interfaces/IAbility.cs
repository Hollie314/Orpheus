using System;

namespace Orpheus.Core.FightSystem
{
    public interface IAbility
    {
        bool Update(float deltaTime);
        void Reset();
        public event Action OnEnd;

        float GetLifeTime();
    }
}
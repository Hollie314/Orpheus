using System;

namespace Orpheus.Core.FightSystem
{
    public interface IAbility
    {
        bool AbilityUpdate(float deltaTime);
        void EndAbility();
        public event Action OnEnd;
        public void Dispose();

        float GetLifeTime();
        public void Init();
    }
}
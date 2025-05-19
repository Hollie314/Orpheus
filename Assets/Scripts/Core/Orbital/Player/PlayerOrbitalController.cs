using NaughtyAttributes;
using Orpheus.Core.Rings;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Orpheus.Core.Orbital.Player
{
    public class PlayerOrbitalController : OrbitalController<PlayerOrbitalController>
    {
        public PlayerInput PlayerInput { get; private set; }
        [SerializeField, BoxGroup("Player")] private PlayerMovementState[] defaultStates;
        [SerializeField, BoxGroup("Player")] private Ring ring;
        protected override void Awake()
        {
            base.Awake();
            PlayerInput = GetComponent<PlayerInput>();
            SetRing(ring);
        }

        private void Start()
        {
            for (int i = 0; i < defaultStates.Length; i++)
            {
                AddState(defaultStates[i]);
            }
        }
    }
}
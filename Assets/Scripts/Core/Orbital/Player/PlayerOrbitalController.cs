using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Player
{
    public class PlayerOrbitalController : OrbitalController<PlayerOrbitalController>
    {
        public PlayerInput PlayerInput { get; private set; }
        [SerializeField] private PlayerMovementState[] defaultStates;
        [SerializeField] private Ring ring;
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
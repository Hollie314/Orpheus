using NaughtyAttributes;
using Orpheus.Core.Orbital.Player.States.MovementState;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Orpheus.Core.Orbital.Player
{
    [CreateAssetMenu(fileName = "JumpState", menuName = "Orpheus/Player/Jump", order = 0)]
    public class PlayerJumpState : PlayerAirborneState
    {
        [SerializeField, Range(0, 10), BoxGroup("Jump")] 
        private float jumpForce;

        [SerializeField, Range(0, 10), BoxGroup("Jump")]
        private int coyoteTime;
        [SerializeField, Range(1, 5), BoxGroup("Jump")] 
        private int jumpCount;
        
        [SerializeField, BoxGroup("Jump")] 
        private AnimationCurve jumpCurve;
        [SerializeField, BoxGroup("Jump")] 
        private AnimationCurve jumpForcePerCountModifier;
        [SerializeField, BoxGroup("Jump")] 
        private float jumpDuration;
        
        private InputAction jumpInput;
        private int currentJumpBuffer;
        private int currentJumpCount;
        private float currentJumpTime;
        
        private bool IsJumping => currentJumpTime >= 0 && currentJumpTime < jumpDuration;
        private bool wantsToJump;
        
        

        public override void Initialize(PlayerOrbitalController orbitalController)
        {
            base.Initialize(orbitalController);
            jumpInput = orbitalController.PlayerInput.actions.FindActionMap("Player").FindAction("Jump");
            jumpInput.performed += OnJumpInputPerformed;
        }

        public override void Dispose(PlayerOrbitalController orbitalController)
        {
            base.Dispose(orbitalController);
            jumpInput = null;
        }
        
        public override void OnEnter(PlayerOrbitalController orbitalController)
        {
            base.OnEnter(orbitalController);
            currentJumpTime = 0;
            currentJumpCount ++;
            orbitalController.Animator.SetTrigger("saut");
            orbitalController.Animator.SetBool("IsJumping", true);
        }

        public override void OnExit(PlayerOrbitalController orbitalController)
        {
            base.OnExit(orbitalController);
            //Debug.Log("Ending jump");
            currentJumpTime = -1;
            currentJumpBuffer = 0;
            orbitalController.Animator.SetBool("IsJumping", false);
        }

        public override void PreUpdate(PlayerOrbitalController orbitalController)
        {
            base.PreUpdate(orbitalController);
            
            if (currentJumpBuffer >= 0)
                currentJumpBuffer--;

            if (orbitalController.IsGrounded)
                currentJumpCount = 0;
        }

        public override int GetStatePriority(PlayerOrbitalController orbitalController)
        {
            if (IsJumping)
                return 10;

            if (currentJumpBuffer <= 0)
                return -1;
            
            bool canJump = orbitalController.IsGrounded || currentJumpCount < jumpCount;
            
            return canJump ? 10 : -1;
        }

        public override Vector2 GetVelocity(PlayerOrbitalController orbitalController, float deltaTime)
        {
            Vector2 velocity = base.GetVelocity(orbitalController, deltaTime);
            float normalizedTime = currentJumpTime / jumpDuration;
            float modifier = jumpForcePerCountModifier.Evaluate(currentJumpCount / (float)jumpCount);
            float currentJumpForce = jumpForce * modifier * jumpCurve.Evaluate(normalizedTime);

            currentJumpTime += Time.deltaTime;

            //Debug.Log($"{currentJumpCount}/{currentJumpTime} => {currentJumpForce}");
            return new Vector2()
            {
                x = velocity.x,
                y = currentJumpForce,
            };
        }

        private void OnJumpInputPerformed(InputAction.CallbackContext obj)
        {
            currentJumpBuffer = coyoteTime;
            if (IsJumping && jumpDuration > 0 && currentJumpCount < jumpCount)
            {
                currentJumpCount++;
                currentJumpTime = 0;
                //Debug.Break();
            }
        }

    }
}
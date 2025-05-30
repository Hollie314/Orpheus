using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Orpheus.Core.Orbital.Player.States.MovementState
{
    [CreateAssetMenu(fileName = "PlayerDunkState", menuName = "Orpheus/Player/PlayerDunk", order = 1)]
    public class Movement : ScriptableObject
    {
        [SerializeField, BoxGroup("Dunk")] private float distance;
        [SerializeField, BoxGroup("Dunk")] private float jumpPower;
        [SerializeField, BoxGroup("Dunk")] private float duration;
        private Tween dunk;
        private Vector3 nextpoint;
        
        private bool isSwapingRing;
        private InputAction swapInput;
        
        public void OnExit(PlayerOrbitalController orbitalController)
        {
            
        }

        public void ApplyMovement(PlayerOrbitalController orbitalController, float deltaTime)
        {
            Debug.Log(nextpoint);
            if (GetDistance(orbitalController) > 0)
            {
                dunk = orbitalController.transform.DOJump(nextpoint, jumpPower, 1, duration, false) .OnComplete(() =>
                {
                    Debug.Log("we have finished it i guess");
                    isSwapingRing = false;
                });
            }
        }
     
        public void OnEnter(PlayerOrbitalController orbitalController)
        {
            Vector2 angularVelocity = new Vector2(orbitalController.CurrentRing.GetAngularSpeed(distance),0) * Time.deltaTime;
            nextpoint = orbitalController.CurrentRing.GetPositionOnRing(orbitalController.transform.position, angularVelocity);
            nextpoint = OrbitalMath.ClampToRing(nextpoint, orbitalController.CurrentRing.transform.position, orbitalController.CurrentRing.RingData.Radius);
        }
        
        private float GetDistance(PlayerOrbitalController orbitalController)
        {
            return (orbitalController.transform.position - nextpoint).magnitude;
        }
        
        private void OnDunkPerfomed(InputAction.CallbackContext obj)
        {
            Debug.Log("wer are performing");
            if(!isSwapingRing)
            {
                Debug.Log("now it's true");
                isSwapingRing = true;
            }
        }
    }
}
using Orpheus.Core.Orbital;
using UnityEngine;

namespace Orpheus.Core.Rings
{
    public class Ring : MonoBehaviour
    {
        [field :SerializeField] public RingData RingData { get; private set; }
        private Floor floor;

        public void Initialize(Floor floor)
        {
            this.floor = floor;
        }
        
        public Ring GetNextLarger()
        {
            return floor.GetBiggerRing(this);
        }

        public Ring GetNextSmaller()
        {
            return floor.GetSmallerRing(this);
        }

        private float GetAngle(Vector3 position)
        {
            return OrbitalMath.GetAngleFromPosition(transform.position, position);
        }

        public float GetAngularSpeed(float force)
        {
            return force / RingData.Radius;
        }

        public Vector3 GetPositionOnRing(Vector3 position, Vector2 speed)
        {
            //move on the ring
            float angle = GetAngle(position); //get actual angle
            angle += speed.x; //calculate new angle based on speed
            Vector3 positionFromAngle = OrbitalMath.GetPositionFromAngle(this.transform.position, RingData.Radius, angle);
            positionFromAngle.y = position.y + speed.y;// Vertical position
            return positionFromAngle; //calculate new position
        }

        public Vector3 ClampToRing(Vector3 position)
        {
            return OrbitalMath.ClampToRing(position, transform.position, RingData.Radius);
        }

        public Vector3 GetNextPosition(Vector3 position, float velocityOnX)
        {
            return OrbitalMath.PositionAfterRotation(this.transform.position, position, velocityOnX, RingData.Radius);
        }
    }
}

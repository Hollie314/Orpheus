using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Orpheus.Core.Rings
{
    public class RingManager : MonoBehaviour
    {
        private RingData[] _ringDatas;
    
        public List<Floor> floors;

        public Ring GetRingOnSameFloor(int floorIndex, RingSize size)
        {
            if (floorIndex < 0 || floorIndex >= floors.Count) return null;

            return floors[floorIndex].rings
                .FirstOrDefault(r => r.RingData.Size == size);
        }

        public bool CanSwapTo(RingSize targetSize, int currentFloorIndex)
        {
            var rings = floors[currentFloorIndex].rings;

            bool hasSmall = rings.Any(r => r.RingData.Size == RingSize.small);
            bool hasLarge = rings.Any(r => r.RingData.Size == RingSize.large);
            bool hasMedium = rings.Any(r => r.RingData.Size == RingSize.medium);

            if ((hasSmall && hasLarge) && !hasMedium)
                return false;

            return rings.Any(r => r.RingData.Size == targetSize);
        }
    }
}

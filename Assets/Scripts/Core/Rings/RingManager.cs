using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RingManager : MonoBehaviour
{
    private RingData[] _ringDatas;
    
    public List<Floor> floors;

    public Ring GetRingOnSameFloor(int floorIndex, RingSize size)
    {
        if (floorIndex < 0 || floorIndex >= floors.Count) return null;

        return floors[floorIndex].rings
            .FirstOrDefault(r => r.size == size);
    }

    public bool CanSwapTo(RingSize targetSize, int currentFloorIndex)
    {
        var rings = floors[currentFloorIndex].rings;

        bool hasSmall = rings.Any(r => r.size == RingSize.small);
        bool hasLarge = rings.Any(r => r.size == RingSize.large);
        bool hasMedium = rings.Any(r => r.size == RingSize.medium);

        if ((hasSmall && hasLarge) && !hasMedium)
            return false;

        return rings.Any(r => r.size == targetSize);
    }
}

using UnityEngine;

public class Ring : MonoBehaviour
{
    private int floorIndex;
    public RingSize size { get; private set; }
    private float radius;
    
    public RingSize GetNextLarger(RingSize size)
    {
        if ((int)size < RingSize.GetValues(typeof(RingSize)).Length - 1)
            return (RingSize)((int)size + 1);
        return size;
    }

    public RingSize GetNextSmaller(RingSize size)
    {
        if ((int)size > 0)
            return (RingSize)((int)size - 1);
        return size;
    }
}

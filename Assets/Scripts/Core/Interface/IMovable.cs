namespace Orpheus.Core.Interface
{
    public interface IMovable
    {
        float GetAngle();
        void SetAngle(float angle);
        void MoveOnRing();
    }
}

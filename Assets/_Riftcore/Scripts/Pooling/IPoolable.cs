namespace Riftcore.Pooling
{
    public interface IPoolable
    {
        void OnGetFromPool();
        void OnReturnToPool();
    }
}
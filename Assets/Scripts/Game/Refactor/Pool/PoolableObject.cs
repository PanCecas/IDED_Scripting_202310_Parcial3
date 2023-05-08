using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    public delegate void OnObjectToRecycle(GameObject instance);
    public event OnObjectToRecycle onObjectToRecycle;

    private IPoolable pool;

    public void RegisterToPool(IPoolable pool)
    {
        this.pool = pool;
        onObjectToRecycle += pool.RecycleInstance;
    }

    public void UnregisterFromPool()
    {
        onObjectToRecycle -= pool.RecycleInstance;
        pool = null;
    }

    public void RecycleObject()
    {
        onObjectToRecycle?.Invoke(gameObject);
    }
}

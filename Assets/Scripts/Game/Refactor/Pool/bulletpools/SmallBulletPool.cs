using System.Collections.Generic;
using UnityEngine;

public class SmallBulletPool : PoolableObject
{
    [SerializeField]
    private int smallBulletCount = 0;

    [SerializeField]
    private GameObject smallBulletPrefab;

    private void Awake()
    {
        base.count = smallBulletCount;
        base.basePrefab = smallBulletPrefab;
        base.PopulatePool();
    }
    public override GameObject RetrieveInstance()
    {
        GameObject instance = base.RetrieveInstance();
        PoolableObject poolableObject = instance.GetComponent<PoolableObject>();
        poolableObject.RegisterToPool(this);
        return instance;
    }

    public override void RecycleInstance(GameObject instance)
    {
        PoolableObject poolableObject = instance.GetComponent<PoolableObject>();
        poolableObject.UnregisterFromPool();
        base.RecycleInstance(instance);
    }
}

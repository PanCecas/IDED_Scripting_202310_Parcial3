using System.Collections.Generic;
using UnityEngine;

public class LargeBulletPool : PoolableObject
{
    [SerializeField]
    private int largeBulletCount = 0;

    [SerializeField]
    private GameObject largeBulletPrefab;

    private void Awake()
    {
        base.count = largeBulletCount;
        base.basePrefab = largeBulletPrefab;
        base.PopulatePool();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        foreach (var instance in this)
        {
            instance.GetComponent<PoolableObject>().onObjectToRecycle -= OnObjectToRecycle;
        }
    }

    protected override void Reinitialize(GameObject obj)
    {
        base.Reinitialize(obj);
        obj.GetComponent<PoolableObject>().onObjectToRecycle += OnObjectToRecycle;
    }

    private void OnObjectToRecycle(GameObject instance)
    {
        Release(instance);
    }
}

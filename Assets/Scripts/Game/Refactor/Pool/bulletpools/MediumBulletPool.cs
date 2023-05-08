using System.Collections.Generic;
using UnityEngine;

public class MediumBulletPool : PoolableObject
{
    [SerializeField]
    private int mediumBulletCount = 0;

    [SerializeField]
    private GameObject mediumBulletPrefab;

    private void Awake()
    {
        base.count = mediumBulletCount;
        base.basePrefab = mediumBulletPrefab;
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

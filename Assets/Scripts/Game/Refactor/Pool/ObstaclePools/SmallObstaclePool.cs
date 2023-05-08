using System.Collections.Generic;
using UnityEngine;

public class SmallObstaclePool : MonoBehaviour
{
    [SerializeField]
    private GameObject obstaclePrefab;

    [SerializeField]
    private int poolSize = 10;

    private Queue<GameObject> obstaclePool = new Queue<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obstacle = Instantiate(obstaclePrefab, transform);
            obstacle.SetActive(false);
            obstaclePool.Enqueue(obstacle);
        }
    }

    public GameObject RetrieveInstance()
    {
        if (obstaclePool.Count == 0)
        {
            GameObject obstacle = Instantiate(obstaclePrefab, transform);
            obstacle.SetActive(false);
            obstaclePool.Enqueue(obstacle);
        }

        GameObject retrievedObstacle = obstaclePool.Dequeue();
        retrievedObstacle.SetActive(true);
        return retrievedObstacle;
    }

    public void ReturnInstance(GameObject obstacleInstance)
    {
        obstacleInstance.SetActive(false);
        obstaclePool.Enqueue(obstacleInstance);
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

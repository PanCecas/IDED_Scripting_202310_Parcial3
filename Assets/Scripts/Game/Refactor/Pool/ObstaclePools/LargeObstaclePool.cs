using System.Collections.Generic;
using UnityEngine;

public class LargeObstaclePool : ObstaclePool
{
    [SerializeField]
    private LargeObstacle largeObstaclePrefab;

    [SerializeField]
    private int poolSize = 5;

    private Queue<LargeObstacle> obstacleQueue = new Queue<LargeObstacle>();

    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < poolSize; i++)
        {
            LargeObstacle obstacle = Instantiate(largeObstaclePrefab);
            obstacle.gameObject.SetActive(false);
            obstacleQueue.Enqueue(obstacle);
        }
    }

    public override Obstacle RetrieveInstance()
    {
        if (obstacleQueue.Count == 0)
        {
            LargeObstacle obstacle = Instantiate(largeObstaclePrefab);
            obstacle.gameObject.SetActive(false);
            obstacleQueue.Enqueue(obstacle);
        }

        Obstacle retrievedObstacle = obstacleQueue.Dequeue();
        retrievedObstacle.gameObject.SetActive(true);
        return retrievedObstacle;
    }

    public override void ReturnInstance(Obstacle obstacle)
    {
        obstacle.gameObject.SetActive(false);
        obstacleQueue.Enqueue((LargeObstacle)obstacle);
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

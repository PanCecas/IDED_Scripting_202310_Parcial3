using UnityEngine;

public class MediumObstaclePool : ObjectPool<ObstacleController>
{
    [SerializeField]
    private ObstacleController mediumObstaclePrefab;

    protected override ObstacleController CreateNewInstance()
    {
        ObstacleController obstacle = Instantiate(mediumObstaclePrefab);
        obstacle.gameObject.SetActive(false);
        return obstacle;
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


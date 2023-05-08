using UnityEngine;

public sealed class ObstacleSpawner : ObstacleSpawnerBase
{
    [SerializeField]
    private GameObject[] obstaclePrefabs;

    protected GameObject[] ObstaclePrefabs { get => obstaclePrefabs; }

    protected int ObjectIndex
    {
        get
        {
            int result = 0;

            if (obstaclePrefabs.Length > 1)
            {
                result = Random.Range(result, obstaclePrefabs.Length);
            }

            return result;
        }
    }

    protected override void SpawnObject()
    {
        // Instanciamos un objeto de la piscina de objetos
        GameObject obstacleInstance = obstaclePool.GetNextObject();

        // Posicionamos el obstáculo en una posición aleatoria en el eje x
        Vector3 spawnPosition = new Vector3(Random.Range(MinX, MaxX), YPos, 0f);
        obstacleInstance.transform.position = spawnPosition;

        // Activamos la instancia
        obstacleInstance.SetActive(true);
    }
}

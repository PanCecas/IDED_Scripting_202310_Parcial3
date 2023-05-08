using UnityEngine;
using UnityEngine.Pool;

public class RefactoredObstacleSpawner : ObstacleSpawnerBase
{
    [SerializeField] private SmallObstaclePool smallObstaclePool;
    [SerializeField] private MediumObstaclePool mediumObstaclePool;
    [SerializeField] private LargeObstaclePool largeObstaclePool;

    private int randomRange;
    int newMinX;

    public static RefactoredObstacleSpawner Instance { get { return instance; } }
    private static RefactoredObstacleSpawner instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Update()
    {
        randomRange = Random.Range(0, 3);
        newMinX = Random.Range(-6, 6);
    }

    protected override void Start()
    {
        base.Start();
        RefactoredGameController.GameOverEvent += OnGameOver;
    }

    protected override void SpawnObject()
    {
        switch (randomRange)
        {
            case 0:
                GameObject obstacle1 = smallObstaclePool.Get();
                obstacle1.transform.position = new Vector2(newMinX, YPos);
                break;

            case 1:
                GameObject obstacle2 = mediumObstaclePool.Get();
                obstacle2.transform.position = new Vector2(newMinX, YPos);
                break;

            case 2:
                GameObject obstacle3 = largeObstaclePool.Get();
                obstacle3.transform.position = new Vector2(newMinX, YPos);
                break;
        }
    }
}

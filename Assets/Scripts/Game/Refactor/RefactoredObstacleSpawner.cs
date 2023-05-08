using UnityEngine;
using UnityEngine.Pool;

public class RefactoredObstacleSpawner : ObstacleSpawnerBase
{
    [SerializeField] private SmallObstaclePool smallObstaclePool;
    [SerializeField] private MediumObstaclePool mediumObstaclePool;
    [SerializeField] private LargeObstaclePool largeObstaclePool;

    private int radomRange;
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
        radomRange = Random.Range(0, 3);
        newMinX = Random.Range(-6, 6);
    }

    protected override void Start()
    {
        base.Start();
        RefactoredGameController.GameOverEvent += OnGameOver;
    }

    protected override void SpawnObject()
    {
        switch (radomRange)
        {
            case 0:
                GameObject obstacle1 = smallObstaclePool.RetrieveInstance();
                obstacle1.transform.position = new Vector2(newMinX, YPos);
                break;

            case 1:
                GameObject obstacle2 = mediumObstaclePool.RetrieveInstance();
                obstacle2.transform.position = new Vector2(newMinX, YPos);
                break;

            case 2:
                GameObject obstacle3 = largeObstaclePool.RetrieveInstance();
                obstacle3.transform.position = new Vector2(newMinX, YPos);
                break;
        }
    }
}

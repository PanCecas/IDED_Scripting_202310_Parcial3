using UnityEngine;
using UnityEngine.Events;

public sealed class RefactoredGameController : GameControllerBase
{
    private static RefactoredGameController instance;
    public static RefactoredGameController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<RefactoredGameController>();

                if (instance == null)
                {
                    var singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<RefactoredGameController>();
                    singletonObject.name = typeof(RefactoredGameController).ToString() + " (Singleton)";

                    DontDestroyOnLoad(singletonObject);
                }
            }

            return instance;
        }
    }

    [SerializeField] private UnityEvent<int> onObstacleDestroyed = new UnityEvent<int>();
    [SerializeField] private UnityEvent onGameOver = new UnityEvent();

    protected override PlayerControllerBase PlayerController => playerController;
    protected override UIManagerBase UiManager => uiManager;
    protected override ObstacleSpawnerBase Spawner => spawner;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private RefactoredObstacleSpawner spawner;

    private RefactoredGameController() { }

    protected override void OnScoreChanged(int hp)
    {
        playerController?.SendMessage("UpdateScore", hp);
        uiManager?.SendMessage("UpdateScoreLabel");
    }

    protected override void EndGame()
    {
        base.EndGame();
        onGameOver.Invoke();
    }

    public void ObstacleDestroyed(int hp)
    {
        onObstacleDestroyed.Invoke(hp);
    }
}

public class RefactoredObstacleSpawner : ObstacleSpawnerBase
{
    [SerializeField] private Obstacle obstaclePrefab;
    [SerializeField] private float spawnDelay = 1f;

    private Coroutine spawningCoroutine;

    protected override void StartSpawning()
    {
        if (spawningCoroutine == null)
        {
            spawningCoroutine = StartCoroutine(SpawningCoroutine());
        }
    }

    protected override void StopSpawning()
    {
        if (spawningCoroutine != null)
        {
            StopCoroutine(spawningCoroutine);
            spawningCoroutine = null;
        }
    }

    private IEnumerator SpawningCoroutine()
    {
        while (true)
        {
            Instantiate(obstaclePrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void OnEnable()
    {
        RefactoredGameController.Instance.ObstacleDestroyed += OnObstacleDestroyed;
    }

    private void OnDisable()
    {
        RefactoredGameController.Instance.ObstacleDestroyed -= OnObstacleDestroyed;
    }

    private void OnObstacleDestroyed(int hp)
    {
        if (Random.value < 0.5f)
        {
            spawnDelay *= 0.95f;
        }
        else
        {
            spawnDelay /= 0.95f;
        }

        spawnDelay = Mathf.Clamp(spawnDelay, 0.1f, 10f);

        if (hp > 1)
        {
            Instantiate(obstaclePrefab, transform.position, Quaternion.identity);
        }
    }
    protected override void EndGame()
    {
        base.EndGame();
        onGameOver.Invoke();
    }

    public void OnGameOver()
    {
        EndGame();
    }
}


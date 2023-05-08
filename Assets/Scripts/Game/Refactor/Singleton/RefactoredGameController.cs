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

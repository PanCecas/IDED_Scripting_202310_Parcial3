using UnityEngine;
using UnityEngine.UI;


public sealed class RefactoredUIManager : UIManagerBase
{
    private static RefactoredUIManager instance;

    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private GameObject gameOverPanel;

    protected override PlayerControllerBase PlayerController => GameManager.Instance.PlayerController;

    protected override GameControllerBase GameController => GameManager.Instance.GameController;

    public static RefactoredUIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<RefactoredUIManager>();

                if (instance == null)
                {
                    Debug.LogError("There's no active RefactoredUIManager in the scene.");
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        RefactoredGameController.Instance.OnObstacleDestroyed.AddListener(UpdateScoreText);
        RefactoredGameController.Instance.OnGameOver.AddListener(GameOver);

        RefactoredPlayerController.Instance.OnScoreChangedEvent.AddListener(UpdateScoreText);
        RefactoredPlayerController.Instance.OnBulletSelected.AddListener(UpdateBulletUI);
    }

    private void OnDestroy()
    {
        RefactoredGameController.Instance.OnObstacleDestroyed.RemoveListener(UpdateScoreText);
        RefactoredGameController.Instance.OnGameOver.RemoveListener(GameOver);

        RefactoredPlayerController.Instance.OnScoreChangedEvent.RemoveListener(UpdateScoreText);
        RefactoredPlayerController.Instance.OnBulletSelected.RemoveListener(UpdateBulletUI);
    }

    private void UpdateScoreText(int score)
    {
        scoreText.text = score.ToString();
    }

    private void UpdateBulletUI(int index)
    {
        // Implementación del método
    }

    private void GameOver()
    {
        gameOverPanel.SetActive(true);
    }
}

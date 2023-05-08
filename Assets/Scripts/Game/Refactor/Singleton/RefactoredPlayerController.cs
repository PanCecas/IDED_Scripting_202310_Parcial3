public class RefactoredPlayerController : PlayerControllerBase
{
    public event Action<int> OnScoreChangedEvent;
    public event Action<int> OnBulletSelectedEvent;

    private static RefactoredPlayerController instance;

    public static RefactoredPlayerController Instance
    {
        get
        {
            if (instance == null)
            {
                // Intentamos obtener una instancia previa del PlayerController original
                PlayerController originalPlayerController = FindObjectOfType<PlayerController>();

                if (originalPlayerController == null)
                {
                    // Si no encontramos una instancia del PlayerController original, lo instanciamos
                    GameObject originalPlayerControllerGO = new GameObject("PlayerController");
                    originalPlayerController = originalPlayerControllerGO.AddComponent<PlayerController>();
                }

                // Creamos una instancia del RefactoredPlayerController y copiamos los valores del original
                GameObject refactoredPlayerControllerGO = new GameObject("RefactoredPlayerController");
                instance = refactoredPlayerControllerGO.AddComponent<RefactoredPlayerController>();

                instance.speed = originalPlayerController.speed;
                instance.bulletPrefabs = originalPlayerController.bulletPrefabs;
                instance.spawnPos = originalPlayerController.spawnPos;
                instance.shootForce = originalPlayerController.shootForce;

                // Destruimos el PlayerController original
                Destroy(originalPlayerController.gameObject);

                // Mantenemos el RefactoredPlayerController entre escenas
                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    public void OnScoreChanged(int scoreAdd)
    {
        OnScoreChangedEvent?.Invoke(scoreAdd);
    }

    public void OnBulletSelected(int index)
    {
        OnBulletSelectedEvent?.Invoke(index);
    }
}

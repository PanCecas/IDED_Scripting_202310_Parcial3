using System.Collections.Generic;
using UnityEngine;

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

    // Implementación de la función Shoot utilizando el pool de balas
    public override void Shoot()
    {
        // Obtener una bala del pool
        GameObject bullet = BulletPool.Instance.GetFromPool();

        // Posicionar la bala en el punto de spawn
        bullet.transform.position = spawnPos.position;
        bullet.transform.rotation = spawnPos.rotation;

        // Agregar fuerza a la bala
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.AddForce(shootForce * spawnPos.up, ForceMode2D.Impulse);

        // Llamar al evento de selección de bala
        OnBulletSelectedEvent?.Invoke(currentBulletIndex);

        // Restar una bala al contador
        bulletsLeft[currentBulletIndex]--;

        // Si se acabaron las balas de este tipo, cambiar de tipo
        if (bulletsLeft[currentBulletIndex] <= 0)
        {
            currentBulletIndex = (currentBulletIndex + 1) % bulletPrefabs.Length;
        }
    }
}

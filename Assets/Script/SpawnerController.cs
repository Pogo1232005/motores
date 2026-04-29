using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class SpawnerController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject enemyVariant;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject Enemy;

    [Header("Settings")]
    [SerializeField] float maxTime = 3f;
    [SerializeField] float spawnRadius = 5f;
    [SerializeField] float spawnHeight = 1.11f; // Altura fija solicitada
    [SerializeField] int maxEnemies = 10;      // Límite total de enemigos
    [SerializeField] float SpawnRange = 2f;
    [SerializeField] float SpawnCooldown = 0.1f;
    [SerializeField] bool stopSpawner = false;


    [Range(0f, 1f)]
    [SerializeField] float variantProbability = 0.3f;

    public bool canSpawn = true;
    private int currentEnemyCount = 0; // Contador interno
    //private GameObject player;

    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");

        if (Player == null)
            Debug.LogError("No se encontró el Player (tag 'Player')");

        StartCoroutine(Timer());
        StartCoroutine(Cooldown());
    }
    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(SpawnCooldown);
        while (!stopSpawner)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(SpawnCooldown);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        // Dibujamos el círculo a la altura de spawn para previsualizar mejor
        Vector3 center = new Vector3(transform.position.x, spawnHeight, transform.position.z);
        Gizmos.DrawWireSphere(center, spawnRadius);
    }

    IEnumerator Timer()
    {
        // El bucle ahora también revisa si no hemos llegado al máximo
        while (canSpawn)
        {
            if (currentEnemyCount < maxEnemies)
            {
                yield return new WaitForSeconds(maxTime);

                // 1. Calcular posición aleatoria (X y Z para 3D)
                Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;

                // Forzamos la altura (Y) a 1.11f
                Vector3 spawnPosition = new Vector3(
                    transform.position.x + randomCircle.x,
                    spawnHeight,
                    transform.position.z + randomCircle.y
                );

                // 2. Elegir enemigo
                GameObject enemyToSpawn = (Random.value < variantProbability) ? enemyVariant : enemyPrefab;

                // 3. Instanciar
                GameObject newEnemy = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
                currentEnemyCount++; // Aumentamos el contador

                // 4. Asignar target
                if (newEnemy.TryGetComponent<EnemyController>(out EnemyController controller) && Player != null)
                {
                    controller.SetTarget(Player);
                }
            }
            else
            {
                // Si llegamos al máximo, esperamos un poco antes de volver a chequear
                // (Por si quieres que spawneen más cuando mueran los actuales)
                yield return new WaitForSeconds(1f);
            }
        }
    }
    void SpawnEnemy()
    {
        float distance = Random.Range(0, SpawnRange);
        float angle = Random.Range(0, 360);
        Vector3 newPosition = transform.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;
        GameObject spawn = Instantiate(Enemy, newPosition, Quaternion.identity);
        spawn.GetComponent<EnemyController>().SetTarget(Player);
    }

    // Método opcional: Llama a esto desde el script de muerte del enemigo para liberar espacio
    public void EnemyDied()
    {
        currentEnemyCount--;
    }
}

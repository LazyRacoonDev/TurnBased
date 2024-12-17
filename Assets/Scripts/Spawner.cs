using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] faction1Prefabs; // Lista de prefabs para faction1
    public GameObject[] faction2Prefabs; // Lista de prefabs para faction2

    public Vector3 faction1SpawnZone = new Vector3(-11.17f, 0.26f, 9.99f); // Zona de spawn para faction1
    public Vector3 faction2SpawnZone = new Vector3(11.78f, 0.26f, -11.47f); // Zona de spawn para faction2

    public float spawnRadius = 3f; // Radio de dispersión alrededor del punto de spawn
    public int unitsPerFaction = 5; // Número de unidades por facción

    void Start()
    {
        // Generar personajes para faction1
        SpawnFaction(faction1Prefabs, faction1SpawnZone);

        // Generar personajes para faction2
        SpawnFaction(faction2Prefabs, faction2SpawnZone);
    }

    void SpawnFaction(GameObject[] prefabs, Vector3 spawnZone)
    {
        if (prefabs == null || prefabs.Length == 0)
        {
            Debug.LogWarning("No hay prefabs asignados para la facción.");
            return;
        }

        for (int i = 0; i < unitsPerFaction; i++)
        {
            // Seleccionar un prefab aleatorio
            GameObject randomPrefab = prefabs[Random.Range(0, prefabs.Length)];

            // Generar una posición aleatoria dentro del radio de spawn
            Vector3 randomPosition = spawnZone + new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                0f,
                Random.Range(-spawnRadius, spawnRadius)
            );

            // Instanciar el prefab en la posición aleatoria
            Instantiate(randomPrefab, randomPosition, Quaternion.identity);
        }
    }
}
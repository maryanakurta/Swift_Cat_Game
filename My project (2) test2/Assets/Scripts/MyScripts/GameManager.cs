using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public List<Transform> playerTransforms;
    public Transform spawnPoint;

    bool gameStarted = false;
    int score = 0;
    int highScore = 0;

    public static GameManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("highScore"))
        {
            highScore = PlayerPrefs.GetInt("highScore");
        }

        StartCoroutine("SpawnEnemies");
        gameStarted = true;
    }

    void Update()
    {
        UpdateSpawnPointPosition();
    }

    void UpdateSpawnPointPosition()
    {
        if (playerTransforms.Count == 0)
        {
            Debug.LogWarning("No player transforms are assigned.");
            return;
        }

        Vector3 weightedAveragePosition = Vector3.zero;
        float totalWeight = 0f;

        foreach (Transform playerTransform in playerTransforms)
        {
            float distanceToPlayer = Vector3.Distance(spawnPoint.position, playerTransform.position);
            float weight = 1f / Mathf.Pow(distanceToPlayer + 1f, 2f);

            weightedAveragePosition += playerTransform.position * weight;
            totalWeight += weight;
        }

        if (totalWeight > 0f)
        {
            weightedAveragePosition /= totalWeight;
        }

        spawnPoint.position = new Vector3(weightedAveragePosition.x, spawnPoint.position.y, weightedAveragePosition.z + 100f);
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.8f);
            Spawn();
        }
    }

    public void Spawn()
    {
        if (enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("No enemy prefabs are assigned.");
            return;
        }

        int randomPrefabIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject selectedEnemyPrefab = enemyPrefabs[randomPrefabIndex];

        float randomSpawnX = Random.Range(-4f, 4f);
        Vector3 enemySpawnPos = spawnPoint.position;
        enemySpawnPos.x = randomSpawnX;

        Instantiate(selectedEnemyPrefab, enemySpawnPos, Quaternion.identity);
    }

    public void Restart()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("highScore", highScore);
        }
        StartCoroutine("SpawnEnemies");
        SceneManager.LoadScene(0);
    }

    public void ScoreUp()
    {
        score++;
    }
}

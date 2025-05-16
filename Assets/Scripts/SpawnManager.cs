using Mono.Cecil;
using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Enemy Spawn info")]
    [SerializeField] 
    private GameObject[] enemyPrefab;
    [SerializeField] 
    private Transform enemyContainer;
    [Header("Powerup Spawn info")]
    [SerializeField]
    private GameObject[] powerUpPrefabs;
    [SerializeField]
    private int[] _powerUpWeights;
    [SerializeField]
    private Transform powerUpContainer;

    [Header("General spawn settings")]
    [SerializeField]
    private bool canSpawn = true;
    
    [Header("Spawn Area")]
    [Tooltip("X: lower bounds of the random range, Y: upper bounds of the random range")]
    [SerializeField]
    private Vector2 spawnXRange;
    [SerializeField]
    private float spawnY;

    [Header("Wave Settings")]
    [SerializeField]
    int _waveCount = 0;
    [SerializeField]
    int _enemiesInWave;
    [SerializeField]
    int _currentEnemies = 0;
    [SerializeField]
    int _spawnedEnemies;
    [SerializeField]
    int _finalWave = 2;

    int _totalWeight;

    private static SpawnManager instance;
    public static SpawnManager Instance
    {
        get
        {
            if (instance == null)
                Debug.LogError("UI Manager private instance is Null!");
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    private void Start()
    {
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerupSpawnRoutine());

        //PUPs calculations
        _totalWeight = 0;
        foreach (int i in _powerUpWeights)
            _totalWeight += i;
        for (int j = 0; j < _powerUpWeights.Length; j++)
            Debug.Log($"Powerup: {powerUpPrefabs[j].GetComponent<Powerup>().GetPowerup().ToString()} : {(float)_powerUpWeights[j] / (float)_totalWeight}");
    }

    IEnumerator EnemySpawnRoutine()
    {
        while(canSpawn && _waveCount < _finalWave)
        {
            yield return new WaitForSeconds(5);
            _waveCount++;
            _enemiesInWave = _waveCount * 10;
            _currentEnemies = 0;
            _spawnedEnemies = 0;
            UIManager.Instance.UpdateWaveText(_waveCount);
            while (_currentEnemies < _enemiesInWave && canSpawn)
            {
                float randX = Random.Range(spawnXRange.x, spawnXRange.y);
                GameObject enemyToSpawn = enemyPrefab[Random.Range(0, enemyPrefab.Length)];
                Instantiate(enemyToSpawn, new Vector3(randX, spawnY, 0), Quaternion.identity, enemyContainer);
                _currentEnemies++;
                yield return new WaitForSeconds(3);
            }
            while(_currentEnemies > 0)
            {
                yield return null;
            }
        }
    }

    IEnumerator PowerupSpawnRoutine()
    {
        while (canSpawn)
        {
            float randX = Random.Range(spawnXRange.x, spawnXRange.y);
            int randPrefab = Random.Range(0, powerUpPrefabs.Length);
            Instantiate(powerUpPrefabs[randPrefab], new Vector3(randX, spawnY, 0), Quaternion.identity, powerUpContainer);
            yield return new WaitForSeconds(3);
        }
    }

    public void OnPlayerDeath()
    {
        canSpawn = false;
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        UIManager.Instance.OnPlayerDeath();
        foreach(Enemy enemy in enemies)
        {
            enemy.OnPlayerDeath();
        }
    }

    public void OnEnemyDeath()
    {
        _currentEnemies--;
    }

}

using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Enemy Spawn info")]
    [SerializeField] 
    private GameObject enemyPrefab;
    [SerializeField] 
    private Transform enemyContainer;
    [Header("Powerup Spawn info")]
    [SerializeField]
    private GameObject[] powerUpPrefabs;
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

    private void Start()
    {
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerupSpawnRoutine());
    }

    IEnumerator EnemySpawnRoutine()
    {
        while(canSpawn)
        {
            float randX = Random.Range(spawnXRange.x, spawnXRange.y);
            Instantiate(enemyPrefab, new Vector3(randX, spawnY, 0), Quaternion.identity, enemyContainer);
            yield return new WaitForSeconds(3);
        }
    }

    IEnumerator PowerupSpawnRoutine()
    {
        while (canSpawn)
        {
            float randX = Random.Range(spawnXRange.x, spawnXRange.y);
            Instantiate(powerUpPrefabs[0], new Vector3(randX, spawnY, 0), Quaternion.identity, powerUpContainer);
            yield return new WaitForSeconds(3);
        }
    }

    public void OnPlayerDeath()
    {
        canSpawn = false;
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        
        foreach(Enemy enemy in enemies)
        {
            enemy.OnPlayerDeath();
        }
    }

}

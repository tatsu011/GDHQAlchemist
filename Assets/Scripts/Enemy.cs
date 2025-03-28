using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Attributes")]
    [SerializeField]
    float speed = 5f;
    [SerializeField]
    int pointValue = 100;
    [SerializeField]
    float _explosionDelay = 0.33f;
    [Header("Screen bounds")]
    [SerializeField]
    float upperBounds = 12f;
    [SerializeField]
    float lowerBounds = -12f;
    [SerializeField]
    float rightBounds = 17f;
    [SerializeField]
    float leftBounds = -17f;
    [SerializeField]
    bool canRespawn = true;
    [SerializeField]
    GameObject _explosionPrefab;
    [SerializeField]
    GameObject laserPrefab;
    [SerializeField]
    Transform laserSpawner;
    private Player player;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindAnyObjectByType<Player>();
        StartCoroutine(FireLaserCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();


    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * (speed * Time.deltaTime));
        if (transform.position.y < lowerBounds && canRespawn)
        {
            Respawn();
            return;
        }
        if(transform.position.y < lowerBounds)
        {
            Destroy(gameObject);
        }
    }

    private void Respawn()
    {
        float rng = Random.Range(leftBounds, rightBounds);
        transform.position = new Vector3(rng, upperBounds, 0);
    }

    IEnumerator FireLaserCoroutine()
    {
        while(player!= null)
        {
            Instantiate(laserPrefab, laserSpawner.position, Quaternion.identity);
            float randomRefireTime = Random.Range(2, 5);
            yield return new WaitForSeconds(randomRefireTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player.Damage();
            OnEnemyDeath();
        }
        if(other.CompareTag("playerProjectile"))
        {
            //damage self.
            Destroy(other.gameObject);
            OnEnemyDeath();

        }
    }

    private void OnEnemyDeath()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        player.AddScore(pointValue);
        GetComponent<Collider>().enabled = false;
        enabled = false;
        Destroy(gameObject, .33f);
    }

    public void OnPlayerDeath()
    {
        canRespawn = false;
    }


}

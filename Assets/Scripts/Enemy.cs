using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Attributes")]
    [SerializeField]
    float speed = 5f;
    [SerializeField]
    int pointValue = 100;

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

    private Player player;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindAnyObjectByType<Player>();
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player.Damage();
            player.AddScore(pointValue);
            Destroy(gameObject);
        }
        if(other.CompareTag("playerProjectile"))
        {
            //damage self.
            player.AddScore(pointValue);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    public void OnPlayerDeath()
    {
        canRespawn = false;
    }


}

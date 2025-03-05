using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Player stats")]
    [SerializeField]
    float speed = 5f;

    [Header("Screen bounds")]
    [SerializeField]
    float upperBounds = 5f;
    [SerializeField]
    float lowerBounds = -5f;
    [SerializeField]
    float rightBounds = 12f;
    [SerializeField]
    float leftBounds = -12f;
    [SerializeField]
    bool wrapAround = true;

    [Header("Laser Settings")]
    [SerializeField] 
    private GameObject laserPrefab;
    [SerializeField]
    private Transform laserContainer;
    [SerializeField]
    private float fireRate = 2.5f;
    [SerializeField]
    float _whenCanFire = -1;

    [SerializeField]
    private int health = 3;

    private SpawnManager spawnManager;

    Vector3 position;
    Vector3 motion;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnManager = GameObject.FindAnyObjectByType<SpawnManager>();
        //TODO: GameMaster object.
        if (spawnManager == null)
            Debug.Log("Spawn Manager is null!", this);
    }

    // Update is called once per frame
    void Update()
    {
        DoMovement();

        BoundsCheck();

        if (Input.GetKey(KeyCode.Space) && _whenCanFire < Time.time)
        {
            FireLaser();
        }
    }

    private void FireLaser()
    {
        Instantiate(laserPrefab, transform.position, Quaternion.identity, laserContainer.transform);
        _whenCanFire = Time.time + fireRate;
    }

    private void BoundsCheck()
    {
        position = transform.position;

        if (transform.position.y < lowerBounds)
            position.y = lowerBounds;
        if (transform.position.y > upperBounds)
            position.y = upperBounds;
        if (transform.position.x < leftBounds)
            position.x = wrapAround ? rightBounds : leftBounds;
        if (transform.position.x > rightBounds)
            position.x = wrapAround ? leftBounds : rightBounds;
        transform.position = position;
    }

    public void UpdateBounds(Vector4 newBounds)
    {
        //left, right, upper, lower.
        leftBounds = newBounds[0];
        rightBounds = newBounds[1];
        upperBounds = newBounds[2];
        lowerBounds = newBounds[3];
    }

    private void DoMovement()
    {
        //Vector3 motion = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        motion.x = Input.GetAxis("Horizontal");
        motion.y = Input.GetAxis("Vertical");
        motion.z = 0f;
        transform.Translate(motion * (Time.deltaTime * speed));
    }

    public void Damage()
    {
        health--;
        if(health <= 0)
        {
            spawnManager.OnPlayerDeath();
            Destroy(gameObject);
        }
    }
}

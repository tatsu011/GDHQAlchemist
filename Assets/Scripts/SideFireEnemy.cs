using UnityEngine;

public class SideFireEnemy : MonoBehaviour
{
    [SerializeField]
    float _speed = 5f;
    [Header("Screen bounds")]
    [SerializeField]
    float rightBounds = 12f;
    [SerializeField]
    float leftBounds = -12f;
    [SerializeField]
    float _upperBounds = 9f;
    [SerializeField]
    float _lowerbounds = -3f;
    [Header("Laser Settings")]
    [SerializeField]
    Transform _firingPoint;
    [SerializeField]
    GameObject _laserPrefab;
    [SerializeField]
    float _laserCooldown = 2.5f;
    [SerializeField]
    Vector3 _laserRotationAdjustments = new Vector3(0, 0, 90);
    [SerializeField]
    GameObject _explosionPrefab;
    [SerializeField]
    private int pointValue;

    float _whenCanFire;
    int _directionMultiplier = 1;
    Vector3 scale = new Vector3(-1, 1, 1);
    Player player;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindAnyObjectByType<Player>();
        _whenCanFire = Time.time + _laserCooldown;

    }

    // Update is called once per frame
    void Update()
    {
        _firingPoint.LookAt(player.transform, Vector3.forward);

        DoMovement();
        if (Time.time > _whenCanFire)
            FireLaser();
    }

    private void ResetSpawmn()
    {
        float randy = Random.Range(_lowerbounds, _upperBounds);
        float x = leftBounds + .05f;
        
    }

    private void FireLaser()
    {
        GameObject prefab = Instantiate(_laserPrefab, _firingPoint.position, _firingPoint.rotation);
        prefab.transform.Rotate(_laserRotationAdjustments);
        _whenCanFire = Time.time + _laserCooldown;
    }

    private void DoMovement()
    {
        transform.Translate(Vector3.right * (_speed * Time.deltaTime * _directionMultiplier));
        if (transform.position.x < leftBounds && _directionMultiplier < 0 || transform.position.x > rightBounds && _directionMultiplier > 0)
        {
            _directionMultiplier *= -1;
            scale.x = transform.localScale.x * -1;
            scale.y = transform.localScale.y;
            scale.z = transform.localScale.z;
            transform.localScale = scale;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.Damage();
            OnEnemyDeath();
        }
        if (other.CompareTag("playerProjectile"))
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
}

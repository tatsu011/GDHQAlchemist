using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Player stats")]


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
    private GameObject doubleshotPrefab;
    [SerializeField]
    private float fireRate = 2.5f;
    [SerializeField]
    float _whenCanFire = -1;
    [SerializeField]
    private AudioClip _laserSound;

    [Header("Health and Shield Settings")]
    [SerializeField]
    private int health = 3;
    [SerializeField]
    private int shieldHealth = 3;
    [SerializeField]
    private int maxShieldHealth = 3;
    [SerializeField]
    ShieldVisuals shieldPart;
    [SerializeField]
    private GameObject[] _damageVisuals;

    [Header("Speed settings")]
    [SerializeField]
    float speedBoostMultiplier = 2f;
    [SerializeField]
    float boostedMultiplier = 1f;
    [SerializeField]
    float speed = 5f;

    [Header("Powerup settings")]
    [SerializeField]
    bool fireDoubleShot = false;

    [SerializeField]
    bool shieldsActive = false;

    [SerializeField]
    bool boostActive = false;

    [SerializeField]
    private int _score;

    private SpawnManager spawnManager;

    private Coroutine doubleShotPowerupRoutine;

    private Coroutine speedPowerupRoutine;



    Vector3 position;
    Vector3 motion;
    



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnManager = FindAnyObjectByType<SpawnManager>();
        //TODO: GameMaster object.
        if (spawnManager == null)
            Debug.Log("Spawn Manager is null!", this);
        UIManager.Instance.UpdateScore(_score);
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
        if (fireDoubleShot)
        {
            Instantiate(doubleshotPrefab, transform.position, Quaternion.identity, laserContainer.transform);
            _whenCanFire = Time.time + fireRate;
        }
        else
        {
            Instantiate(laserPrefab, transform.position, Quaternion.identity, laserContainer.transform);
            _whenCanFire = Time.time + fireRate;
        }
        AudioManager.Instance.PlaySoundAtPlayer(_laserSound);
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
        transform.Translate(motion * (Time.deltaTime * boostedMultiplier * speed));
    }

    public void Damage()
    {
        if(shieldsActive)
        {
            shieldHealth--;
            shieldPart.DamageShields();
            UIManager.Instance.UpdateShield(shieldHealth);
            if (shieldHealth <= 0)
            {
                shieldsActive = false;
                ShieldActive(false);
            }
                return;
        }
        health--;


        int rng = Random.Range(0, _damageVisuals.Length);
        if (health == 2)
        {
            _damageVisuals[rng].SetActive(true);
        }
        if (health == 1)
        {
            if (_damageVisuals[0].activeInHierarchy == true)
                _damageVisuals[1].SetActive(true);
            else
                _damageVisuals[0].SetActive(true);
        }
        UIManager.Instance.UpdateHealth(health);
        if (health <= 0)
        {
            spawnManager.OnPlayerDeath();
            transform.GetChild(0).gameObject.SetActive(false);
            this.enabled = false;
            GetComponent<Collider>().enabled = false;

            Destroy(gameObject, 1.0f);
        }
    }

    public void ShieldActive(bool isActive)
    {
        shieldPart.gameObject.SetActive(isActive);
        shieldsActive = isActive;
        shieldHealth = maxShieldHealth;
        if(isActive)
            UIManager.Instance.UpdateShield(shieldHealth);
        shieldPart.ResetShields(maxShieldHealth);
    }

    public void ActivateDoubleshot()
    {
        fireDoubleShot = true;
        if(doubleShotPowerupRoutine == null)
        {
            doubleShotPowerupRoutine = StartCoroutine(DoubleShotPowerdownTimer());
        }
        else
        {
            StopCoroutine(doubleShotPowerupRoutine);
            doubleShotPowerupRoutine = StartCoroutine(DoubleShotPowerdownTimer());
        }
    }

    public void ActivateSpeedBoost()
    {
        boostedMultiplier = speedBoostMultiplier;
        if (speedPowerupRoutine == null)
            speedPowerupRoutine = StartCoroutine(SpeedBoostCountdownRoutine());
        else
        {
            StopCoroutine(speedPowerupRoutine);
            speedPowerupRoutine = StartCoroutine(SpeedBoostCountdownRoutine());
        }
    }

    IEnumerator DoubleShotPowerdownTimer()
    {
        yield return new WaitForSeconds(5f);
        fireDoubleShot = false;
        doubleShotPowerupRoutine = null;
    }

    IEnumerator SpeedBoostCountdownRoutine()
    {
        yield return new WaitForSeconds(5f);
        boostedMultiplier = 1f;
        speedPowerupRoutine = null;
        
    }

    public void AddScore(int points)
    {
        _score += points;
        UIManager.Instance.UpdateScore(_score);
    }

    public void RestoreHealth()
    {
        if (_damageVisuals[0].activeInHierarchy == true)
            _damageVisuals[0].SetActive(false);
        else if (_damageVisuals[1].activeInHierarchy == true)
            _damageVisuals[1].SetActive(false);
    }
}

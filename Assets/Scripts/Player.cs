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
    [SerializeField]
    int _ammoCount = 15;
    [SerializeField]
    private AudioClip _emptySound;

    [Header("Health and Shield Settings")]
    [SerializeField]
    private int health = 3;
    [SerializeField]
    private int maxHealth = 3;
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
    [SerializeField]
    float _thrustMultiplier = 1;
    [SerializeField]
    float _boostedThrustMultiplier = 2f;
    [SerializeField]
    bool _isThrustActive;
    [SerializeField]
    float _engineHeat = 0f;
    [SerializeField]
    private float _heatingRate = 2f;
    [SerializeField]
    private float _coolingRate = 1.5f;
    [SerializeField]
    private float _maxEngineHeat = 100f;
    [SerializeField]
    private bool _isEngineOverheated = false;

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
        UIManager.Instance.UpdateThrusterVisual(_engineHeat);
        UIManager.Instance.UpdateAmmo(_ammoCount);
    }

    // Update is called once per frame
    void Update()
    {
        BoostCheck();

        DoMovement();

        BoundsCheck();

        if (Input.GetKey(KeyCode.Space) && _whenCanFire < Time.time)
        {
            FireLaser();
        }
    }

    private void BoostCheck()
    {
        if (_engineHeat <= 100)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !_isEngineOverheated)
            {
                _isThrustActive = true;
                _thrustMultiplier = _boostedThrustMultiplier;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift) || _isEngineOverheated)
            {
                _isThrustActive = false;
                _thrustMultiplier = 1f;
            }
        }
        else if(_engineHeat > 100)
        {
            _isThrustActive = false;
        }

        if (_isThrustActive && _engineHeat < _maxEngineHeat)
        {
            _engineHeat += _heatingRate * Time.deltaTime;
        }
        if (!_isThrustActive && _engineHeat > 0)
            _engineHeat -= _coolingRate * Time.deltaTime;

        if (_engineHeat > _maxEngineHeat)
        {
            _isEngineOverheated = true;
            _engineHeat = _maxEngineHeat;
        }
        if (_engineHeat < 0)
        {
            _isEngineOverheated = false;
            _engineHeat = 0;
        }

        UIManager.Instance.UpdateThrusterVisual(_engineHeat / _maxEngineHeat);

    }

    private void FireLaser()
    {
        if (_ammoCount > 0)
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
            _ammoCount--;
            UIManager.Instance.UpdateAmmo(_ammoCount);
        }
        else
        {

            AudioManager.Instance.PlaySoundAtPlayer(_emptySound);
        }
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
        transform.Translate(motion * (Time.deltaTime * boostedMultiplier * _thrustMultiplier * speed));
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

    internal void addAmmo(int contentsAmount)
    {
        _ammoCount += contentsAmount;
        UIManager.Instance.UpdateAmmo(_ammoCount);
    }

    internal void addHealth(int contentsAmount)
    {
        if (health + contentsAmount > maxHealth)
            health = maxHealth;
        else
            health += contentsAmount;
        RestoreHealth();
        UIManager.Instance.UpdateHealth(health);
    }
}

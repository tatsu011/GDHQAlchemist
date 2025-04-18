using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class Enemy_2 : MonoBehaviour
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

    [SerializeField]
    Vector2 _fixedPoint;
    private float _stopPoint;
    private bool _startCircle;
    private float _currentAngle;
    private float _radius;
    [SerializeField]
    float maxTimer = 0f;
    Coroutine _circlingRoutine;
    Vector2 _centerPoint;
    private float _angluarSpeed;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindAnyObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        DoMovement();
    }

    private void DoMovement()
    {
        if (_startCircle == false)
        {
            transform.Translate(Vector3.down * (speed * Time.deltaTime));
        }

        if (_startCircle == false && transform.position.y <= _stopPoint && _circlingRoutine == null)
        {
            _startCircle = true;
            _centerPoint = transform.position;
            _centerPoint.y -= _radius;
            _circlingRoutine = StartCoroutine(CircleTimerRoutine());
        }

        if (_startCircle)
        {
            _currentAngle += _angluarSpeed /*+ Time.deltaTime*/;
            Vector2 offset = new Vector2(Mathf.Sin(_currentAngle), Mathf.Cos(_currentAngle)) * _radius;
            transform.position = _centerPoint + offset;
        }
    }

    private IEnumerator CircleTimerRoutine()
    {
        float timer = UnityEngine.Random.Range(1, maxTimer);
        while (timer > 0)
        {


            yield return null;
        }
    }

    void BoundsRespawn()
    {

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

    public void OnPlayerDeath()
    {
        canRespawn = false;
    }
}

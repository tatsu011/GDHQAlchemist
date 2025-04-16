using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _flyTime = 5f;
    [SerializeField] float _time;
    [SerializeField] bool _isStarburst;
    [SerializeField] Transform _laserContainer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _time = Time.time + _flyTime;
        _laserContainer = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * (_speed * Time.deltaTime));

        if (Time.time > _time)
        {
            if(_isStarburst)
            {
                foreach(Transform child in GetComponentInChildren<Transform>())
                {
                    if (!child.gameObject.activeSelf)
                        child.gameObject.SetActive(true);
                    child.SetParent(_laserContainer);
                }
            }

            if (!transform.parent.CompareTag("container"))
                Destroy(transform.parent.gameObject);
            Destroy(gameObject);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(gameObject.CompareTag("enemyProjectile") && other.CompareTag("Player"))
        {
            other.GetComponent<Player>()?.Damage();
            Destroy(gameObject);
        }
    }
}

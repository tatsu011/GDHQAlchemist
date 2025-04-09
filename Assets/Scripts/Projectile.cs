using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float time;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        time = Time.time + 5f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * (speed * Time.deltaTime));

        if (Time.time > time)
        {
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

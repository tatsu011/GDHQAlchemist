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
            Destroy(gameObject);
        }
        
    }
}

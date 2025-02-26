using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    float speed = 5f;

    [SerializeField]
    float upperBounds = 6f;
    [SerializeField]
    float lowerBounds = -5f;
    [SerializeField]
    float rightBounds = 10f;
    [SerializeField]
    float leftBounds = -10f;

    Vector3 position;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DoMovement();

        position = transform.position;

        if (transform.position.y < lowerBounds)
            position.y = lowerBounds;
        if(transform.position.y > upperBounds)
            position.y = upperBounds;
        if(transform.position.x < leftBounds)
            position.x = leftBounds;
        if (transform.position.x > rightBounds)
            position.x = rightBounds;
        transform.position = position;
    }

    private void DoMovement()
    {
        Vector3 motion = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.Translate(motion * (Time.deltaTime * speed));
    }
}

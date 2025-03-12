using Unity.VisualScripting;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    float speed;

    [SerializeField]
    float bottomBounds;

    public enum PowerupTypes
    {
        None, DoubleShot, Tripleshot, Healthpack, Ammopack, SpeedBoost, Shield
    }

    [SerializeField]
    PowerupTypes powerupType;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * (speed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            switch (powerupType)
            {
                case PowerupTypes.None:
                    Debug.LogWarning("Powerup with no payload picked up.");
                    break;
                case PowerupTypes.DoubleShot:
                    player.ActivateDoubleshot();
                    break;
                case PowerupTypes.Tripleshot:
                    break;
                case PowerupTypes.Healthpack:
                    break;
                case PowerupTypes.Ammopack:
                    break;
                case PowerupTypes.SpeedBoost:
                    break;
                case PowerupTypes.Shield:
                    break;
                default:
                    break;
            }
            Destroy(gameObject);
        }
    }
}

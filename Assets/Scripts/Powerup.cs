using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Powerup : MonoBehaviour
{
    [SerializeField]
    float speed;

    [SerializeField]
    float bottomBounds;

    [SerializeField]
    int _contentsAmount;

    AudioSource _audioSource;



    [SerializeField]
    PowerupTypes powerupType;
    [SerializeField]
    private float _audioDuration;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
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
            //AudioSource.PlayClipAtPoint(_audioSource.clip, transform.position, _audioSource.volume);


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
                    player.addHealth(_contentsAmount);
                    break;
                case PowerupTypes.Ammopack:
                    player.addAmmo(_contentsAmount);
                    break;
                case PowerupTypes.SpeedBoost:
                    player.ActivateSpeedBoost();
                    break;
                case PowerupTypes.Shield:
                    player.ShieldActive(true);
                    break;
                case PowerupTypes.Gatling:
                    player.ActivateGatling();
                    break;
                default:
                    break;
            }
            OnCollect();
        }
    }
    private void OnCollect()
    {
        _audioSource.Play();
        
        speed = 0;
        Destroy(gameObject, _audioDuration);
    }

    public PowerupTypes GetPowerup()
    {
        return powerupType;
    }
}


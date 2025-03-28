using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Powerup : MonoBehaviour
{
    [SerializeField]
    float speed;

    [SerializeField]
    float bottomBounds;

    AudioSource _audioSource;

    public enum PowerupTypes
    {
        None, DoubleShot, Tripleshot, Healthpack, Ammopack, SpeedBoost, Shield
    }

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
                    break;
                case PowerupTypes.Ammopack:
                    break;
                case PowerupTypes.SpeedBoost:
                    player.ActivateSpeedBoost();
                    break;
                case PowerupTypes.Shield:
                    player.ShieldActive(true);
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
}


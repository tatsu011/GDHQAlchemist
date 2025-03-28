using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    [SerializeField]
    private AudioSource _clipAudioSource;
    [SerializeField]
    private AudioSource _musicAudioSource;

    public static AudioManager Instance
    {
        get { return _instance; }
        private set { _instance = value; }
    }

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void PlaySoundAtPlayer(AudioClip clip)
    {

    }
}

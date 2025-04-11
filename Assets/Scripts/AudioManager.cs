using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    [SerializeField]
    private AudioSource _clipAudioSource;
    [SerializeField]
    private AudioSource _musicAudioSource;

    bool _audioLock;

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
        _clipAudioSource.clip = clip;
        if (_clipAudioSource.isPlaying && _audioLock)
            return;
        _clipAudioSource.Play();
    }

    public void PlayAndLockClip(AudioClip clip)
    {
        _audioLock = true;
        _clipAudioSource.clip = clip;
        if (_clipAudioSource.isPlaying)
            return;
        _clipAudioSource.Play();
    }

    public void PlayMusicAtPlayer(AudioClip clip)
    {

    }
}

using UnityEngine;

public class ColtFiring : MonoBehaviour
{
    Animator _anim;
    [SerializeField] ParticleSystem _muzzleFlash1;
    [SerializeField] ParticleSystem _muzzleFlash2;
    [SerializeField] ParticleSystem _smoke;
    AudioSource _audio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            _anim.SetTrigger("Bang");
            _muzzleFlash1.Play();
            _muzzleFlash2.Play();
            _smoke.Play();
            _audio.Play();
        }
    }
}

using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    Vector3 _originalPosition;
    Vector3 _shakePosition = new Vector3();
    Coroutine _coRoutine;
    float _randx, _randy, _time;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CameraShake(float duration, float intensity)
    {
        if(_coRoutine == null)
            _coRoutine = StartCoroutine(shakeRoutine(duration, intensity));
    }

    IEnumerator shakeRoutine(float duration, float intensity)
    {
       
        _time = 0;
        while(_time < duration)
        {
            _shakePosition.x = Random.value * intensity;
            _shakePosition.y = Random.value * intensity;
            _shakePosition.z = _originalPosition.z;
            transform.position = _shakePosition;


            _time += Time.deltaTime;
            yield return null;
        }
        transform.position = _originalPosition;
    }

}

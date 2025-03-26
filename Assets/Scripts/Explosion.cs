using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private float _animtationTime;

    private void Start()
    {
        //Destroy(this.gameObject, _animtationTime);
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}

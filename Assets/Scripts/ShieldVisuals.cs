using UnityEngine;

public class ShieldVisuals : MonoBehaviour
{
    [SerializeField]
    int currentShields;

    [SerializeField]
    float spinSpeed = 5f;
    [SerializeField]
    Sprite fullShields;
    [SerializeField]
    Sprite halfShields;
    [SerializeField]
    Sprite lastHit;

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, spinSpeed * Time.deltaTime));
    }

    public void DamageShields()
    {
        currentShields--;
        if (currentShields == 2)
            GetComponent<SpriteRenderer>().sprite = halfShields;
        else
        if (currentShields == 1)
            GetComponent<SpriteRenderer>().sprite = lastHit;
    }

    public void ResetShields(int maxShields)
    {
        currentShields = maxShields;
        GetComponent<SpriteRenderer>().sprite = fullShields;
    }
}

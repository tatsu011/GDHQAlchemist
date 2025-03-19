using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image shieldMeter;
    [SerializeField] private Image lifeMeter;

    [SerializeField] private Sprite[] shieldSprites;
    [SerializeField] private Sprite[] lifeSprites;

    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
                Debug.LogError("UI Manager private instance is Null!");
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if(instance != this)
            Destroy(this);
    }

    public void UpdateShield(int shield)
    {
        if (shield > 0)
            shieldMeter.gameObject.SetActive(true);
        else
            shieldMeter.gameObject.SetActive(false);

        if (shield == 3)
        {
            shieldMeter.gameObject.SetActive(true);
            shieldMeter.sprite = shieldSprites[2];
        }
        if(shield == 2)
        {
            shieldMeter.sprite = shieldSprites[1];
        }
        if(shield == 1)
        {
            shieldMeter.sprite = shieldSprites[0];
        }
    }

    public void UpdateHealth(int health)
    {
        if (health >= 3)
        {
            //lifeMeter.gameObject.SetActive(true); //enable the gameobject.
            lifeMeter.sprite = lifeSprites[shieldSprites.Length - 1];
        }
        if (health == 2)
        {
            lifeMeter.sprite = lifeSprites[1];
        }
        if (health == 1)
        {
            lifeMeter.sprite = lifeSprites[0];
        }
    }

}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image shieldMeter;
    [SerializeField] private Image lifeMeter;

    [SerializeField] private Sprite[] shieldSprites;
    [SerializeField] private Sprite[] lifeSprites;

    [SerializeField] private TMP_Text scoreText;

    [SerializeField] private GameObject gameOverText;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private GameObject restartText;

    [SerializeField] private GameManager gameManager;

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

    public void Start()
    {
        gameOverText.SetActive(false);
        finalScoreText.gameObject.SetActive(false);
        restartText.SetActive(false);
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
            lifeMeter.sprite = lifeSprites[2];
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

    public void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    public void OnPlayerDeath()
    {
        gameManager.OnPlayerDeath();
        finalScoreText.text = $"Final {scoreText.text}";
        scoreText.gameObject.SetActive(false);
        StartCoroutine(GameOverSequence());
        StartCoroutine(DelayFinalScore());
    }

    IEnumerator GameOverSequence()
    {
        while (true)
        {
            yield return null;
            gameOverText.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            gameOverText.SetActive(false);
            yield return new WaitForSeconds(1.5f);
        }
    }

    IEnumerator DelayFinalScore()
    {
        yield return new WaitForSeconds(5f);
        finalScoreText.gameObject.SetActive(true);
        yield return new WaitForSeconds(.5f);
        restartText.SetActive(true);

    }


}

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isPlayerDead = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && _isPlayerDead)
        {
            //restart the scene.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }    
    }

    public void OnPlayerDeath()
    {
        _isPlayerDead = true;
    }
}

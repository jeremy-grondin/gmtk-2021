using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerHud = null;
    [SerializeField] GameObject pauseMenu = null;
    [SerializeField] GameObject deathMenu = null;

    public static bool isGamePause = false;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        deathMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isGamePause = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Resume()
    {
        isGamePause = false;
        Time.timeScale = 1.0f;
        pauseMenu.SetActive(false);
    }

    public void QuitApp()
    {
        Application.Quit();
    }

    public void PlayerDeath()
    {
        playerHud.SetActive(false);
        deathMenu.SetActive(true);
    }
}

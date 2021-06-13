using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] UnityEvent enemiesAllDead = null;

    [SerializeField] GameObject playerHud = null;
    [SerializeField] GameObject pauseMenu = null;
    [SerializeField] GameObject deathMenu = null;
    [SerializeField] GameObject victoryMenu = null;

    public static bool isGamePause = false;

    public int nbEnemies = 0;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        deathMenu.SetActive(false);
        victoryMenu.SetActive(false);

        nbEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;

        Debug.Log("Nb Enemies" + nbEnemies);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Resume();
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
        isGamePause = !isGamePause;
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        Time.timeScale = (Time.timeScale == 0) ? 1.0f : 0.0f;
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

    public void EnemyDeath()
    {
        nbEnemies--;
        if (nbEnemies <= 0)
        {
            enemiesAllDead.Invoke();
            victoryMenu.SetActive(true);
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] UnityEvent enemiesAllDead = null;

    [SerializeField] GameObject playerHud = null;
    [SerializeField] GameObject pauseMenu = null;
    [SerializeField] GameObject deathMenu = null;
    [SerializeField] GameObject victoryMenu = null;

    public static bool isGamePause = false;

    bool isGameWin = false;
    static public int nbEnemies = 0;

    [SerializeField] Text enemyText = null;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        victoryMenu.SetActive(false);
        deathMenu.SetActive(false);

        nbEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        enemyText.text = "Enemies remaining : " + nbEnemies.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Resume();

        if(nbEnemies == 0 && !isGameWin)
        {
            isGameWin = true;
            Time.timeScale = 0.0f;
            victoryMenu.SetActive(true);
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void Play()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Resume()
    {
        Time.timeScale = (Time.timeScale == 0) ? 1.0f : 0.0f;
        isGamePause = !isGamePause;
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

    public void QuitApp()
    {
        Application.Quit();
    }

    public void PlayerDeath()
    {
        Time.timeScale = 0.0f;
        playerHud.SetActive(false);
        deathMenu.SetActive(true);
    }

    public void EnemyDeath()
    {
        GameManager.nbEnemies--;

        Debug.Log("EnemyDeath");
        
        if (GameManager.nbEnemies <= 0)
        {
            GameManager.nbEnemies = 0;

            if (enemiesAllDead != null)
            {
                enemiesAllDead.Invoke();
            }
        }
        GameObject.FindGameObjectWithTag("enemyText").GetComponent<Text>().text = "Enemies remaining : " + nbEnemies.ToString();
    }
}

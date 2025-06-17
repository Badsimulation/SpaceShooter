using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    //Singleton instance
    public static UIManager Instance {  get; private set; }
    
    public TMP_Text scoreText;
    public TMP_Text livesText;

    public GameObject pauseMenu; //Assign the pause menu panel here
    private bool isPaused = false;


    //Awake is called when the script instance is being loaded
    private void Awake()
    {
        //check if an instance already exists
        if (Instance == null)
        {
            Instance = this; //Assign the instance
        } else
        {
            Destroy(gameObject); //Destroy duplicates
        }
    }

    void Start()
    {
        pauseMenu.SetActive(false); //makes sure the pause menu is hidden at start
    }


    /*-------------METHODS FOR TEXT FIELDS-----------------*/

    //method to update the score UI
    public void UpdateScoreUI(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        } 
        else
        {
            Debug.LogError("Score Text is not assigned in UIManager.");
        }
    }

    //method to update the lives UI from other scripts
    public void UpdateLivesUI(int currentLives)
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + currentLives;
        }
        else
        {
            Debug.LogError("Lives Text is not assigned in UIManager.");
        }
        
    }

    /*-------------METHODS FOR BUTTONS-----------------*/

    //method for pause button
    public void TogglePause()
    {
        isPaused = !isPaused;               //Toggle the paused state

        if (isPaused)
        {
            Time.timeScale = 0;             //freeze the game
            pauseMenu.SetActive(true);      //show the pause menu
        } 
        else
        {
            Time.timeScale = 1;             //Resume the game
            pauseMenu.SetActive(false);     //Hide the pause menu
        }
    }

    //method for restart button
    public void RestartGame()
    {
        Time.timeScale = 1; //make sure time is running normally
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //reload the current scene
    }

    //method for Quit button
    public void QuitGame()
    {
        SceneManager.LoadScene("StartMenu"); //loads the starting menu screen
    }

    //Method for start Game button
    public void StartGame()
    {
        SceneManager.LoadScene("EarthLevel1"); //Loads the first level
    }

}

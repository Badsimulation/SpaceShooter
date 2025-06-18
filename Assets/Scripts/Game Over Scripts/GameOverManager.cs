using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    
    //Method for the restart button
    public void RestartGame()
    {
        //load EarthLevel1
        GameState.isFrozen = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("EarthLevel1", LoadSceneMode.Single);
    }

    //Method for the Quit button
    public void QuitToMainMenu()
    {
        //Load the main menu
        SceneManager.LoadScene("StartMenu");
    }

}

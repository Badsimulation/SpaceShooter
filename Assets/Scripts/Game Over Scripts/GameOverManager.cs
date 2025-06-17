using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    
    //Method for the restart button
    public void RestartGame()
    {
        //load EarthLevel1
        SceneManager.LoadScene("EarthLevel1");
    }

    //Method for the Quit button
    public void QuitToMainMenu()
    {
        //Load the main menu
        SceneManager.LoadScene("StartMenu");
    }

}

using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLives : MonoBehaviour
{
    private int currentLives = 3;
    private Vector3 respawnPosition = new Vector3(0, -3.5f, 0); //player respawn position

    //Singleton instance for easy access by other scripts
    public static PlayerLives instance {  get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //initialize the UI with the starting number of lives
        UIManager.Instance.UpdateLivesUI(currentLives);
    }

    public void loseLife(int amount)
    {
        currentLives -= amount;

        //update the UI
        UIManager.Instance.UpdateLivesUI(currentLives);
    }

    public void gainLife(int amount)
    {
        currentLives += amount;

        //Update the UI
        UIManager.Instance.UpdateLivesUI(currentLives);
    }

    public int GetCurrentLives()
    {
        return currentLives;
    }


}

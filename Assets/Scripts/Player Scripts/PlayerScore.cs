using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    private int score = 0;

    //Singleton instance for easy access by other scripts
    public static PlayerScore instance {  get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Start method
    private void Start()
    {
        //initialize the UI with the starting score
        UIManager.Instance.UpdateScoreUI(score);
    }

    //method built in case I decide there should be anything that can decrease the player's score
    public void loseScore(int amount)
    {
        score -= amount;
        
        //score can't go below zero
        if (score < 0)
        {
            score = 0;
        }

        //Update the UI
        UIManager.Instance.UpdateScoreUI(score);
    }

    //method that increases the player's current score
    public void gainScore(int amount)
    {
        score += amount;

        //add any effects that happen at reaching milestones here

        //Update UI
        UIManager.Instance.UpdateScoreUI(score);
    }

    //method to retrieve the current score if needed for anything
    public int GetScore()
    {
        return score;
    }
}

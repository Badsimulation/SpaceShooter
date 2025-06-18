using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{

    //audio
    public AudioClip deathJingle;
    public AudioClip playerDestroyedSound;
    private AudioPlayer audioPlayer;

    //effects
    public GameObject explosionPrefab;
    public float flickerInterval = 0.1f; //interval for flicker effect
    
    //references for logic
    private PlayerController playerController;
    private PlayerShooting playerShooting;
    private Renderer playerRenderer;
    private Collider2D playerCollider;

    //logic variables
    public float destroyDelay = 1f;
    public float respawnDelay = 2f;
    public float invulnerabilityDuration = 3f;      //duration of invulnerability
    private Vector3 respawnPosition = new Vector3(0, -3.5f, 0);

    private void Start()
    {
        playerRenderer = GetComponent<Renderer>();
        playerCollider = GetComponent<Collider2D>();
        playerController = GetComponent<PlayerController>();
        playerShooting = GetComponent<PlayerShooting>();
    }


    //if player collides with enemy, destroys player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player collision detected.");
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyBullet"))
        {
            //TriggerExplosion
            TriggerExplosion();

            //handle player death
            gameObject.SetActive(false); //sets player inactive

            //lose a life
            PlayerLives.instance.loseLife(1); //decrease lives

            //destroy enemy bullet if collided with one
            if (collision.CompareTag("EnemyBullet"))
            {
                Destroy(collision.gameObject);
            }

            //freeze enemies during respawn phase
            FreezeEnemies(true);

            //respawn and game-over logic here
            if (PlayerLives.instance.GetCurrentLives() > 0)
            {
                Invoke("RespawnPlayer", respawnDelay); //respawn after a delay
            }
            else
            {
                //handle game over
                audioPlayer.PlaySound(deathJingle);
                SceneManager.LoadScene("GameOver");
            }
        }
    }

    void RespawnPlayer()
    {
        //Respawn player to the spawn point
        transform.position = respawnPosition;
        gameObject.SetActive (true);

        StartCoroutine(InvulnerabilityEffect() );
    }

    void TriggerExplosion()
    {
        //Instantiate explosion effect at the player's position
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        //Get the AudioPlayer component from the SoundEffects object
        GameObject soundEffectObject = GameObject.Find("SoundEffects");
        if (soundEffectObject != null)
        {
            audioPlayer = soundEffectObject.GetComponent<AudioPlayer>();
        }
        else
        {
            Debug.LogWarning("SoundEffects object not found in PlayerShooting.");
        }

        //Play sound effect for player destruction
        audioPlayer.PlaySound(playerDestroyedSound);

        //destroy explosion after a delay
        Destroy(explosion, destroyDelay);
    }

    IEnumerator InvulnerabilityEffect()
    {
        playerCollider.enabled = false; //disable player collider to avoid collisions during respawn

        //disable input
        if (playerController != null)
            playerController.enabled = false;
        if (playerShooting != null)
            playerShooting.enabled = false;

        float elapsedTime = 0f;

        //Flicker effect for invulnerability
        while (elapsedTime < invulnerabilityDuration)
        {
            playerRenderer.enabled = !playerRenderer.enabled; //toggle visibility
            yield return new WaitForSeconds (flickerInterval);

            elapsedTime += flickerInterval;
        }

        playerRenderer.enabled = true;      //ensure player is visible
        playerCollider.enabled = true;      //re-enable player collider

        //re-enable input
        if (playerController != null)
            playerController.enabled = true;
        if (playerShooting != null)
            playerShooting.enabled = true;

        //unfreeze enemies after invulnerability ends
        FreezeEnemies(false);
    }

    void FreezeEnemies(bool freeze)
    {
        GameState.isFrozen = freeze;
    }



    
}

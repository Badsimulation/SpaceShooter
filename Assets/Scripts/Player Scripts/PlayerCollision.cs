using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    public GameObject explosionPrefab;
    public float destroyDelay = 1f;
    public float respawnDelay = 2f;
    public AudioClip deathJingle;
    public AudioClip playerDestroyedSound;
    private AudioPlayer audioPlayer;
    private Renderer playerRenderer;
    private Collider2D playerCollider;
    private bool isInvulnerable = false;
    public float invulnerabilityDuration = 3f;      //duration of invulnerability
    public float flickerInterval = 0.1f;            //interval for flicker effect

    private Vector3 respawnPosition = new Vector3(0, -3.5f, 0);

    private void Start()
    {
        playerRenderer = GetComponent<Renderer>();
        playerCollider = GetComponent<Collider2D>();
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
        FreezeEnemies(true);                        //Freeze enemies while player is invulnerable
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
        isInvulnerable = true;
        playerCollider.enabled = false; //disable player collider to avoid collisions during respawn

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
        isInvulnerable = false;
        FreezeEnemies(false);               //Unfreeze enemies after invulnerability
    }

    void FreezeEnemies(bool freeze)
    {
        EnemyController[] enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
        foreach (EnemyController enemy in enemies)
        {
            enemy.SetFrozen(freeze);
        }
    }


    
}

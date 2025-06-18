using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    //logic
    private float zValueSetting = -1f;
    public float destroyDelay = 1f;
    private bool isVulnerable = false;

    //audio
    public AudioClip enemyDestroyedSound;
    private AudioPlayer audioPlayer;

    //particle effect
    public GameObject explosionEffect; //reference to the particle effect

    //references
    public EnemyWave wave;
    public GameObject extraLifePrefab;

    private void OnBecameVisible()
    {
        isVulnerable = true; //becomes vulnerable once visible
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isVulnerable) return;

        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject); // remove bullet
            Kill(escaped: false);          // handle all destruction + reward
        }
    }

    public void SetWave(EnemyWave wave)
    {
        this.wave = wave;
    }

    public void Kill(bool escaped)
    {
        // Instantiate explosion effect
        if (!escaped && explosionEffect != null)
        {
            GameObject explosion = Instantiate(
                explosionEffect,
                new Vector3(transform.position.x, transform.position.y, zValueSetting),
                Quaternion.identity
            );

            // 1/25 chance to drop extra life
            if (extraLifePrefab != null && Random.Range(0, 25) == 0)
            {
                Instantiate(extraLifePrefab, transform.position, Quaternion.identity);
            }

            Destroy(explosion, destroyDelay);
            Debug.Log("Explosion effect instantiated at " + transform.position);


        }

        // Load sound player if not already cached
        if (audioPlayer == null)
        {
            GameObject soundEffectObject = GameObject.Find("SoundEffects");
            if (soundEffectObject != null)
            {
                audioPlayer = soundEffectObject.GetComponent<AudioPlayer>();
            }
            else
            {
                Debug.LogWarning("SoundEffects object not found in EnemyCollision.");
            }
        }

        // Play sound based on cause of death
        if (audioPlayer != null && !escaped)
        {
            audioPlayer.PlaySound(enemyDestroyedSound);
        }

        // Wave bookkeeping
        wave?.UnregisterEnemy();

        // Score adjustment
        if (escaped)
            PlayerScore.instance.loseScore(5);
        else
            PlayerScore.instance.gainScore(1);

        // Destroy self
        Destroy(gameObject);
    }
}

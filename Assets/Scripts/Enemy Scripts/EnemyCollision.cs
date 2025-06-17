using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    private float zValueSetting = -1f;
    public float destroyDelay = 1f;
    public AudioClip enemyDestroyedSound;
    private AudioPlayer audioPlayer;

    public GameObject explosionEffect; //reference to the particle effect

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            //make explosion particle effect
            GameObject explosion = Instantiate(explosionEffect, new Vector3(transform.position.x, transform.position.y, zValueSetting), Quaternion.identity);
            Debug.Log("Explosion effect instantiated at " + transform.position);

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

            //Play sound effect for enemy destruction
            audioPlayer.PlaySound(enemyDestroyedSound);

            //Destroy the bullet
            Destroy(collision.gameObject);

            //Destroy the enemy
            Destroy(gameObject);

            //increase player score
            PlayerScore.instance.gainScore(1);

            //Destroy the effect
            Destroy(explosion, destroyDelay);

        } //end if
    }
}

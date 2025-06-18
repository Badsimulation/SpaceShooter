using UnityEngine;

public class KillZoneHandler : MonoBehaviour
{
    public int scorePenalty = 5;
    public AudioClip enemyEscapedSound;
    private AudioPlayer audioPlayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyCollision enemy = collision.GetComponent<EnemyCollision>();
            if (enemy != null)
            {
                enemy.Kill(escaped: true);
            }

            // Play escape sound (you can also move this into Kill() if you prefer centralizing it)
            if (audioPlayer == null)
            {
                GameObject soundEffectObject = GameObject.Find("SoundEffects");
                if (soundEffectObject != null)
                {
                    audioPlayer = soundEffectObject.GetComponent<AudioPlayer>();
                }
            }

            if (audioPlayer != null)
                audioPlayer.PlaySound(enemyEscapedSound);
        }
    }
}

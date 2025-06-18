using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.CompareTag("Bullet") && collision.CompareTag("EnemyBullet") ||
            gameObject.CompareTag("EnemyBullet") && collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject); // destroy the other bullet
            Destroy(gameObject);           // destroy self
        }
    }
}

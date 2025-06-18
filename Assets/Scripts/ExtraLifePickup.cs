using UnityEngine;

public class ExtraLifePickup : MonoBehaviour
{
    public float moveSpeed = 1.5f;

    private void Update()
    {
        if (!GameState.isFrozen)
        {
            transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerLives.instance.gainLife(1); // assumes a gainLife method exists
            Destroy(gameObject);
        }
    }
}

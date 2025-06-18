using UnityEngine;

public class BulletPause : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 storedVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (GameState.isFrozen)
        {
            if (rb.linearVelocity != Vector2.zero)
            {
                storedVelocity = rb.linearVelocity;
                rb.linearVelocity = Vector2.zero;
            }
        }
        else
        {
            if (rb.linearVelocity == Vector2.zero && storedVelocity != Vector2.zero)
            {
                rb.linearVelocity = storedVelocity;
            }
        }
    }
}

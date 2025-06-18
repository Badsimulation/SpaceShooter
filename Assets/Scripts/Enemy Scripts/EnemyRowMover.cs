using UnityEngine;

public class EnemyRowMover : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float approachSpeed = 0.3f;
    public float boundaryPadding = 0.5f;
    public bool movingRight = true;

    private float xmin, xmax;

    void Start()
    {
        float z = transform.position.z - Camera.main.transform.position.z;
        xmin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, z)).x + boundaryPadding;
        xmax = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, z)).x - boundaryPadding;
    }

    void Update()
    {
        if (GameState.isFrozen) return; //freeze movement
        
        float moveStep = moveSpeed * Time.deltaTime;
        float driftStep = approachSpeed * Time.deltaTime;

        // Move left/right
        transform.position += (movingRight ? Vector3.right : Vector3.left) * moveStep;

        // Constant slow descent
        transform.position += Vector3.down * driftStep;

        if (HasHitEdge())
        {
            movingRight = !movingRight;
        }
    }

    bool HasHitEdge()
    {
        foreach (Transform enemy in transform)
        {
            if (enemy == null) continue;
            float x = enemy.position.x;
            if (x < xmin || x > xmax)
                return true;
        }
        return false;
    }
}
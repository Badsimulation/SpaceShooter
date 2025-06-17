using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public float moveSpeed = 1f;
    public float approachSpeed = 1f;
    private bool isFrozen = false;

    private Vector3 direction;
    private Vector2 screenBounds;
    private Vector3 storedVelocity;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //get the screen boundaries to know when enemies are off screen
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        //start moving right and down
        direction = Vector3.right;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFrozen)
        {

            //Destroy the enemy if it moves off the bottom of the screen
            if (transform.position.y < -screenBounds.y - 1)
            {
                Destroy(gameObject);
            }

            //move the enemy in the current direction and towards the bottom of the screen
            transform.position += direction * moveSpeed * Time.deltaTime;
            transform.position += Vector3.down * approachSpeed * Time.deltaTime;

        }
    }

    //detecting bullet collision
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Boundary")) 
        {
            Debug.Log("Boundary collision dtected by: " + gameObject.name);
            //Reverse the direction
            direction = (direction == Vector3.right) ? Vector3.left : Vector3.right;

        }
    }

    public void SetFrozen(bool freeze)
    {
        isFrozen = freeze;
        if (isFrozen)
        {
            storedVelocity = GetComponent<Rigidbody2D>().linearVelocity;
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }
        else
        {
            GetComponent<Rigidbody2D>().linearVelocity = storedVelocity;
        }
    }

    
}

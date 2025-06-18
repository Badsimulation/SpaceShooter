using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    //public variables
    public GameObject enemyBulletPrefab;        //attach enemy bullet prefab here
    public Transform firePoint;                 //Point from where the bullet will be fired
    public float bulletSpeed = 5f;              //speed of the bullet
    public float minShootInterval = 2f;         //minimum time between shots
    public float maxShootInterval = 5f;         //maximum time between shots
    public float bulletLifeTime = 5f;           //time before bullet is destroyed

    //private variables
    private float nextFireTime;

    //variables for initial shooting delay until on screen
    private bool hasEnteredScreen = false;

    void Update()
    {
        if (GameState.isFrozen)
        {
            nextFireTime += Time.deltaTime;      //shift the fire timer forward
            return;
        }

        if (!hasEnteredScreen)
        {
            if (isVisibleToCamera())
            {
                hasEnteredScreen = true;
                SetNextFireTime();               //set the next time for the enemy to shoot
            }
            return; //skip shooting until has entered view
        }

        //regular shooting logic after entering screen
        if (Time.time >= nextFireTime)
        {
            Shoot();
            SetNextFireTime();
        }
    }


    //method to make the enemy shoot
    void Shoot()
    {
        //instantiate the bullet at the fire point
        GameObject bullet = Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);

        //make the bullet move downward
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.down * bulletSpeed;

        //destroy bullet after some time to avoid clutter
        Destroy(bullet, bulletLifeTime);
    }

    //method to set next time for the enemy to shoot
    void SetNextFireTime()
    {
        //set a random interval between shots
        nextFireTime = Time.time + Random.Range(minShootInterval, maxShootInterval);
    }

    //checks if the enemy is in the screen area
    bool isVisibleToCamera()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        return (viewportPos.x >= 0 && viewportPos.x <= 1 && 
                viewportPos.y >= 0 && viewportPos.y <= 1);
    }
}

using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject enemyBulletPrefab;        //attach enemy bullet prefab here
    public Transform firePoint;                 //Point from where the bullet will be fired
    public float bulletSpeed = 5f;              //speed of the bullet
    public float minShootInterval = 1f;         //minimum time between shots
    public float maxShootInterval = 5f;         //maximum time between shots
    public float bulletLifeTime = 5f;           //time before bullet is destroyed

    private float nextFireTime;

    void Start()
    {
        SetNextFireTime();
    }

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            SetNextFireTime();                  //set the next time for the enemy to shoot
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
}

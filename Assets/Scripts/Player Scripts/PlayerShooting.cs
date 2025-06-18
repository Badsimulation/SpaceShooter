using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    //logic
    public GameObject bulletPrefab;             //Assign bullet prefab here
    public Transform firePoint;                 //point from where the bullet is shot
    public float bulletSpeed = 10f;             //speed that the bullet moves
    public float fireRate = 0.5f;               //time between shots
    public float nextFireTime = 0f;
    private float destroyDelay = 1.5f;
    
    //audio
    public AudioClip shotSound;                 //Assign sound for player's weapon here
    private AudioPlayer audioPlayer;            //reference the the AudioSource for sound effects


    private void Start()
    {
        //Get the AudioPlayer component from the SoundEffects object
        GameObject soundEffectObject = GameObject.Find("SoundEffects");
        if (soundEffectObject != null )
        {
            audioPlayer = soundEffectObject.GetComponent<AudioPlayer>();
        } else
        {
            Debug.LogWarning("SoundEffects object not found in PlayerShooting.");
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate; //update the next fire time
        }
    }

    void Shoot()
    {
        //instantiate a new bullet at the fire points position
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        //add velocity to the bullet to make it move forward
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = firePoint.up * bulletSpeed;

        //play the shot sound effect through the AudioSource
        if (audioPlayer != null)
        {
            audioPlayer.PlaySound(shotSound); //play sound through AudioPlayer
        }

        //TODO: might need to check if bullet still exists
        //destroy the bullet after a cetain amount of time to avoid clutter
        Destroy(bullet, destroyDelay);
    }


}

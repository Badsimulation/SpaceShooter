using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    public GameObject enemyPrefab;                  //references the enemy prefab
    public int rows = 5;                            //number of rows
    public int cols = 10;                           //number of columns
    public float spacing = 1.5f;                    //spacing between enemies
    public float moveSpeed = 1f;                    //speed of horizontal movement
    public float approachingSpeed = 0.5f;           //speed of downward movement
    public float startDistance = 2f;

    private Vector2 screenBounds;                   //determine when enemies are off-screen
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //prevent enemies from colliding with each other
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"));

        //Get screen boundaries to determine off-screen starting positions
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        //Spawn the enemies above the screen
        SpawnEnemiesOffScreen();
    }


    //spawns enemies in a grid formation, starting off-screen
    void SpawnEnemiesOffScreen()
    {
        //calculate the total width of the enemy grid
        float gridWidth = (cols -1) * spacing;

        //calculate the starting y position above the screen
        float startY = screenBounds.y + startDistance;

        //calculate the x offset so that the grid is centerd on the screen
        float startX = -(gridWidth / 2);


        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                //position each enemy in a grid pattern, starting off-screen
                Vector3 position = new Vector3(startX + col * spacing, startY + row * spacing, 0);
                Instantiate(enemyPrefab, position, enemyPrefab.transform.rotation, transform);
            }
        }
    }

}

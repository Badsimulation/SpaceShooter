using System.Collections;
using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    private GameObject enemyPrefab;                  //references the enemy prefab
    private int rows = 5;                            //number of rows
    private int cols = 10;                           //number of columns
    private float spacing = 1.5f;                    //spacing between enemies
    private float moveSpeed = 1f;                    //speed of horizontal movement
    private float approachingSpeed = 0.5f;           //speed of downward movement
    private float startDistance = 2f;

    //delay variables for enemies
    private float delayBetweenEnemy = 0.05f;
    private float delayBetweenRows = 0.5f;

    //keeps track of the number of enemies
    private int enemyCount = 0;

    private Vector2 screenBounds;                   //determine when enemies are off-screen


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Initialize()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"));
        screenBounds = Camera.main.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    //level manager can call this method
    public void BeginWave()
    {
        StartCoroutine(SpawnEnemyRows());
    }

    IEnumerator SpawnEnemyRows()
    {
        float gridWidth = (cols - 1) * spacing;
        float startY = screenBounds.y + startDistance;
        float startX = -(gridWidth / 2);

        float currentY = startY;

        for (int row = 0; row < rows; row++)
        {
            //create a container gameobject for the row
            GameObject rowContainer = new GameObject("EnemyRow_" + row);
            rowContainer.transform.parent = transform;

            //add movement script to the row
            EnemyRowMover mover = rowContainer.AddComponent<EnemyRowMover>();
            mover.moveSpeed = moveSpeed;
            mover.approachSpeed = approachingSpeed;
            mover.movingRight = Random.value > 0.5f; //random start direction

            for (int col = 0; col < cols; col++)
            {
                Vector3 position = new Vector3(startX + col * spacing, currentY, 0);
                GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity, rowContainer.transform);
                enemy.GetComponent<EnemyCollision>()?.SetWave(this);

                yield return new WaitForSeconds(delayBetweenEnemy);
            }

            //randomize spacing for next row
            float rowSpacing = spacing + Random.Range(-0.3f, 0.8f);
            currentY += rowSpacing;

            yield return new WaitForSeconds(delayBetweenRows);
        }
    }

    public void ConfigureWave(GameObject enemyPrefab, int rows, int cols, float spacing, float moveSpeed, 
                              float approachingSpeed, float startDistance, int totalEnemies)
    {
        this.enemyPrefab = enemyPrefab;
        this.rows = rows;
        this.cols = cols;
        this.spacing = spacing;
        this.moveSpeed = moveSpeed;
        this.approachingSpeed = approachingSpeed;
        this.startDistance = startDistance;
        this.enemyCount = totalEnemies;
        Initialize();
    }

    public void UnregisterEnemy()
    {
        enemyCount--;
        if (enemyCount <= 0)
        {
            Debug.Log("Wave cleared");
            LevelManager.Instance.OnWaveCleared();
        }
    }

}

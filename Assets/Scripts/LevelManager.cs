using System.Collections;
using System.Data;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public int currentLevel = 1;
    public EnemyWave currentWave;
    public GameObject enemyPrefab;

    //variables for advancing level
    public TMP_Text levelBannerText;
    public float levelIntroDelay = 1f;
    public Transform playerTransform;

    //references to planet sprite for swapping
    public SpriteRenderer planetRenderer;
    public Sprite[] planetSprites; //1 per planet 

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        LoadLevel(currentLevel);
    }

    public void LoadLevel(int level)
    {
        currentLevel = level;

        // Remove previous wave if exists
        if (currentWave != null)
            Destroy(currentWave.gameObject);


        //set planet visuals
        int planetIndex = SetPlanetAppearance(currentLevel);

        //start boss fight if at end of game
        if (planetIndex == 8 && currentLevel >= (planetIndex + 1))
        {
            StartBossFight();
            return; //prevent spawning another wave
        }

        StartCoroutine(LevelIntroRoutine());
    }

    IEnumerator LevelIntroRoutine()
    {
        //show level banner
        levelBannerText.text = "Level " + currentLevel;
        levelBannerText.gameObject.SetActive(true);

        //reset player position
        playerTransform.position = new Vector3(0, -3.5f, 0);

        yield return new WaitForSeconds(levelIntroDelay);
        levelBannerText.gameObject.SetActive(false);

        //now spawn the wave
        GameObject waveGO = new GameObject("EnemyWave");
        currentWave = waveGO.AddComponent<EnemyWave>();

        int rows = Mathf.Min(10, 1 + currentLevel);
        int cols = Mathf.Min(6, 4 + currentLevel);
        int totalEnemies = rows * cols;


        currentWave.ConfigureWave(
            enemyPrefab: enemyPrefab,
            rows: rows,
            cols: cols,
            spacing: 2.5f,
            moveSpeed: 1.5f,
            approachingSpeed: 0.8f,
            startDistance: 1f,
            totalEnemies: totalEnemies
            );


        // Start the wave
        currentWave.BeginWave();

    }

    private int SetPlanetAppearance(int level)
    {
        int planetIndex = (level - 1);
        planetIndex = Mathf.Min(planetIndex, planetSprites.Length - 1);

        //set planet sprite
        SetPlanetSprite(planetSprites[planetIndex]);

        //reset tint to white by default
        planetRenderer.material.color = Color.white;

        //apply specific tints if needed
        switch (planetIndex)
        {
            case 2: //Mars
                planetRenderer.material.color = new Color(1f, 0.4f, 0.4f); //soft red tint
                break;

            case 3: //Jupitor
                planetRenderer.material.color = new Color(1f, 0.75f, 0.6f); // Light reddish-orange tone
                break;

            case 4: //Saturn - have to figure out how to add ring
                planetRenderer.material.color = new Color(1f, 1f, 0.8f); // Gentle yellow-gold tint
                break;

            case 5: //Uranus
                planetRenderer.material.color = new Color(0.4f, 0.4f, 1f); //slightly more blue
                break;

            case 6: //Neptune
                planetRenderer.material.color = new Color(0.2f, 0.3f, 1f); //more blue
                break;

            default:
                break; //no tint for Earth and unspecified planets
        }

        return planetIndex;
    }

    void StartBossFight()
    {
        // Replace with actual boss logic/spawn
        Debug.Log("Boss fight begins!");
        // Hide wave spawner if needed
        // Trigger cinematic/music/etc.
    }

    public void OnWaveCleared()
    {
        Debug.Log("Wave cleared!");
        currentLevel++;
        LoadLevel(currentLevel);
    }

    public void SetPlanetSprite(Sprite newSprite)
    {
        StartCoroutine(SetSpriteAndPreserveSize(newSprite));
    }

    private IEnumerator SetSpriteAndPreserveSize(Sprite newSprite)
    {
        // Store old size
        Vector2 oldSize = planetRenderer.bounds.size;

        // Assign new sprite
        planetRenderer.sprite = newSprite;

        // Wait one frame so bounds update
        yield return null;

        // Get new size after render update
        Vector2 newSize = planetRenderer.bounds.size;

        // Adjust scale to keep same size
        Vector3 currentScale = planetRenderer.transform.localScale;
        float scaleX = currentScale.x * (oldSize.x / newSize.x);
        float scaleY = currentScale.y * (oldSize.y / newSize.y);
        planetRenderer.transform.localScale = new Vector3(scaleX, scaleY, currentScale.z);
    }

}
   

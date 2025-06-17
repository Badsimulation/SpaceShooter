using System.Collections;
using UnityEngine;

public class EnemyShipMovement : MonoBehaviour
{
    public float speed = 1f;                //Speed of the enemy ship
    public float hoverDuration = 1f;        //Time the ship hovers
    public float moveTime = 2f;
    public Vector3 startingPosition;
    public float startOffset = 2f;          //offset for starting position

    //time intervals between ship swooping in after loop ends
    public float smallestInterval = 5f;
    public float largestInterval = 10f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set the starting position to be off screen (top right corner)
        startingPosition = new Vector3(Screen.width + startOffset * Screen.width, Screen.height + startOffset * Screen.height, 0);
        transform.position = Camera.main.ScreenToWorldPoint(startingPosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0); // Set z to 0 for 2D
        StartCoroutine(SwoopInAndOut());
    }

    private IEnumerator SwoopInAndOut()
    {
        while (true)
        {
            //swoop down towards the center
            Debug.Log("Running Coroutine SwooptoCenter");
            yield return StartCoroutine(SwoopToCenter());
            Debug.Log("SwoopToCenter Coroutine Completed");
            //hover for a moment
            Debug.Log("Waiting for HoverDuration");
            yield return new WaitForSeconds(hoverDuration);
            Debug.Log("HoverDuration Completed.");
            //move off towards the bottom left
            Debug.Log("Starting Coroutine MoveOffScreen");
            yield return StartCoroutine(MoveOffScreen());
            Debug.Log("MoveOffScreen Coroutine Completed");
            //wait for a random interval before next swoop
            Debug.Log("Waiting for RandomWaitTime");
            float randomWaitTime = Random.Range(smallestInterval, largestInterval);
            yield return new WaitForSeconds(randomWaitTime);
            Debug.Log("RandomWaitTime Completed");
            //reset position for next swoop
            Debug.Log("Resetting Ship to Starting position");
            ResetPosition();
        }
    }

    private IEnumerator SwoopToCenter()
    {
        Vector3 targetPosition = new Vector3(0, 0, 0); //center position
        float tripLength = Vector3.Distance(transform.position, targetPosition);
        float traveled = 0f;

        while (traveled < tripLength)
        {
            traveled += speed * Time.deltaTime;
            float t = Mathf.Clamp01(traveled / tripLength);
            transform.position = Vector3.Lerp(transform.position, targetPosition, t);
            yield return null; //wait for the next frame
        }
        yield return new WaitForSeconds(hoverDuration);
        transform.position = targetPosition; //ensure final position is exact
    }

    private IEnumerator MoveOffScreen()
    {
        // Convert screen space to world space for the off-screen position
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(-Screen.width, -Screen.height, 0));
        targetPosition.z = transform.position.z; // Keep the same Z position if you're working in 2D

        float tripLength = Vector3.Distance(transform.position, targetPosition);
        float traveled = 0f;

        while (traveled < tripLength)
        {
            traveled += speed * Time.deltaTime;
            float t = Mathf.Clamp01(traveled / tripLength);
            transform.position = Vector3.Lerp(transform.position, targetPosition, t);
            yield return null;
        }

        yield return new WaitForSeconds(hoverDuration);

        // Ensure the final position is set exactly to the target
        transform.position = targetPosition;
    }

    private void ResetPosition()
    {
        transform.position = Camera.main.ScreenToWorldPoint(startingPosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

}

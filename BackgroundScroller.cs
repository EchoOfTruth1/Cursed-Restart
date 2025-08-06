using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public Transform[] backgrounds;     // Assign in Inspector
    public float parallaxMultiplier = 0.5f;  // How fast the background moves relative to speed
    public StatManager statManager;     // Reference to the StatManager

    void Update()
    {
        float scrollSpeed = statManager.speed * parallaxMultiplier * Time.deltaTime;

        foreach (Transform bg in backgrounds)
        {
            bg.position += Vector3.left * scrollSpeed;

            // Optional: Loop background when it moves off-screen (simple logic)
            if (bg.position.x <= -20f)
            {
                bg.position += new Vector3(40f, 0, 0); // Move it back to the right
            }
        }
    }

    // Declare the gameover variable
    public bool gameover = false;

    // Example: Check for player null inside Update (assuming 'player' is defined elsewhere)
    // Uncomment and adjust as needed:

    // Make sure to declare 'player' at the class level
    public GameObject player;

    void LateUpdate()
    {
        if (player == null)
        {
            gameover = true;
        }
    }

    // Declare movespeed before using it
    public float movespeed = 0.5f;
    public float moveLeft = 0.5f * Time.deltaTime;
}

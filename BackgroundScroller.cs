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
}

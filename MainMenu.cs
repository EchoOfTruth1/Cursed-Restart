using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Load your main game scene here
        SceneManager.LoadScene("GameScene"); // Replace with your scene name
    }

    public void OpenStats()
    {
        // Load a stats screen or popup
        Debug.Log("Stats Screen Opened");
        // Optionally use: SceneManager.LoadScene("StatsScene");
    }

    public void QuitGame()
    {
        Debug.Log("Game Closed");
        Application.Quit();
    }
}

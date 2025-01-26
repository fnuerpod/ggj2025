using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScenes : MonoBehaviour
{
    private int finalTime;

    [SerializeField] private TextMeshProUGUI timeText;

    private void Start()
    {
        if (timeText != null)
        {
            float time = PlayerPrefs.GetFloat("Time"); // Get the time from PlayerPrefs
            finalTime = Mathf.RoundToInt(time); // Round the time to the nearest integer and save it as the final time
            timeText.text = $"Time Survived\n{finalTime} Seconds"; // Display the final time
        }
    }

    public void StartGame()
    {
        PlayerPrefs.DeleteKey("Time"); // Delete the time from PlayerPrefs
        SceneManager.LoadScene("Game"); // Load the game scene
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu"); // Load the main menu scene
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the game
    }
}

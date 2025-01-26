using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game_Over_Scene : MonoBehaviour
{
    FMOD.Studio.EventInstance BGM;
    FMOD.Studio.EventInstance ButtonClickSound;

    public Button RestartButton;
    public Button MenuClick;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Ensure cursor visibility.
        Cursor.visible = true;

        ButtonClickSound = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/PotGame_SFX_ItemPickup");

        if (SceneManager.GetActiveScene().name == "Game Over")
        {
            BGM = FMODUnity.RuntimeManager.CreateInstance("event:/MUSIC/PotGame_Music_GameTheme");
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("PotIntensity", 5f);
        } else
        {
            BGM = FMODUnity.RuntimeManager.CreateInstance("event:/MUSIC/PotGame_Music_MainMenu");
        }
       
        BGM.start();

        RestartButton.onClick.AddListener(RestartClick);

        MenuClick.onClick.AddListener(MainMenuClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RestartClick()
    {
        ButtonClickSound.start();
        BGM.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        SceneManager.LoadScene("Game");
    }

    void MainMenuClick()
    {
        ButtonClickSound.start();
        BGM.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        
        if (SceneManager.GetActiveScene().name == "Game Over")
        {
            SceneManager.LoadScene("Main Menu");
        }
        else
        {
            if (Application.isEditor)
            {
                Debug.LogWarning("Would exit in Built mode!");
            } else
            {
                Application.Quit();
            }
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game_Over_Scene : MonoBehaviour
{
    FMOD.Studio.EventInstance BGM;

    public Button RestartButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Ensure cursor visibility.
        Cursor.visible = true;

        BGM = FMODUnity.RuntimeManager.CreateInstance("event:/MUSIC/PotGame_Music_GameTheme");
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("PotIntensity", 5f);
        BGM.start();

        RestartButton.onClick.AddListener(RestartClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RestartClick()
    {
        BGM.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        SceneManager.LoadScene("Game");
    }
}

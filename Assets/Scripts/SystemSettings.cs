using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemSettings : MonoBehaviour
{
    public bool isFullscreen = true;
    public bool sfxOnClick = false;
    public float volume = 1.0f;
    public bool displayCPS = false;
    public bool visibleButton = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // volume??????

    // save the daa
    public void SaveSettings()
    {
        SaveSystem.SaveSettings(this);
    }

    public void LoadSettings()
    {
        SystemData data = SaveSystem.LoadSettings();
        Debug.Log("Loading system data...");

        isFullscreen = data.isFullscreen;
        sfxOnClick = data.sfxOnClick;
        displayCPS = data.displayCPS;
        visibleButton = data.visibleButton;
        //volume = data.volume;
    }
}

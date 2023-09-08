using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SystemData
{
    public bool isFullscreen;
    public bool sfxOnClick;
    public float volume;
    public bool displayCPS;
    public bool visibleButton;

    public SystemData(SystemSettings settings)
    {
        isFullscreen = settings.isFullscreen;
        sfxOnClick = settings.sfxOnClick;
        displayCPS = settings.displayCPS;
        visibleButton = settings.visibleButton;
        volume = settings.volume;
    }
}

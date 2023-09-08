using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


public enum MenuState
{
    START_SCREEN,
    MAIN_MENU,
    START_GAME,
    POPUP,
    SETTINGS
}

public class MenuUI : MonoBehaviour
{
    [SerializeField] private GameObject pressKeyText;
    [SerializeField] private Animator titleAnim;
    [SerializeField] private Animator borderAnim;
    [SerializeField] private List<Text> uiText;
    [SerializeField] private GameObject infoPopup;
    [SerializeField] private List<Text> popupText;
    [SerializeField] private Text popupInfoText;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadSlider;
    [SerializeField] private GameManager gm;
    [SerializeField] private Animator settingsBorderAnim;
    [SerializeField] private Animator creditsAnim;
    [SerializeField] private SystemSettings settings;
    [SerializeField] private Slider audioSlider;
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle sfxToggle;
    [SerializeField] private Toggle cpsToggle;
    [SerializeField] private Text mainMenuLabel;

    private MenuState state;

    [SerializeField] private int menuIndex;
    private int menuIndexLast;

    private Scene currentScene;
    private string sceneName;
    private bool exist;
    private bool settingsExist;

    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;

        // fix the first option not being selected by default (please)
        menuIndexLast = 1;
        // start music or something
        if (sceneName == "MainMenu")
        {
            exist = SaveSystem.DoesPlayerFileExist();
            if (exist)
            {
                gm.LoadData();
            }

            settingsExist = SaveSystem.DoesSettingsFileExist();
            if (settingsExist)
            {
                UpdateAllSettings();
            }
            else
            {
                SaveSystem.CreateSettingsFile(settings);
            }
            
            
            if (!FindObjectOfType<AudioManager>().IsPlaying("music"))
            {
                FindObjectOfType<AudioManager>().Play("music");
            } 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneName == "MainMenu")
        {
            if (state == MenuState.MAIN_MENU)
            {
                if (menuIndex > uiText.Count - 1)
                {
                    menuIndex = 0;
                }

                if (menuIndex < 0)
                {
                    menuIndex = uiText.Count - 1;
                }
            }
            
            // this stinks
            if (state == MenuState.START_GAME)
            {
                
                if (gm.ngPlusCount < 2 || !exist)
                {
                    if (menuIndex > 1)
                    {
                        menuIndex = 0;
                    }

                    if (menuIndex < 0)
                    {
                        menuIndex = 1;
                    }
                }
                else
                {
                    if (menuIndex > uiText.Count - 1)
                    {
                        menuIndex = 0;
                    }

                    if (menuIndex < 0)
                    {
                        menuIndex = uiText.Count - 1;
                    }
                }
            }

            if (state == MenuState.POPUP)
            {
                if (menuIndex > popupText.Count - 1)
                {
                    menuIndex = 0;
                }

                if (menuIndex < 0)
                {
                    menuIndex = popupText.Count - 1;
                }
            }

            ProcessInputs();
            UpdateUIText();
            ConfirmChoices();
            CancelChoices();
        }
        else if (sceneName == "MainScene")
        {
            UpdateUIText();
            ProcessInputs();
            ConfirmChoices();
            if (menuIndex > popupText.Count - 1)
            {
                menuIndex = 0;
            }

            if (menuIndex < 0)
            {
                menuIndex = popupText.Count - 1;
            }
        }
        
    }

    private void UpdateUIText()
    {
        if (sceneName == "MainMenu")
        {
            switch (state)
            {
                case MenuState.MAIN_MENU:
                    uiText[menuIndex].fontSize = 75;
                    uiText[menuIndexLast].fontSize = 58;
                    break;
                case MenuState.START_GAME:
                    uiText[menuIndex].fontSize = 75;
                    uiText[menuIndexLast].fontSize = 58;
                    break;
                case MenuState.POPUP:
                    // CHANGE THIS WHEN POPUP MADE
                    popupText[menuIndex].fontSize = 53;
                    popupText[menuIndexLast].fontSize = 43;
                    break;
            }
        }
        else if (sceneName == "MainScene")
        {
            popupText[menuIndex].fontSize = 53;
            popupText[menuIndexLast].fontSize = 43;
        }

        
    }

    private void ProcessInputs()
    {
        if (sceneName == "MainMenu")
        {
            // main menu & start play game
            if (Input.GetKeyDown(KeyCode.DownArrow) && state != MenuState.START_SCREEN && state != MenuState.POPUP && state != MenuState.SETTINGS)
            {
                FindObjectOfType<AudioManager>().Play("selection");
                menuIndexLast = menuIndex;
                menuIndex++;
                UpdateUIText();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) && state != MenuState.START_SCREEN && state != MenuState.POPUP && state != MenuState.SETTINGS)
            {
                FindObjectOfType<AudioManager>().Play("selection");
                menuIndexLast = menuIndex;
                menuIndex--;
                UpdateUIText();
            }



            // popup
            if (Input.GetKeyDown(KeyCode.LeftArrow) && state == MenuState.POPUP)
            {
                FindObjectOfType<AudioManager>().Play("selection");
                menuIndexLast = menuIndex;
                menuIndex--;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && state == MenuState.POPUP)
            {
                FindObjectOfType<AudioManager>().Play("selection");
                menuIndexLast = menuIndex;
                menuIndex++;
            }
        }
        else if (sceneName == "MainScene")
        {

            if (gm.paused)
            {
                if (infoPopup.activeInHierarchy)
                {
                    // popup
                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        menuIndexLast = menuIndex;
                        menuIndex--;
                    }
                    else if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        menuIndexLast = menuIndex;
                        menuIndex++;
                    }
                }

            }
        }
    }

    private void ConfirmChoices()
    {
        if (sceneName == "MainMenu")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                switch (state)
                {
                    case MenuState.START_GAME:
                        switch (menuIndex)
                        {
                            case 0:
                                // check if file exists
                                bool exist = SaveSystem.DoesPlayerFileExist();
                                if (!exist)
                                {
                                    FindObjectOfType<AudioManager>().Play("confirm");
                                    //SceneManager.LoadScene("MainScene");
                                    mainMenu.SetActive(false);
                                    loadingScreen.SetActive(true);
                                    StartCoroutine(LoadingLevel());
                                }
                                else
                                {
                                    // get da popup
                                    FindObjectOfType<AudioManager>().Play("confirm");
                                    state = MenuState.POPUP;
                                    SpawnPopUp(0);
                                }
                                break;
                            case 1:
                                // check if file exists
                                bool exists = SaveSystem.DoesPlayerFileExist();
                                if (exists)
                                {
                                    FindObjectOfType<AudioManager>().Play("confirm");
                                    //SceneManager.LoadScene("MainScene");
                                    gm.endlessActive = false;
                                    SaveSystem.SaveGame(gm, false);
                                    mainMenu.SetActive(false);
                                    loadingScreen.SetActive(true);
                                    StartCoroutine(LoadingLevel());
                                }
                                else
                                {
                                    // no data?
                                    FindObjectOfType<AudioManager>().Play("No");
                                }
                                break;
                            case 2:
                                // load endless mode
                                gm.endlessActive = true;
                                SaveSystem.SaveGame(gm, false);
                                FindObjectOfType<AudioManager>().Play("confirm");
                                mainMenu.SetActive(false);
                                loadingScreen.SetActive(true);
                                StartCoroutine(LoadingLevel());
                                break;
                        }
                        break;
                    case MenuState.MAIN_MENU:
                        FindObjectOfType<AudioManager>().Play("confirm");
                        switch (menuIndex)
                        {
                            case 0:
                                // switch to the other menu
                                state = MenuState.START_GAME;
                                uiText[0].text = "New Game";
                                uiText[1].text = "Continue Game";
                                mainMenuLabel.text = "Start\nGame";
                                if (gm.ngPlusCount > 1)
                                {
                                    uiText[2].text = "Endless Mode";
                                }
                                else
                                {
                                    uiText[2].text = "";
                                }
                                break;
                            case 1:
                                // settings
                                state = MenuState.SETTINGS;

                                // set settings animation
                                // set title animation to settings mode
                                // set credits animation to settings mode
                                settingsBorderAnim.SetBool("Active", true);
                                titleAnim.SetBool("Settings", true);
                                creditsAnim.SetBool("active", true);
                                borderAnim.SetBool("Active", false);
                                state = MenuState.SETTINGS;
                                break;
                            case 2:
                                // quit
                                Application.Quit();
                                break;
                        }
                        break;
                    case MenuState.POPUP:
                        switch (menuIndex)
                        {
                            // really running out of time here huh
                            case 0:
                                if (sceneName == "MainMenu")
                                {
                                    SaveSystem.DeletePlayerFile(false);
                                    //SceneManager.LoadScene("MainScene");
                                    mainMenu.SetActive(false);
                                    loadingScreen.SetActive(true);
                                    StartCoroutine(LoadingLevel());

                                }
                                else
                                {
                                    FindObjectOfType<AudioManager>().StopMusic("night_theme");
                                    SceneManager.LoadScene("MainMenu");

                                }
                                break;
                            case 1:
                                if (sceneName == "MainMenu")
                                {
                                    state = MenuState.START_GAME;
                                    infoPopup.SetActive(false);
                                }
                                else
                                {
                                    // unpause
                                    infoPopup.SetActive(false);
                                }
                                break;

                        }
                        break;
                }
            }

            // start screen 
            if (Input.anyKey && state == MenuState.START_SCREEN)
            {
                // confirm sound
                FindObjectOfType<AudioManager>().Play("confirm");
                titleAnim.SetBool("active", true);
                borderAnim.SetBool("Active", true);
                pressKeyText.SetActive(false);
                state = MenuState.MAIN_MENU;

            }
        }
        else if (sceneName == "MainScene")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (infoPopup.activeInHierarchy)
                {
                    switch (menuIndex)
                    {
                        case 0:
                            SceneManager.LoadScene("MainMenu");
                            break;
                        case 1:
                            Debug.Log("Dismiss popup");
                            infoPopup.SetActive(false);
                            //gm.popupAcitve = false;
                            break;
                    }
                }
            }
        }
    }

    private void CancelChoices()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            switch (state)
            {
                case MenuState.START_GAME:
                    // switch back
                    FindObjectOfType<AudioManager>().Play("cancel");
                    state = MenuState.MAIN_MENU;
                    uiText[0].text = "Start Game";
                    uiText[1].text = "Settings";
                    uiText[2].text = "Quit Game";
                    mainMenuLabel.text = "Main\nMenu";
                    break;
                case MenuState.SETTINGS:
                    FindObjectOfType<AudioManager>().Play("cancel");
                    settingsBorderAnim.SetBool("Active", false);
                    titleAnim.SetBool("Settings", false);
                    titleAnim.SetBool("Settings", false);
                    creditsAnim.SetBool("active", false);
                    borderAnim.SetBool("Active", true);
                    state = MenuState.MAIN_MENU;
                    settings.SaveSettings();
                    break;
            }
        }
    }

    private void SpawnPopUp(int s)
    {
        infoPopup.SetActive(true);
        switch (s)
        {
            case 0:
                popupInfoText.text = "Save data already exists and starting a new game will overwrite that, are you okay with this?";
                break;
            case 1:
                popupInfoText.text = "endlessmodedescription";
                break;
            case 2:
                popupInfoText.text = "Are you sure you want to quit? \nYou will lose any progress from the current day.";
                break;
        }
    }

    public void ResumeButton()
    {
        GameObject obj = GameObject.Find("GameManager");
        GameManager gm = obj.GetComponent<GameManager>();

        gm.paused = false;
        
    }

    public void QuitButton()
    {
        GameObject obj = GameObject.Find("GameManager");
        GameManager gm = obj.GetComponent<GameManager>();

        gm.popupAcitve = true;
        SpawnPopUp(2);
        
    }

    // loading from main menu to the game (takes a moment)

    IEnumerator LoadingLevel()
    {

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync("MainScene");

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadSlider.value = progressValue;
            yield return null;
        }
    }

    public void MouseOverText(int index)
    {
        switch (index)
        {
            case 1:
                menuIndex = 0;
                FindObjectOfType<AudioManager>().Play("selection");
                break;
            case 2:
                menuIndex = 1;
                FindObjectOfType<AudioManager>().Play("selection");
                break;
        }
    }

    // settings stuff

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        settings.isFullscreen = isFullscreen;
    }

    public void SetSoundToggle(bool sfxEnabled)
    {
        settings.sfxOnClick = sfxEnabled;
    }

    public void SetAudioLevel(float sliderValue)
    {
        mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
        settings.volume = Mathf.Log10(sliderValue) * 20;
    }

    // unused
    public void SetCPSToggle(bool cpsEnabled)
    {
        settings.displayCPS = cpsEnabled;
    }

    public void DisplayButtonToggle(bool buttonEnabled)
    {
        settings.visibleButton = buttonEnabled;
    }

    // call this on start when settings data has loaded
    private void UpdateAllSettings()
    {
        // just in case
        settings.LoadSettings();
        Screen.fullScreen = settings.isFullscreen;
        fullscreenToggle.isOn = settings.isFullscreen;
        sfxToggle.isOn = settings.sfxOnClick;
        audioSlider.value = settings.volume;
        cpsToggle.isOn = settings.displayCPS;
    }
}

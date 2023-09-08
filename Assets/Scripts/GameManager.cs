using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// TODO VERSION 1.3:
// Minigames???????? (idk Dee suggested it)
// golden cookie alternative
// fix bugs from 1.1 and 1.2





public class GameManager : MonoBehaviour
{
    // the main values
    /*
     * clickCount = the amount of clicks you have currently
     * clicksPerSecond = the amount of clicks that will be added to your total count each second
     * amountPerClick = the amount of clicks you get per clicking the button
     */
    public float clickCount = 0f;
    public float clicksPerSecond = 0f;
    public float amountPerClick = 1f;
    
    public int currentDay;
    public float currentTime;
    public float maxTimeInDay;
    public bool crunchTimeActive;
    public int previousDay;
    public int ngPlusCount = 1;

    // endless timer values
    public float eSeconds;
    public float eMinutes;
    public float eHours;

    // crunchy
    public int tiredLevel;
    public bool alreadyNight;
    private float tempCPS;
    private bool fuck;

    // LETS SEE HOW THIS GOES BEFORE CHANGING IT
    public int deadlineDaysLeft = 20;

    // stat recording variables
    public float totalClicksOfTheDay;
    public float endGoal;
    public float totalClicks;

    // upgrades should act as multipliers for the clicksPerSecond i think??????
    public List<int> upgradeAmount;


    // milestones for the deadline completion (define them in start based on the progressbar amount i think)
    public float[] milestones;

    // some object references
    [SerializeField] private Slider progressAmount;
    public List<Upgrade> upgrades;
    [SerializeField] private GameObject dayBG;
    [SerializeField] private GameObject nightBG;
    [SerializeField] private GameObject transition;

    // scene stuff
    private Scene currentScene;
    private string sceneName;

    public bool paused;
    public bool popupAcitve;
    [SerializeField] private GameObject pauseStuff;

    // endless
    public bool endlessActive;

    // settings
    [SerializeField] private SystemSettings systemSettings;

    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        if (sceneName == "MainScene")
        {            

            // LOAD SAVE DATA MOMENT
            bool exists = SaveSystem.DoesPlayerFileExist();
            if (exists)
            {
                LoadData();
                systemSettings.LoadSettings();

                if (endlessActive)
                {
                    bool endlessExist = SaveSystem.DoesEndlessFileExist();

                    if (endlessExist)
                    {
                        LoadEndlessData();
                    }
                    else
                    {
                        clickCount = 0;
                        clicksPerSecond = 0;
                        ResetUpgrades();
                        ResetPrices();
                        SaveSystem.CreatePlayerFile(this, true);
                        SaveData();
                    }
                    
                }

                if (ngPlusCount > 1)
                {
                    AdjustDeadLine();
                    ResetUpgrades();
                    ResetPrices();
                    SaveData();
                }

                // fix upgrade prices
                for (int i = 0; i < upgrades.Count; i++)
                {
                    FixPrices(i);
                }


            }
            else
            {
                ResetPrices();
                ResetUpgrades();
                SaveSystem.CreatePlayerFile(this, false);
                SaveData();
            }

            // don't make the tutorial appear constantly
            if (clickCount > 0)
            {
                GameObject tutorial = GameObject.Find("Tutorial");
                tutorial.SetActive(false);
            }

            // uhhhh test
            SetMilestones();

            // initiate crunch if loading from save
            if (alreadyNight)
            {
                StartCoroutine(SetupCrunchTime());
            }

            // start the clickspersecond doo doo
            InvokeRepeating("ApplyClickPerSecond", 1f, 1f);


        }
        else if (sceneName == "EndOfDay")
        {
            LoadData();
        }
        else if (sceneName == "FinishedDeadline")
        {
            LoadData();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (sceneName == "MainScene")
        {
            // prevent negative clicks
            if (clickCount < 0)
            {
                clickCount = 0;
            }

            // count time
            if (!endlessActive)
            {
                currentTime += Time.deltaTime;
            }
            
            if (endlessActive)
            {
                eSeconds += Time.deltaTime;

                if (eSeconds > 59)
                {
                    eSeconds = 0;
                    eMinutes += 1;
                }

                if (eMinutes > 59)
                {
                    eMinutes = 0;
                    eHours += 1;
                }
            }

            // detect if day is over
            if (currentTime > maxTimeInDay)
            {
                // check if you're behind
                if (!crunchTimeActive)
                {

                    // change scene to end of day
                    StartCoroutine(EndTheDay());
                }
                else
                {
                    // switch to night time
                    if (!alreadyNight)
                    {
                        if (!fuck)
                        {
                            StartCoroutine(SetupCrunchTime());
                        }

                    }
                    else
                    {
                        // change scene to end of day
                        StartCoroutine(EndTheDay());
                    }
                    
                }

            }

            // progress bar
            // SHOULD THIS BE THE CURRENT AMOUNT OF CLICKS OR TOTAL???????
            // ITS TOTAL DUMBASS
            progressAmount.value = totalClicks;

            // hack fix for a very annoying bug
            if (amountPerClick < 1)
            {
                amountPerClick = 1;
            }
        }

        // pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = true;
        }

        if (sceneName == "MainScene")
        {
            if (paused)
            {
                pauseStuff.SetActive(true);
            }
            else
            {
                pauseStuff.SetActive(false);
            }
        }
    }

    private void SetMilestones()
    {
        milestones = new float[5];
        milestones[0] = progressAmount.maxValue / 5;
        milestones[1] = milestones[0] * 2;
        milestones[2] = milestones[0] * 3;
        milestones[3] = milestones[0] * 4;
        milestones[4] = progressAmount.maxValue;
        endGoal = progressAmount.maxValue;
    }

    // add clicks to the click count each second
    private void ApplyClickPerSecond()
    {
        clickCount += clicksPerSecond;
        totalClicks += clicksPerSecond;
        totalClicksOfTheDay += clicksPerSecond;
        Debug.Log("Added " + clicksPerSecond + " clicks to click count");
    }

    // for upgrades
    public void UpdateCLickValues()
    {
        Debug.Log("UpdateClickValues() called!");
        // loop through each upgrade
        for (int i = 0; i < upgradeAmount.Count; i++)
        {
            // all this to fix a bug why is it stupid
            Upgrade currentUpgrade = upgrades[i];
            // i think this will work (clueless)
            Debug.Log("updating " + upgrades[i].upgradeName);
            clicksPerSecond += upgrades[i].cpsIncrease * upgradeAmount[i];
            Debug.Log("CURRENT UPGRADE = " + currentUpgrade.upgradeName);
            // this is my last attempt i have been forced to do this
            // IT WORKS HOLY SHIT
            Debug.Log("AMOUNT PER CLICK = " + upgrades[i].clickPowerIncrease);
            if (currentUpgrade.clickPowerIncrease > 0)
            {
                amountPerClick = upgrades[i].clickPowerIncrease * upgradeAmount[i];
            }

        }
        
    }

    public void LoadData()
    {
        GameData data = SaveSystem.LoadPlayer(false);
        Debug.Log("Loading game data...");

        clickCount = data.clickCount;
        clicksPerSecond = data.clicksPerSecond;
        amountPerClick = data.amountPerClick;
        currentDay = data.currentDay;
        crunchTimeActive = data.crunchTimeActive;
        upgradeAmount = data.upgradeAmount;
        deadlineDaysLeft = data.deadlineDaysLeft;
        totalClicks = data.totalProgress;
        tiredLevel = data.tiredLevel;
        alreadyNight = data.alreadyNight;
        ngPlusCount = data.ngPlusCount;
        endlessActive = data.endlessActive;
    }

    public void LoadEndlessData()
    {
        GameData data = SaveSystem.LoadPlayer(true);
        Debug.Log("Loading game data...");

        clickCount = data.clickCount;
        clicksPerSecond = data.clicksPerSecond;
        amountPerClick = data.amountPerClick;
        currentDay = data.currentDay;
        crunchTimeActive = data.crunchTimeActive;
        upgradeAmount = data.upgradeAmount;
        deadlineDaysLeft = data.deadlineDaysLeft;
        totalClicks = data.totalProgress;
        tiredLevel = data.tiredLevel;
        alreadyNight = data.alreadyNight;
        ngPlusCount = data.ngPlusCount;
        endlessActive = data.endlessActive;
    }

    private void SaveData()
    {
        if (!endlessActive)
        {
            SaveSystem.SaveGame(this, false);
        }
        else
        {
            SaveSystem.SaveGame(this, true);
        }
        
    }

    // fix upgrade prices after restart
    // now that they're prefabs do we even need this?
    private void FixPrices(int id)
    {

        Debug.Log("FixedPrices()");
        for (int i = 0; i < upgradeAmount[id]; i++)
        {
            upgrades[id].upgradePrice *= upgrades[id].priceIncrease;
        } 
       
    }

    // call this when its night time
    private IEnumerator SetupCrunchTime()
    {
        FindObjectOfType<AudioManager>().StopMusic("music");
        FindObjectOfType<AudioManager>().Play("night_theme");
        fuck = true;
        Debug.Log("Setting up crunch");
        transition.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        // change bg
        dayBG.SetActive(false);
        nightBG.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        // reset timer and halve the productivity of everything
        currentTime = 0;

        // save old stats
        tempCPS = clicksPerSecond;

        // change bg
        dayBG.SetActive(false);
        nightBG.SetActive(true);

        alreadyNight = true;

        // stat changes based on tiredness
        clicksPerSecond /= tiredLevel;

        for (int i = 0; i < upgrades.Count; i++)
        {
            if (upgrades[1].cpsIncrease > 0)
            {
                upgrades[i].cpsIncrease /= tiredLevel;
            }

            if (upgrades[i].clickPowerIncrease > 0)
            {
                upgrades[i].clickPowerIncrease /= tiredLevel;
            }
        }
    }

    // i can explain
    private void ResetPrices()
    {
        // when i made the upgrades prefabs it made them a bit fucky with saving their price increases
        upgrades[0].upgradePrice = 15;
        upgrades[1].upgradePrice = 100;
        upgrades[2].upgradePrice = 1000;
        upgrades[3].upgradePrice = 1500;
        upgrades[4].upgradePrice = 2000;
    }

    // call after crunch ends
    private void ResetUpgradeStats()
    {
        upgrades[0].cpsIncrease = 0.1f;
        upgrades[1].cpsIncrease = 0.25f;
        upgrades[2].cpsIncrease = 1.35f;
        upgrades[3].cpsIncrease = 2.5f;
        upgrades[4].cpsIncrease = 3;
    }

    private IEnumerator EndTheDay()
    {
        transition.SetActive(true);

        yield return new WaitForSeconds(0.49f);

        // lower deadline day count
        deadlineDaysLeft--;

        // fucky day shit
        previousDay = currentDay;
        currentDay++;
        alreadyNight = false;
        // save stats
        if (crunchTimeActive)
        {
            clicksPerSecond = tempCPS;
            ResetUpgradeStats();
        }
        
        SaveData();

        SceneManager.LoadScene("EndOfDay");
    }

    // with new game+, the deadline should be increased with each playthrough
    private void AdjustDeadLine()
    {
        Debug.Log("NG+ :DDDDDD");
        progressAmount.maxValue *= ngPlusCount;
    }

    // wipes all previously bought upgrades
    private void ResetUpgrades()
    {
        // the lazy way of doing it
        for (int i = 0; i > upgradeAmount.Count; i++)
        {
            upgradeAmount[i] = 0;
        }
    }
}

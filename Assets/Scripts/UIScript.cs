using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

// THIS IS FOR THE IN-GAME UI NOT ANY MENU LOOKIN SHITS
public class UIScript : MonoBehaviour
{
    [SerializeField] private Text clickText;
    [SerializeField] private Text dayText;
    [SerializeField] private Text cpsText;

    // the
    [SerializeField] private GameObject amountPerClickText;
    [SerializeField] private GameObject particle;

    // obj ref for the manager
    [SerializeField] private GameManager gameManager;

    // for the upgrade panel
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private Text upgradeDescriptionText;
    [SerializeField] private Text upgradeCostText;

    [SerializeField] private List<Text> upgradeButtonText;

    [SerializeField] private Text progressText;

    [SerializeField] private SystemSettings settings;

    [SerializeField] private Text endlessTimer;

    [SerializeField] private GameObject progressionStuff;

    // Start is called before the first frame update
    void Start()
    {
        dayText.text = "Day " + gameManager.currentDay;

        // make the button text say the upgrade names
        for (int i = 0; i < upgradeButtonText.Count; i++)
        {
            upgradeButtonText[i].text = gameManager.upgrades[i].upgradeName;
        }

        if (settings.displayCPS)
        {
            cpsText.gameObject.SetActive(true);
        }

        if (gameManager.endlessActive)
        {
            //endlessTimer.gameObject.SetActive(true);
            progressionStuff.SetActive(false);
            dayText.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // doesn't need to update every frame but oh well
        // GRAMMAR LMAO
        switch (gameManager.clicksPerSecond)
        {
            case 1:
                cpsText.text = gameManager.clicksPerSecond + " Click Per Second";
                break;
            default:
                cpsText.text = gameManager.clicksPerSecond + " Clicks Per Second";
                break;
        }

        if (gameManager.clickCount < 1000)
        {
            switch (gameManager.clickCount)
            {
                case 1:
                    clickText.text = (int)gameManager.clickCount + " Click";
                    break;
                default:
                    clickText.text = (int)gameManager.clickCount + " Clicks";
                    break;

            }
        }
        else if (gameManager.clickCount >= 1000 && gameManager.clickCount < 1000000)
        {
            clickText.text = Math.Round(gameManager.clickCount / 1000, 1) + "k clicks";
        }
        else if (gameManager.clickCount >= 1000000)
        {
            clickText.text = Math.Round(gameManager.clickCount / 1000000, 1) + "m clicks";
        }
        else if (gameManager.clickCount >= 1000000000)
        {
            clickText.text = Math.Round(gameManager.clickCount / 1000000000, 1) + "b clicks";
        }
        else if (gameManager.clickCount >= 1000000000000)
        {
            clickText.text = Math.Round(gameManager.clickCount / 1000000000000, 1) + " trillion clicks";
        }
        
        // ^^^^ EWWWW THIS IS GROSS MAKE IS BETTER LEWIS

        if (gameManager.alreadyNight)
        {
            dayText.text = "Night " + gameManager.currentDay;
        }

        // make the panel follow the mouse
        upgradePanel.transform.position = new Vector3(Input.mousePosition.x + 380, Input.mousePosition.y + 160, upgradePanel.transform.position.z);

        // make the text on the progress bar consistent
        progressText.text = Math.Round(gameManager.totalClicks) + "/" + gameManager.endGoal;

        if (gameManager.endlessActive)
        {
            endlessTimer.text = gameManager.eHours + ":" + gameManager.eMinutes + ":" + Mathf.RoundToInt(gameManager.eSeconds);
        }
    }

    public void ClickedOnButtonText()
    {
        GameObject temp = Instantiate(amountPerClickText, GameObject.Find("Canvas").transform);
        //temp.transform.parent = GameObject.Find("Canvas").transform;
        temp.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, temp.transform.position.z);

        GameObject tempParticle = Instantiate(particle);
        //temp.transform.parent = GameObject.Find("Canvas").transform;
        tempParticle.transform.position = new Vector3(GameObject.Find("ParticleLocation").transform.position.x, GameObject.Find("ParticleLocation").transform.position.y, tempParticle.transform.position.z);

    }

    // UPGRADE BUTTONS
    public void UpgradeButton(int id)
    {
        if (gameManager.clickCount >= gameManager.upgrades[id].upgradePrice)
        {
            Debug.Log("Adding to id: " + id);
            // add to correct upgrade counter
            gameManager.upgradeAmount[id]++;

            // increase price of upgrade
            gameManager.upgrades[id].upgradePrice *= gameManager.upgrades[id].priceIncrease;

            gameManager.clickCount -= gameManager.upgrades[id].upgradePrice;
            gameManager.UpdateCLickValues();
            // put this here to update the price text
            upgradeCostText.text = gameManager.upgrades[id].upgradePrice + " Clicks";
        }
        else
        {
            Debug.Log("upgrade too expensive!");
            // TODO: visual indication for this!
        }

    }

    // these two might need to be reworked if i use something beyond ui buttons

    // show the panel NOW
    public void ShowUpgradePanel(int id)
    {
        upgradePanel.SetActive(true);

        // make text match the upgrade you're highlighting
        upgradeDescriptionText.text = gameManager.upgrades[id].description;
        
        upgradeCostText.text = gameManager.upgrades[id].upgradePrice + " Clicks";
    }

    // ok hide it now
    public void HideUpgradePanel()
    {
        upgradePanel.SetActive(false);
    }
}

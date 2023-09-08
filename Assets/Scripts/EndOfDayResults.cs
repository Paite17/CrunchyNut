using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndOfDayResults : MonoBehaviour
{
    [SerializeField] private Text dayLabel;
    [SerializeField] private Text moneyEarnedLabel;
    [SerializeField] private Text progressMadeLabel;
    [SerializeField] private Text onScheduleLabel;
    [SerializeField] private Text deadlineLabel;

    // fucky wucky
    [SerializeField] private GameManager gameManager;

    private GameData data;
    

    // temp
    private float cooldownTimer;
    // Start is called before the first frame update
    void Start()
    {
        // why do i need to do this
        gameManager.LoadData();
        // get the data needed
        SetLabelValues();
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;

        // press any key to continue
        if (Input.anyKeyDown && cooldownTimer > 1.5f)
        {
            

            // music stuff
            if (FindObjectOfType<AudioManager>().IsPlaying("night_theme"))
            {
                FindObjectOfType<AudioManager>().StopMusic("night_theme");
                FindObjectOfType<AudioManager>().Play("music");
            }

            SceneManager.LoadScene("MainScene");
        }
    }

    private void SetLabelValues()
    {
        data = SaveSystem.LoadPlayer(false);

        // check if finished (temp)
        if (data.totalProgress >= data.endGoal)
        {
            SceneManager.LoadScene("FinishedDeadline");
        }
        
        // check if missed deadline
        if (data.deadlineDaysLeft < 1)
        {
            SceneManager.LoadScene("GameOverScene");
        }

        // need to make sure of something
        Debug.Log(data.totalProgress + " PROGRESS");
        Debug.Log(data.amountOfDaysProgress + " TODAYS PROGRESS");
        Debug.Log("ENDGOAL = " + data.endGoal);

        dayLabel.text = "End of Day: " + data.previousDay;
        moneyEarnedLabel.text = "$" + data.amountOfDaysProgress;
        // figure out percentage of total progress
        int percentage = (int)(Mathf.Round((float)data.totalProgress * 100f) / data.endGoal);
        Debug.Log("PERCENTAGE = " + percentage);
        progressMadeLabel.text = percentage + "%";
        
        // ok new plan
        // using the milestones array we're gonna see if you're behind on the invisible mini-deadlines
        // if you are then crunch should activate in which ending the day would start the night segment instead
        
        // yandere dev ass code right here
        if (data.currentDay < 4)
        {
            onScheduleLabel.color = new Color32(214, 153, 37, 255);
            onScheduleLabel.text = "N/A";
        }
        else if (data.currentDay > 4)
        {
            int miniPercentage = (int)(Mathf.Round((float)data.totalProgress * 100f) / data.milestones[0]);
            Debug.Log("miniPercentage 1 = " + miniPercentage);
            if (miniPercentage < 35)
            {
                onScheduleLabel.color = new Color32(188, 52, 52, 255);
                onScheduleLabel.text = "NO";
                data.crunchTimeActive = true;
                data.tiredLevel++;
            }
            else
            {
                onScheduleLabel.color = new Color32(87, 132, 81, 255);
                onScheduleLabel.text = "YES";
                data.crunchTimeActive = false;
            }
        }
        else if (data.currentDay > 8)
        {
            int miniPercentage = (int)(Mathf.Round((float)data.totalProgress * 100f) / data.milestones[1]);
            Debug.Log("miniPercentage 2 = " + miniPercentage);
            if (miniPercentage < 35)
            {
                onScheduleLabel.color = new Color32(188, 52, 52, 255);
                onScheduleLabel.text = "NO";
                data.crunchTimeActive = true;
                data.tiredLevel++;
            }
            else
            {
                onScheduleLabel.color = new Color32(87, 132, 81, 255);
                onScheduleLabel.text = "YES";
                data.crunchTimeActive = false;
            }
        }
        else if (data.currentDay < 12)
        {
            int miniPercentage = (int)(Mathf.Round((float)data.totalProgress * 100f) / data.milestones[2]);
            Debug.Log("miniPercentage 3 = " + miniPercentage);
            if (miniPercentage < 35)
            {
                onScheduleLabel.color = new Color32(188, 52, 52, 255);
                onScheduleLabel.text = "NO";
                data.crunchTimeActive = true;
                data.tiredLevel++;
            }
            else
            {
                onScheduleLabel.color = new Color32(87, 132, 81, 255);
                onScheduleLabel.text = "YES";
                data.crunchTimeActive = false;
            }
        }
        else if (data.currentDay > 16)
        {
            int miniPercentage = (int)(Mathf.Round((float)data.totalProgress * 100f) / data.milestones[3]);
            Debug.Log("miniPercentage 4 = " + miniPercentage);
            if (miniPercentage < 35)
            {
                onScheduleLabel.color = new Color32(188, 52, 52, 255);
                onScheduleLabel.text = "NO";
                data.crunchTimeActive = true;
                data.tiredLevel++;
            }
            else
            {
                onScheduleLabel.color = new Color32(87, 132, 81, 255);
                onScheduleLabel.text = "YES";
                data.crunchTimeActive = false;
            }
        }
        else if (data.currentDay < 18)
        {
            int miniPercentage = (int)(Mathf.Round((float)data.totalProgress * 100f) / data.milestones[4]);
            Debug.Log("miniPercentage 5 = " + miniPercentage);
            if (miniPercentage < 35)
            {
                onScheduleLabel.color = new Color32(188, 52, 52, 255);
                onScheduleLabel.text = "NO";
                data.crunchTimeActive = true;
                data.tiredLevel++;
            }
            else
            {
                onScheduleLabel.color = new Color32(87, 132, 81, 255);
                onScheduleLabel.text = "YES";
                data.crunchTimeActive = false;
            }
        }
       
        // this should be a cool scripted thing later
        // TODO: that ^
        if (data.deadlineDaysLeft < 10)
        {
            deadlineLabel.color = new Color32(188, 52, 52, 255);
        }
        else
        {
            //deadlineLabel.color = new Color32(87, 132, 81, 255);
        }

        deadlineLabel.text = data.deadlineDaysLeft.ToString();

        SaveData();
    }

    private void SaveData()
    {
        // funny
        gameManager.crunchTimeActive = data.crunchTimeActive;
        gameManager.tiredLevel = data.tiredLevel;
        SaveSystem.SaveGame(gameManager, false);
    }
}

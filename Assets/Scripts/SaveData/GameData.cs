using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // these gotta be saved
    public float clickCount;
    public float clicksPerSecond;
    public float amountPerClick;
    public int currentDay;
    public bool crunchTimeActive;
    public List<int> upgradeAmount;
    public float endGoal;
    public float amountOfDaysProgress;
    public float totalProgress;
    public int deadlineDaysLeft;
    public int previousDay;
    public float[] milestones;
    public int tiredLevel;
    public bool alreadyNight;
    public int ngPlusCount;
    public bool endlessActive;

    public GameData(GameManager GM)
    {
        clickCount = GM.clickCount;
        clicksPerSecond = GM.clicksPerSecond;
        amountPerClick = GM.amountPerClick;
        currentDay = GM.currentDay;
        crunchTimeActive = GM.crunchTimeActive;
        upgradeAmount = GM.upgradeAmount;
        endGoal = GM.endGoal;
        amountOfDaysProgress = GM.totalClicksOfTheDay;
        totalProgress = GM.totalClicks;
        deadlineDaysLeft = GM.deadlineDaysLeft;
        previousDay = GM.previousDay;
        milestones = GM.milestones;
        tiredLevel = GM.tiredLevel;
        alreadyNight = GM.alreadyNight;
        ngPlusCount = GM.ngPlusCount;
        endlessActive = GM.endlessActive;
    }
}

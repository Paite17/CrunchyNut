using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ClickButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // obj ref for the numbers!!!
    [SerializeField] private GameManager gameManager;

    // took all of this from the unity documentation thx guys :DDDDD

    //Detect current clicks on the GameObject (the one with the script attached)
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        //Output the name of the GameObject that is being clicked
        Debug.Log(name + "Game Object Click in Progress");

        // create reference for ui script obj
        GameObject ui = GameObject.Find("Canvas");
        UIScript theUI = ui.GetComponent<UIScript>();

        // create reference for settings
        GameObject setting = GameObject.Find("Settings");
        SystemSettings settings = setting.GetComponent<SystemSettings>();

        if (settings.sfxOnClick)
        {
            FindObjectOfType<AudioManager>().Play("click");
        }

        theUI.ClickedOnButtonText();
        gameManager.clickCount += gameManager.amountPerClick;
        gameManager.totalClicks += gameManager.amountPerClick;
        gameManager.totalClicksOfTheDay += gameManager.amountPerClick;
    }

    //Detect if clicks are no longer registering
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        Debug.Log(name + "No longer being clicked");
        // this could help for animation shjits
    }


}

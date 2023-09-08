using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTextChangeColour : MonoBehaviour
{
    [SerializeField] private List<Text> upgradeText;
    [SerializeField] private GameManager GM;

    // potentially not a good idea to use a for loop for this o____o
    // it worked fine the hell you on about
    void FixedUpdate()
    {
        // check prices
        for (int i = 0; i < GM.upgrades.Count; i++)
        {
            if (GM.upgrades[i].upgradePrice > GM.clickCount)
            {
                //upgradeText[i].color = Color.red;
                upgradeText[i].color = new Color32(200, 68, 63, 255);
            }
            else
            {
                //upgradeText[i].color= Color.green;
                upgradeText[i].color = new Color32(88, 140, 126, 255);
            }
        }
    }
}

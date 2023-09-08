using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanelTransparency : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "UIButton")
        {
            gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 130);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "UIButton")
        {
            gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
    }
}

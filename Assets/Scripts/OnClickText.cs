using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnClickText : MonoBehaviour
{
    private int speed = 75;
    private float existTime;
    // Start is called before the first frame update
    void Start()
    {
        GameObject temp1 = GameObject.Find("GameManager");
        GameManager gameManager = temp1.GetComponent<GameManager>();

        // make text points per click
        GetComponent<Text>().text = "+" + gameManager.amountPerClick;

    }

    // Update is called once per frame
    void Update()
    {
        // go upwards

        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // die after a short period
        existTime += Time.deltaTime;

        if (existTime > 0.15f)
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeadlineFinish : MonoBehaviour
{
    [SerializeField] private Text congratsText;
    [SerializeField] private Text firstSentence;
    [SerializeField] private Text finalBit;
    [SerializeField] private GameManager gm;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadSlider;
    [SerializeField] private GameObject clickText;
    private bool done;

    // Start is called before the first frame update
    void Start()
    {
        gm.LoadData();        
        FindObjectOfType<AudioManager>().StopMusic("music");
        StartCoroutine(ShowText());
        SaveNGPlus();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && done)
        {
            FindObjectOfType<AudioManager>().Play("music");
            //SceneManager.LoadScene("MainScene");
            loadingScreen.SetActive(true);
            StartCoroutine(LoadingLevel());
        }
    }

    private IEnumerator ShowText()
    {
        FindObjectOfType<AudioManager>().Play("first");
        congratsText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.0f);

        FindObjectOfType<AudioManager>().Play("second");
        firstSentence.gameObject.SetActive(true);

        yield return new WaitForSeconds(2.0f);

        FindObjectOfType<AudioManager>().Play("third");
        finalBit.gameObject.SetActive(true);
        clickText.SetActive(true);
        done = true;
        
    }

    // save ng+ to file
    private void SaveNGPlus()
    {
        Debug.Log("Saving ng+ moment");
        gm.ngPlusCount++;
        gm.currentDay = 1;
        gm.deadlineDaysLeft = 20;
        gm.totalClicks = 0;
        SaveSystem.SaveGame(gm, false);
    }

    
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
}

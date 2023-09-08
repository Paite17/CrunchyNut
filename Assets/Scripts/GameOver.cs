using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private float cooldown;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().StopMusic("music");
        if (!FindObjectOfType<AudioManager>().IsPlaying("night_theme"))
        {
            FindObjectOfType<AudioManager>().Play("night_theme");
        }
        SaveSystem.DeletePlayerFile(false);
    }

    // Update is called once per frame
    void Update()
    {
        cooldown += Time.deltaTime;

        if (cooldown > 2 && Input.anyKeyDown)
        {
            FindObjectOfType<AudioManager>().StopMusic("night_theme");
            SceneManager.LoadScene("MainMenu");
        }

    }
}

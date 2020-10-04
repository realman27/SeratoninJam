using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject levelSelectMenu;
    public GameObject settingsMenu;

    public TextMeshProUGUI startText;

    public int progress;
    public int unlocked;

    public RectTransform spiral;

    public AudioClip menuMusic;

    private void Start()
    {
        progress = PlayerPrefs.GetInt("Progress", 0);

        unlocked = PlayerPrefs.GetInt("Unlocked", 1);

        AudioManager.instance.PlayMusic(menuMusic, 0.5f, transform);
    }

    private void Update()
    {
        spiral.rotation = spiral.rotation * Quaternion.Euler(0f, 0f, Time.deltaTime * 100f);
    }

    public void StartContinue()
    {
        SceneManager.LoadScene("Beginning Cutscene");
    }

    public void OpenMenu(GameObject menu)
    {
        menu.SetActive(true);

        mainMenu.SetActive(false);
    }

    public void Back()
    {
        levelSelectMenu.SetActive(false);
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ResetData()
    {
        progress = 0;
        unlocked = 1;
        PlayerPrefs.SetInt("Progress", 0);
        PlayerPrefs.SetInt("Unlocked", 1);
    }
}

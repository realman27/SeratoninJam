using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance { get; private set; }

    public int currentLevel = 0;

    public List<GameObject> Levels = new List<GameObject>();

    public Transform ground;

    public bool DebugMode;

    public AudioClip BGMusic;

    public RectTransform LevelDisplay;
    public GameObject deadMenu;

    public GameObject playerPrefab;
    public CircularCamera cam;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadLevel(0);

        AudioManager.instance.PlayMusic(BGMusic, 0.2f, transform);
    }

    public void LoadLevel(int id)
    {
        currentLevel = id;

        if (id + 1 <= Levels.Count)
        {
            Levels[id].SetActive(true);
        }
        else
        {
            Debug.Log("You win!");
        }

        if (currentLevel > 1)
        {
            PlayerPrefs.SetInt("Progress", currentLevel);
        }

        if (currentLevel > PlayerPrefs.GetInt("Unlocked", 1)) {
            PlayerPrefs.SetInt("Unlocked", currentLevel);
        }

        if (currentLevel != 0)
        {
            LevelDisplay.GetComponent<TextMeshProUGUI>().text = "Level " + currentLevel;
            LevelDisplay.GetComponent<Animator>().SetTrigger("Display");
        }
    }

    public void UnloadLevel(int id)
    {
        Levels[id].SetActive(false);
    }

    public void Respawn()
    {
        GameObject player = Instantiate(playerPrefab);
        player.transform.position = new Vector3(-3.7f, 12.4f, 0f);
        cam.player = player.transform;
        deadMenu.SetActive(false);

        UnloadLevel(currentLevel);
        LoadLevel(currentLevel - 1);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        SceneManager.UnloadSceneAsync("Game");
        Destroy(instance.gameObject);
    }
}

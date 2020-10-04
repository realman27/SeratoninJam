using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneTwo : MonoBehaviour
{

    public AudioClip line;

    void Start()
    {
        AudioManager.instance.Play(line, transform);

        StartCoroutine(End());
    }

    IEnumerator End()
    {
        yield return new WaitForSeconds(7f);

        SceneManager.LoadScene("MainMenu");
    }
}

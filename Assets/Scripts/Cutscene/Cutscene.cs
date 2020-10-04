using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour
{
    public SpriteRenderer Background1;
    public SpriteRenderer Background2;

    public SpriteRenderer Character1;
    public SpriteRenderer Character2;

    public AudioClip VoiceLine1;
    public AudioClip VoiceLine2;

    private void Start()
    {
        StartCoroutine(StartScene());
    }

    IEnumerator StartScene()
    {
        AudioManager.instance.Play(VoiceLine1, transform);
        yield return new WaitForSeconds(VoiceLine1.length);

        Character1.GetComponent<Animator>().SetTrigger("Zoom");

        yield return new WaitForSeconds(5);

        Background2.enabled = true;
        Character2.enabled = true;
        Background1.enabled = false;
        Character1.enabled = false;

        Character2.GetComponent<Animator>().Play("falling cutscene");

        AudioManager.instance.Play(VoiceLine2, transform);
        yield return new WaitForSeconds(VoiceLine2.length);

        SceneManager.LoadScene("Game");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgression : MonoBehaviour
{
    public string entered;
    public string exited;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger == true)
            return;
        if (collision.transform.GetComponent<PlayerEntity>() == false)
            return;

        float dir = transform.position.x - collision.transform.position.x;

        if (dir > 0f)
        {
            entered = "left";
        }
        else if (dir < 0f)
        {
            entered = "right";
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.isTrigger == true)
            return;
        if (collision.transform.GetComponent<PlayerEntity>() == false)
            return;

        float dir = transform.position.x - collision.transform.position.x;

        if (dir < 0f)
        {
            exited = "right";
        }
        else if (dir > 0f)
        {
            exited = "left";
        }

        bool exit = entered.Equals(exited);

        if (exit)
            return;

        if (dir < 0) // Right
        {
            int nextLevel = LevelManager.instance.currentLevel + 1;
            LevelManager.instance.LoadLevel(nextLevel);

            if (nextLevel > 0)
            {
                LevelManager.instance.UnloadLevel(nextLevel - 1);
            }
        }
        else if (dir > 0) // Left
        {
            int previousLevel = LevelManager.instance.currentLevel - 1;
            LevelManager.instance.UnloadLevel(previousLevel + 1);
            LevelManager.instance.LoadLevel(previousLevel);
        }
    }
}

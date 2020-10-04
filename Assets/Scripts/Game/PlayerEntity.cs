using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : Entity
{
    public static PlayerEntity instance { get; private set; }

    public Animator animator;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void Death()
    {
        base.Death();

        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerAbilities>().enabled = false;
        animator.SetTrigger("Death");

        Destroy(transform.gameObject, 0.5f);

        LevelManager.instance.deadMenu.SetActive(true);
    }
}

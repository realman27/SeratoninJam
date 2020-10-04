using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public float Health;
    public float maxHealth;

    public AudioClip hurt;
    public AudioClip dead;

    public HealthBar healthbar;

    public virtual void Start()
    {
        Health = maxHealth;
    }

    public virtual void Update()
    {
        healthbar.UpdateBar(Health, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        AudioManager.instance.Play(hurt, transform);

        Health -= damage;

        if (Health <= 0)
            Death();
    }

    public virtual void Death()
    {
        AudioManager.instance.Play(dead, transform);

        Debug.Log(transform.name + " died.");
    }
}

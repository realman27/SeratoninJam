using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slam : MonoBehaviour
{
    bool slammed = false;

    public int slamDamage = 30;

    public AudioClip slamSound;

    private void Start()
    {
        GetComponent<ParticleSystem>().Play();
        AudioManager.instance.Play(slamSound, transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (slammed)
            return;

        if (collision.transform.GetComponent<PlayerEntity>() == false)
            return;

        slammed = true;

        collision.transform.GetComponent<PlayerEntity>().TakeDamage(slamDamage);
    }
}

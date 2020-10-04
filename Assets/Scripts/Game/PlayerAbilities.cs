using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public bool serotoninShot = false;
    public float serotoninCooldown = 1f;
    public ParticleSystem serotoninLaser;
    public ParticleSystem serotoninLaserCrouch;
    public int serotoninDamage;

    public LayerMask enemyLayer;

    public AudioClip LaserSound;

    private void Update()
    {
        SerotoninInput();
    }

    void SerotoninInput()
    {
        if (Input.GetKeyDown("i") || Input.GetKeyDown("x"))
        {
            if (serotoninShot == true)
                return;

            StartCoroutine(SerotoninBeam());
        }
    }

    IEnumerator SerotoninBeam()
    {
        serotoninShot = true;

        AudioManager.instance.Play(LaserSound, transform);

        ParticleSystem point;
        if (GetComponent<PlayerMovement>().crouching == false)
        {
            point = serotoninLaser;
        }
        else
        {
            point = serotoninLaserCrouch;
        }
        float direction = GetComponent<PlayerMovement>().direction;
        if (direction > 0)
        {
            point.transform.localRotation = Quaternion.Euler(0, 0, 0); //-7
        } else
        {
            point.transform.localRotation = Quaternion.Euler(0, 0, 180); //187
        }

        point.Play();

        RaycastHit2D hit = Physics2D.Raycast(point.transform.position, transform.up * direction, 100, enemyLayer);
        if (hit.collider != null)
        {

            if (hit.collider.transform.GetComponent<Entity>() == true)
            {

                Entity enemy = hit.collider.transform.GetComponent<Entity>();
                enemy.TakeDamage(serotoninDamage);
            }
        }

        float cooldown = serotoninCooldown;
        while (cooldown > 0f)
        {
            cooldown -= Time.deltaTime;

            yield return null;
        }

        serotoninShot = false;
    }
}

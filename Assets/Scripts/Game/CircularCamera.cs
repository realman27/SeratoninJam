using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularCamera : MonoBehaviour
{
    public Transform player;

    public Transform cam;

    public float smooth = 0.3f;

    private void FixedUpdate()
    {
        Rotation();
        Movement();
    }

    void Movement()
    {
        if (player == null)
            return;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, player.position, smooth);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, -10);
    }

    void Rotation()
    {
        if (player == null)
            return;

        cam.rotation = player.rotation * Quaternion.Euler(0, 0, 90);
    }
}

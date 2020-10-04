using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularGravity : MonoBehaviour
{
    Rigidbody2D rb;

    private Transform ground;

    public float gravity;
    public float maxGravityDist;

    Vector3 forceDirection;

    Vector3 lookDirection;
    float lookAngle;

    private void Start()
    {
        ground = LevelManager.instance.ground;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(ground.transform.position, transform.position);
        Vector3 v = ground.transform.position - transform.position;
        rb.AddForce(v.normalized * (1f - distance / maxGravityDist) * gravity, ForceMode2D.Impulse);

        lookDirection = v;
        lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);
    }
}

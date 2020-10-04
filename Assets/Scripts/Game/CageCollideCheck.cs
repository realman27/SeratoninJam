using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageCollideCheck : CollideCheck
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.transform.GetComponent<PlayerEntity>())
        {
            colliding = true;
        }
    }

    public override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        if (collision.transform.GetComponent<PlayerEntity>())
        {
            colliding = false;
        }
    }
}

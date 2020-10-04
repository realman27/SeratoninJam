using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideCheck : MonoBehaviour
{
    public bool colliding = false;

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger == true)
            return;

        colliding = true;
    }

    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.isTrigger == true)
            return;

        colliding = false;
    }
}

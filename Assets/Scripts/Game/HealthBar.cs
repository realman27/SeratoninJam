using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Transform Bar;
    public SpriteRenderer background;
    public SpriteRenderer sprite;

    public Color barColor;

    private void Start()
    {
        if (sprite != null)
            sprite.color = barColor;   
    }

    public virtual void UpdateBar(float health, float maxHealth)
    {
        if ((health / maxHealth) == 1)
        {
            background.enabled = false;
            sprite.enabled = false;
        }
        else
        {
            background.enabled = true;
            sprite.enabled = true;
        }

        Bar.localScale = new Vector3(1, health / maxHealth, 1);
    }
}

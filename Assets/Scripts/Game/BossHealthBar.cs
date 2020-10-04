using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : HealthBar
{
    public Image barUI;

    public override void UpdateBar(float health, float maxHealth)
    {
        barUI.fillAmount = health / maxHealth;
    }
}

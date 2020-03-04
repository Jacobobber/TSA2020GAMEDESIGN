using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DoPlayerDamage : MonoBehaviour
{
    public Slider healthFill;

    public float currentHealth;
    public float maxHealth = 100;


    void Update()
    {
        if(Input.GetKeyDown("j"))
        {
            ChangeHealth(-10);
        }

        if(currentHealth <= 0)
        {
            SendMessage("PlayerDied");
        }
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthFill.value = currentHealth / maxHealth;
    }
}

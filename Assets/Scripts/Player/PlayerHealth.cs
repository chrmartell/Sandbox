using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //Fields
    [SerializeField]
    private int currentHealth;
    [SerializeField]
    private int maxHealth = 100;
    //[SerializeField]
    //private int numOfHealthBars = 5;

    [SerializeField] HealthBar healthbar;

    bool playerCanTakeDamage = true;

    private void Start()
    {
        // Initialize currentHealth to maxHealth at the start
        currentHealth = maxHealth;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha8))
        {
            DamagePlayer(20);
            Debug.Log("Damage Taken; HP: " + currentHealth);
        }
        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            HealPlayer(10);
            Debug.Log("Damage Healed; HP: " + currentHealth);
        }
    }

    // Function to inflict damage to the player
    public void DamagePlayer(int damageAmount)
    {
        if(playerCanTakeDamage)
        {
            currentHealth -= damageAmount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            healthbar.setHealth(currentHealth);
        }

        // Check if the player is dead
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Function to heal the player
    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthbar.setHealth(currentHealth);
    }

    // Function to handle player death
    private void Die()
    {
        // Check if the GameManager reference is not null
        if (GameManager.gameManager != null)
        {
            // Call the GameOver function from the GameManager
            GameManager.gameManager.GameOver();
        }
        else
        {
            Debug.LogWarning("GameManager reference not set in PlayerHealth script.");
        }
    }
}

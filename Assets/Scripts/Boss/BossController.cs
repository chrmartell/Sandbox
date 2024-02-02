using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    // Event to signal a phase change
    public static event System.Action<int> PhaseChange;

    //Ring
    public GameObject ring;

    // Phase variables
    private int currentPhase = 1;

    [SerializeField]
    private int currentBossHealth;
    [SerializeField]
    private int maxBossHealth = 100;
    //[SerializeField]
    //private int numOfBossHealthBars = 5;

    //[SerializeField] HealthBar healthbar;

    private void Start()
    {
        // Initialize currentBossHealth to maxBossHealth at the start
        currentBossHealth = maxBossHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // For testing purposes, switch phases on '1' or '2' key press
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchPhase(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchPhase(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchPhase(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchPhase(4);
        }
    }

    // Method to switch phases and trigger the PhaseChange event
    private void SwitchPhase(int newPhase)
    {
        if (newPhase != currentPhase)
        {
            currentPhase = newPhase;

            // Trigger the PhaseChange event
            PhaseChange?.Invoke(currentPhase);

            // Print debug message
            Debug.Log("Switched to Phase " + currentPhase);

            GameManager.gameManager.SetBossPhase(newPhase);

            EnableBossRing(currentPhase);
        }
    }
    private void EnableBossRing(int currentPhase)
    {
        if(currentPhase == 1) ring.SetActive(true);
        else ring.SetActive(false);
    }

    // Method to get the current phase
    public int GetCurrentPhase()
    {
        return currentPhase;
    }

     // Function to inflict damage to the player
    public void DamageBoss(int damageAmount)
    {
        currentBossHealth -= damageAmount;
        currentBossHealth = Mathf.Clamp(currentBossHealth, 0, maxBossHealth);

        Debug.Log("Damage Taken; Boss HP: " + currentBossHealth);

        //healthbar.setHealth(currentHealth);

        // Check if the player is dead
        if (currentBossHealth <= 0)
        {
            //BossDefeat();
            Debug.Log("Boss defeated!");
        }
    }
}

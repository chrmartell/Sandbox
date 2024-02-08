using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public static event System.Action<int> PhaseChange;

    [SerializeField] private GameObject ring;
    [SerializeField] private int maxBossHealth = 100;
    [SerializeField] private List<int> bossPhasePlan = new List<int>();

    [SerializeField] private int currentBossHealth; //just have it serialized to be see it for debugging
    [SerializeField] private int currentPhaseIndex = 0; //just have it serialized to be see it for debugging

    private void Start()
    {
        currentBossHealth = maxBossHealth;
    }

    private void Update()
    {
        /*
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
        */
    }

    private void SwitchPhase(int newPhase)
    {
        currentBossHealth = maxBossHealth;

        // Trigger the PhaseChange event
        PhaseChange?.Invoke(newPhase);
        Debug.Log("Switched to Phase " + newPhase);

        GameManager.gameManager.SetBossPhase(newPhase);
        EnableBossRing(newPhase);
    }

    private void EnableBossRing(int phase)
    {
        ring.SetActive(phase == 1);
    }

    public int GetCurrentPhase()
    {
        return bossPhasePlan[currentPhaseIndex];
    }

    public void DamageBoss(int damageAmount)
    {
        currentBossHealth -= damageAmount;
        currentBossHealth = Mathf.Clamp(currentBossHealth, 0, maxBossHealth);

        Debug.Log("Damage Taken; Boss HP: " + currentBossHealth);

        if (currentBossHealth <= 0 && currentPhaseIndex == bossPhasePlan.Count - 1)
        {
            Debug.Log("Boss defeated!");
        }
        else if (currentBossHealth <= 0 && currentPhaseIndex < bossPhasePlan.Count - 1)
        {
            currentPhaseIndex++;
            Debug.Log("Attempt to switch to phase: " + bossPhasePlan[currentPhaseIndex]);
            SwitchPhase(bossPhasePlan[currentPhaseIndex]);
        }
    }
}


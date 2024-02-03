using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager {get; private set;}

    public static event System.Action<int> OnBossPhaseChanged; // Event for phase changes
    public int CurrentBossPhase;


    void Awake()
    {
        if(gameManager != null && gameManager != this)
        {
            Destroy(this);
        }
        else
        {
            gameManager = this;
        }

        // Set the initial boss phase to match the BossController's currentPhase
        CurrentBossPhase = FindObjectOfType<BossController>()?.GetCurrentPhase() ?? 0;
    }

    void Start()
    {
        //UpdateGameState(GameState.SelectColor);
    }
    
    public void SetBossPhase(int phase)
    {
        CurrentBossPhase = phase;

        // Trigger the phase change event
        OnBossPhaseChanged?.Invoke(phase);
    }
    
    // Game over logic
    public void GameOver()
    {
        // Implement your game over logic here
        Debug.Log("Game Over");
    }
}

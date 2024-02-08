using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class GetSetBossPhase : Action
{
    // SharedInt variable to hold the boss phase
    public SharedInt BossPhase;

    public override TaskStatus OnUpdate()
    {
        // Check if the GameManager exists
        GameManager gameManager =  GameObject.FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            // Get the current boss phase from the GameManager
            int currentBossPhase = gameManager.CurrentBossPhase;

            // Set the value of BossPhase
            BossPhase.Value = currentBossPhase;

            // Return success to indicate that the task has been completed
            return TaskStatus.Success;
        }
        else
        {
            // GameManager is not found, return failure
            return TaskStatus.Failure;
        }
    }
}

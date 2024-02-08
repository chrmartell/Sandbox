using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CheckBossPhase : Conditional
{
    public int conditionalBossPhase;

    private GameManager gameManager;

    public override void OnStart()
    {
        // Get a reference to the GameManager object
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    public override TaskStatus OnUpdate()
    {
        if (gameManager != null)
        {
            int currentBossPhase = gameManager.CurrentBossPhase;

            // Check if the currentBossPhase is equal to conditionalBossPhase
            if (currentBossPhase == conditionalBossPhase)
            {
                return TaskStatus.Success;
            }
        }

        // If GameManager is not found or currentBossPhase doesn't match conditionalBossPhase, return failure
        return TaskStatus.Failure;
    }
}

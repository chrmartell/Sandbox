using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera ringCamera;
    public CinemachineVirtualCamera arenaCamera;
    public CinemachineVirtualCamera playerCamera;
    public CinemachineVirtualCamera testCamera;

    private CinemachineBrain cinemachineBrain;

    private void Start()
    {
        // Get the CinemachineBrain component
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();

        // Subscribe to the PhaseChange event
        //BossController.PhaseChange += OnPhaseChange;

        // Subscribe to the GameManager's phase change event
        GameManager.OnBossPhaseChanged += OnBossPhaseChanged;

        // Start with the RingCamera enabled
        EnableRingCamera();
    }

    private void OnDestroy()
    {
        // Unsubscribe from the PhaseChange event to avoid memory leaks
        //BossController.PhaseChange -= OnPhaseChange;

        // Unsubscribe from the GameManager's phase change event to avoid memory leaks
        GameManager.OnBossPhaseChanged -= OnBossPhaseChanged;
    }

    private void OnBossPhaseChanged(int newPhase)
    {
        // Switch cameras based on the boss fight phase
        if (newPhase == 1)
            EnableRingCamera();
        else if (newPhase == 2)
            EnableArenaCamera();
        else if (newPhase == 3)
            EnablePlayerCamera();
        else if (newPhase == 4)
            EnableTestCamera();
    }

    private void EnableRingCamera()
    {
        ringCamera.enabled = true;
        arenaCamera.enabled = false;
        playerCamera.enabled = false;
        testCamera.enabled = false;

        // Debug log to verify camera switch
        Debug.Log("Switched to RingCamera. Position: " + ringCamera.transform.position);
    
        // Use a smooth blend to transition to the RingCamera
        cinemachineBrain.m_DefaultBlend.m_Time = 1f;
        cinemachineBrain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
    }

    private void EnableArenaCamera()
    {
        ringCamera.enabled = false;
        arenaCamera.enabled = true;
        playerCamera.enabled = false;
        testCamera.enabled = false;

        // Debug log to verify camera switch
        Debug.Log("Switched to ArenaCamera. Position: " + arenaCamera.transform.position);
    
        // Use a smooth blend to transition to the ArenaCamera
        cinemachineBrain.m_DefaultBlend.m_Time = 1f;
        cinemachineBrain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
    }

    private void EnablePlayerCamera()
    {
        ringCamera.enabled = false;
        arenaCamera.enabled = false;
        playerCamera.enabled = true;
        testCamera.enabled = false;

        // Debug log to verify camera switch
        Debug.Log("Switched to PlayerCamera. Position: " + playerCamera.transform.position);
    
        // Use a smooth blend to transition to the PlayerCamera
        cinemachineBrain.m_DefaultBlend.m_Time = 1f;
        cinemachineBrain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
    }

    private void EnableTestCamera()
    {
        ringCamera.enabled = false;
        arenaCamera.enabled = false;
        playerCamera.enabled = false;
        testCamera.enabled = true;

        // Debug log to verify camera switch
        Debug.Log("Switched to TestCamera. Position: " + testCamera.transform.position);
    
        // Use a smooth blend to transition to the TestCamera
        cinemachineBrain.m_DefaultBlend.m_Time = 1f;
        cinemachineBrain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
    }
}

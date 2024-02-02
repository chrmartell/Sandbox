using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPointController : MonoBehaviour
{
    public Transform player; // Reference to the player transform
    public Transform mouseTarget; // Reference to the MouseTarget transform
    public float maxDistance = 5f; // Maximum distance from the player

    private Vector3 initialOffset; // Initial offset from the player to this DashPoint

    void Start()
    {
        // Calculate initial offset
        initialOffset = transform.position - player.position;
    }

    void Update()
    {
        // Ensure DashPoint doesn't go beyond maxDistance
        Vector3 playerToMouseTarget = mouseTarget.position - player.position;
        Vector3 limitedOffset = Vector3.ClampMagnitude(playerToMouseTarget, maxDistance);

        // Update DashPoint position
        transform.position = player.position + limitedOffset;

        // Rotate DashPoint to face MouseTarget
        transform.LookAt(mouseTarget);
    }
}

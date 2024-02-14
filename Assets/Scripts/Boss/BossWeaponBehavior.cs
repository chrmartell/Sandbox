using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeaponBehavior : MonoBehaviour
{
    [SerializeField] private int weaponDamage;
    private BoxCollider col;

    void Start()
    {
        col = GetComponent<BoxCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Check if the PlayerHealth script is present on the "Player" GameObject
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                // Call DamagePlayer function on the PlayerHealth script
                playerHealth.DamagePlayer(weaponDamage);
                Debug.Log("Player Hit! Damage dealt to the player.");
            }
            else
            {
                Debug.LogError("PlayerHealth script not found on the Player GameObject.");
            }
        }
    }

    public void EnableBossWeaponHitbox()
    {
        col.enabled = true;
    }
    public void DisableBossWeaponHitbox()
    {
        col.enabled = false;
    }
}

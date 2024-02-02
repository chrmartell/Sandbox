using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{
    [SerializeField] private int weaponDamage;
    private BoxCollider col;

    void Start()
    {
        col = GetComponent<BoxCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Check if the BossController script is present on the "Enemy" GameObject
            BossController bossController = other.gameObject.GetComponent<BossController>();

            if (bossController != null)
            {
                // Call DamageBoss function on the BossController script
                bossController.DamageBoss(weaponDamage);
                Debug.Log("Enemy Hit! Damage dealt to the boss.");
            }
            else
            {
                Debug.LogError("BossController script not found on the Enemy GameObject.");
            }
        }
    }

    public void EnableSwordHitbox()
    {
        col.enabled = true;
        Debug.Log("EnableSwordHitbox");
    }
    public void DisableSwordHitbox()
    {
        col.enabled = false;
        Debug.Log("DisableSwordHitbox");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletBehavior : MonoBehaviour
{
    [SerializeField] private int damage;
    private SphereCollider col;

    void Start()
    {
        col = GetComponent<SphereCollider>();
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
                bossController.DamageBoss(damage);
                //Debug.Log("Bullet: Enemy Hit! Damage dealt to the boss.");
                Destroy(gameObject);
            }
            else
            {
                //Debug.LogError("Bullet: BossController script not found on the Enemy GameObject.");
            }
        }
        if (other.gameObject.CompareTag("Projectile"))
        {
            /*// Check if the BossController script is present on the "Enemy" GameObject
            ProjectileBehavior projectileBehavior = other.gameObject.GetComponent<ProjectileBehavior>();

            if (projectileBehavior != null)
            {
                // Call DamageProjectile function on the ProjectileBehavior script
                ProjectileBehavior.DamageProjectile(damage);
                Debug.Log("Enemy projectile hit.");
            }
            else
            {
                Debug.LogError("ProjectileBehavior script not found on the Projectile GameObject.");
            }*/
        }
        if (other.gameObject.CompareTag("Ring") || other.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}

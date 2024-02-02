using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Projectile", menuName = "Projectiles/Projectile")]
public class Projectile : ScriptableObject
{
    public string projectileName;
    public int damage;
    public int speed;
}

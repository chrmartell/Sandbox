using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public GameObject shield;

    public void EnableShield()
    {
        shield.SetActive(true);
    }

    public void DisableShield()
    {
        shield.SetActive(false);
    }
}

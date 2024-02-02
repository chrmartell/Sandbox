using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] HealthBar healthbar;

    //Call this for another unit to construct their hp
    //public UnitHealth playerHealth = new UnitHealth(100, 100);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    /*void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha8))
        {
            playerTakeDmg(20);
            Debug.Log("Damage Taken; HP: " + GameManager.gameManager.playerHealth.Health);
        }
        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            playerHeal(10);
            Debug.Log("Damage Healed; HP: " + GameManager.gameManager.playerHealth.Health);
        }
    }

    private void playerTakeDmg(int dmg)
    {
        GameManager.gameManager.playerHealth.DmgPlayer(dmg);
        healthbar.setHealth(GameManager.gameManager.playerHealth.Health);
    }

    private void playerHeal(int healing)
    {
        GameManager.gameManager.playerHealth.HealPlayer(healing);
        healthbar.setHealth(GameManager.gameManager.playerHealth.Health);
    }*/
}

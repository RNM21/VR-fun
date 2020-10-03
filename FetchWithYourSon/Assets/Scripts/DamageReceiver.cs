using UnityEngine;

public class DamageReceiver : MonoBehaviour, IEntity
{
    //This script will keep track of player HP
    public float playerHP = 100;

    public void ApplyDamage(float points)
    {
        playerHP -= points;

        if (playerHP <= 0)
        {
            //Player is dead
           // playerController.canMove = false;
            playerHP = 0;
        }
    }
}
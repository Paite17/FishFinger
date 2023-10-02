using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float enemyHealth;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float damage;
    [SerializeField] private float scoreBase;
    public float EnemyHealthh
    {
        get { return enemyHealth; }
        set { enemyHealth = value; }
    }

    public float Damage
    {
        get { return damage; }
    }

    public void TakeDamage(float amount, float distance)
    {   
        enemyHealth -= amount;
        DeathCheck(distance);
    }

    private void DeathCheck(float distance)
    {
        if (enemyHealth <= 0)
        {
            Destroy(gameObject);

            float scoreToAdd = scoreBase - (distance / 2);

            Debug.Log(scoreToAdd);

            if (scoreToAdd <= 0)
            {
                scoreToAdd = 10;
            }
            GameObject.Find("Player").GetComponent<Player>().AddScore(scoreToAdd);
        }
    }
}

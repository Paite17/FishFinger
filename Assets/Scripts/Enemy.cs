using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float enemyHealth;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float damage;
    [SerializeField] private float scoreBase;
    [SerializeField] private GameObject deathScoreText;
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
            Vector3 currentPos = transform.position;
            Transform camPos = GameObject.Find("CameraPos").transform;
            GameObject temp = Instantiate(deathScoreText, camPos);
            temp.transform.position = new Vector3(transform.position.x, transform.position.y , transform.position.z);

            float scoreToAdd = scoreBase - (distance / 2);
            temp.GetComponent<TMP_Text>().text = scoreToAdd.ToString("F0");
            
            Debug.Log(scoreToAdd);

            if (scoreToAdd <= 0)
            {
                scoreToAdd = 10;
            }
            GameObject.Find("Player").GetComponent<Player>().AddScore(scoreToAdd);

            Destroy(gameObject);
        }
    }
}

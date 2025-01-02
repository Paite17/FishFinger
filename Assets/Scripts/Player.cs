using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float playerHealth = 100;
    [SerializeField] private float playerScore;
    [SerializeField] private Gun currentlyEquippedGun;

    private GameManager gm;

    public float PlayerHealth
    {
        get { return playerHealth; }
        set { playerHealth = value; }
    }

    public float PlayerScore
    {
        get { return playerScore; }
        set { playerScore = value; }
    }

    public Gun CurrentlyEquippedGun
    {
        get { return currentlyEquippedGun; }
        set { currentlyEquippedGun = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();
    }

    // receive damage from something
    public void TakeDamage(float dmgAmount)
    {
        playerHealth -= dmgAmount;
    }

    private void CheckHealth()
    {
        if (playerHealth <= 0)
        {
            playerHealth = 0;
            if (gm.currentState != GameState.RESULTS)
            {
                gm.GameOver();
            }
        }
    }

    public void AddScore(float scoreAmount)
    {
        playerScore += scoreAmount;
    }

  
}

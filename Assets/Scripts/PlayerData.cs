using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float highScore;

    public PlayerData(Player player)
    {
        highScore = player.PlayerScore;
    }

    public PlayerData(MainMenuScoreLoader data)
    {
        highScore = data.highScore;
    }

}

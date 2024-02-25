using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float highScore;
    public bool eggstraWork;
    public float[] gameSeed;
    public bool endlessMode;
    public PlayerData(Player player)
    {
        highScore = player.PlayerScore;
    }

    public PlayerData(GameManager data)
    {
        eggstraWork = data.eggstraWork;
    }

    public PlayerData(MainMenuScoreLoader data)
    {
        highScore = data.highScore;
        eggstraWork = data.eggstraWork;
        gameSeed = data.eggstraSeed;
        endlessMode = data.endlessMode;
    }

}

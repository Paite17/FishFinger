using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : ScriptableObject
{
    [SerializeField] private string mapName;
    [SerializeField][TextArea] private string mapDescription;
    [SerializeField] private List<GameObject> uniqueEnemies;  // enemies that may be specific to a map

    public string MapName
    {
        get { return mapName; }
    }

    public string MapDescription
    {
        get { return mapDescription; }
    }

    public List<GameObject> UniqueEnemies
    {
        get { return uniqueEnemies; }
    }
}

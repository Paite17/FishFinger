using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    
    public static void SaveGame(Player player)
    {
        // saves data of the player (including lvl, health, xp, etc)
        BinaryFormatter formatter = new BinaryFormatter();
        string path;
        path = Application.persistentDataPath + "/PlayerData.fish";
        
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveGame(MainMenuScoreLoader player)
    {
        // saves data of the player (including lvl, health, xp, etc)
        BinaryFormatter formatter = new BinaryFormatter();
        string path;
        path = Application.persistentDataPath + "/PlayerData.fish";

        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        // making the same variable again like a boss
        string path;
        path = Application.persistentDataPath + "/PlayerData.fish";


        if (File.Exists(path))
        {
            // load data
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

   
    // check if a save file for the player exists and if it doesn't, return false 
    public static bool DoesPlayerFileExist()
    {
        string path = Application.persistentDataPath + "/PlayerData.fish";
        if (File.Exists(path))
        {
            Debug.Log("exists");
            return true;
        }
        else
        {
            Debug.Log("Doesn't exist for some reason");
            return false;
        }
    }




    // initially create a player file with data and do nothing if there is one
    public static void CreatePlayerFile(Player player)
    {
        string path;


        path = Application.persistentDataPath + "/PlayerData.fish";

        if (!File.Exists(path))
        {
            Debug.Log("GameData doesn't exist, creating!");
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            PlayerData data = new PlayerData(player);

            formatter.Serialize(stream, data);
            stream.Close();
        }
        else
        {
            Debug.Log("GameData exists, doing nothing!");
            return;
        }
    }

    public static void CreatePlayerFile(MainMenuScoreLoader player)
    {
        string path;


        path = Application.persistentDataPath + "/PlayerData.fish";

        if (!File.Exists(path))
        {
            Debug.Log("GameData doesn't exist, creating!");
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            PlayerData data = new PlayerData(player);

            formatter.Serialize(stream, data);
            stream.Close();
        }
        else
        {
            Debug.Log("GameData exists, doing nothing!");
            return;
        }
    }

    public static void DeletePlayerFile()
    {

        string path;
        
        path = Application.persistentDataPath + "/PlayerData.fish";
        
        File.Delete(path);
    }


}

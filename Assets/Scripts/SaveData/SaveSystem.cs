using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    
    public static void SaveGame(GameManager GM, bool endless)
    {
        // saves data of the player (including lvl, health, xp, etc)
        BinaryFormatter formatter = new BinaryFormatter();
        string path;
        if (!endless)
        {
            path = Application.persistentDataPath + "/gameData.poo";
        }
        else
        {
            path = Application.persistentDataPath + "/endlessData.poo";
        }
        
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData(GM);

        formatter.Serialize(stream, data);
        stream.Close();
    }


    public static GameData LoadPlayer(bool endless)
    {
        // making the same variable again like a boss
        string path;

        if (!endless)
        {
            path = Application.persistentDataPath + "/gameData.poo";
        }
        else
        {
            path = Application.persistentDataPath + "/endlessData.poo";
        }

        if (File.Exists(path))
        {
            // load data
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SaveSettings(SystemSettings settings)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/settings.poo";
        FileStream stream = new FileStream(path, FileMode.Create);

        SystemData data = new SystemData(settings);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SystemData LoadSettings()
    {
        string path = Application.persistentDataPath + "/settings.poo";
        if (File.Exists(path))
        {
            // load data
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SystemData data = formatter.Deserialize(stream) as SystemData;
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
        string path = Application.persistentDataPath + "/gameData.poo";
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

    public static bool DoesEndlessFileExist()
    {
        string path = Application.persistentDataPath + "/endlessData.poo";
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

    public static bool DoesSettingsFileExist()
    {
        string path = Application.persistentDataPath + "/settings.poo";
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
    public static void CreatePlayerFile(GameManager player, bool endless)
    {
        string path;

        if (!endless)
        {
            path = Application.persistentDataPath + "/gameData.poo";
        }
        else
        {
            path = Application.persistentDataPath + "/endlessData.poo";
        }

        if (!File.Exists(path))
        {
            Debug.Log("GameData doesn't exist, creating!");
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            GameData data = new GameData(player);

            formatter.Serialize(stream, data);
            stream.Close();
        }
        else
        {
            Debug.Log("GameData exists, doing nothing!");
            return;
        }
    }

    public static void CreateSettingsFile(SystemSettings settings)
    {
        string path = Application.persistentDataPath + "/settings.poo";
        if (!File.Exists(path))
        {
            Debug.Log("GameData doesn't exist, creating!");
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            SystemData data = new SystemData(settings);

            formatter.Serialize(stream, data);
            stream.Close();
        }
        else
        {
            Debug.Log("settings exists, doing nothing!");
            return;
        }
    }

    public static void DeletePlayerFile(bool endless)
    {

        string path;
        
        if (!endless)
        {
            path = Application.persistentDataPath + "/gameData.poo";
        }
        else
        {
            path = Application.persistentDataPath + "/endlessData.poo";
        }

        File.Delete(path);
    }

    public static void DeleteSettingsFile()
    {
        string path = Application.persistentDataPath + "/settings.poo";
        File.Delete(path);
    }
}

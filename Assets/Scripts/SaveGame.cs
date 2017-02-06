using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;

public static class SaveGame {
    public static List<Game> savedGames = new List<Game>();

    public static void Save()
    {
        savedGames.Add(Game.current);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
        bf.Serialize(file, SaveGame.savedGames);
        file.Close();
    }

    public static void Load()
    {
        Game.current.UnloadGame();
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            SaveGame.savedGames = (List<Game>)bf.Deserialize(file);
            file.Close();
            //Remove this later
            Game.current.LoadGame(0);
        }
    }
}

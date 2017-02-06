using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;

[System.Serializable]
public class Game {
    public static Game current;
    public PlayerStats currentStats;
    //Units
    public List<Army> AllArmies = new List<Army>();
    //public List<Monster> AllMonsters;
    //public List<Hero> AllHeroes;
    public List<VillageNode> AllNodes;
    public Game()
    {
        current = this;
    }

    //
    public void LoadGame(int GameToLoad)
    {
        Game.current = SaveGame.savedGames[GameToLoad];
        MessageDispatcher.SendMessage("SAVE_LoadGame");
    }

    public void UnloadGame()
    {
        MessageDispatcher.SendMessage("SAVE_UnloadGame");
    }
}

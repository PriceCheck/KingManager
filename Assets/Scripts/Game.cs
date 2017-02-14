using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;

[System.Serializable]
public class Game {
    public bool Loaded = false;
    public static Game current;
    public PlayerStats currentStats;
    //Units
    public List<Army> AllArmies = new List<Army>();
    //public List<Monster> AllMonsters;
    //public List<Hero> AllHeroes;
    public List<VillageNode> AllNodes = new List<VillageNode>();
    public Game()
    {
        current = this;
    }

    public void uploadCurrentGameState(ArmyRepresentation[] armies, VillageNode[] nodes)
    {
         
        for(int i = 0; i < armies.Length; ++i)
        {
            AllArmies.Add(armies[i].myArmy);
        }
       
        AllNodes = new List<VillageNode>(nodes);
        AllNodes.Sort(delegate (VillageNode x, VillageNode y) { return x.ID.CompareTo(y.ID); });
    }
    
    //
    public void LoadGame(int GameToLoad)
    {
        Game.current = SaveGame.savedGames[GameToLoad];
        Loaded = true;
        MessageDispatcher.SendMessage("SAVE_LoadGame");
    }

    public void UnloadGame()
    {
        Loaded = false;
        MessageDispatcher.SendMessage("SAVE_UnloadGame");
    }
}

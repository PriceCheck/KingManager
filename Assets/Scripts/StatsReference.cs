/*This file was auto-generated. Any changes that are made will be overwritten*/
/*Written By: Taylor Riviera*/
/*Version 0.1*/
using UnityEngine;
using System.Collections;

public enum UnitType { Foot_Solider, Archer, Length };

public static class StatsReference
 {
	public struct UnitStats {
		public int Hit_Points;
		public int Damage;
		public int Accuracy;
		public int Size;
		public int Speed;
		public string Name;
		public bool Is_Ranged;
		public bool Can_Be_Hit_Ground;
		public bool Can_Be_Hit_Ranged;

		public UnitStats(int Hit_Points_, int Damage_, int Accuracy_, int Size_, int Speed_, string Name_, bool Is_Ranged_, bool Can_Be_Hit_Ground_, bool Can_Be_Hit_Ranged_)
		{
			Hit_Points = Hit_Points_;
			Damage = Damage_;
			Accuracy = Accuracy_;
			Size = Size_;
			Speed = Speed_;
			Name = Name_;
			Is_Ranged = Is_Ranged_;
			Can_Be_Hit_Ground = Can_Be_Hit_Ground_;
			Can_Be_Hit_Ranged = Can_Be_Hit_Ranged_;
		}
	}
	public static UnitStats[] UnitStatsArray = new UnitStats[] {
		new UnitStats(5 , 2 , 75 , 1 , 5 , "Foot Solider" , false , true , true ),
		new UnitStats(2 , 4 , 30 , 1 , 3 , "Archer" , true , true , true ),
	};
}
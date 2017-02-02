using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum TokenType {String, Int, Float, Bool};

public class UnitStatsImporter {
    static public string filePath = "KingManager\\Assets\\SpreadSheets\\UnitStats.txt";
    static public string newScriptPath = "KingManager\\Assets\\Scripts\\StatsReference.cs";
	// Use this for initialization
	void Start () {

        UpdateUnitStats();
    }

    static public void UpdateUnitStats()
    {
        string[] FileLines = System.IO.File.ReadAllLines(@"..\" + filePath);
        int NumLines = FileLines.Length;

        string[][] TokenArray = new string[NumLines][];
        for(int i = 0; i < NumLines; ++i)
        {
            TokenArray[i] = Tokenize(FileLines[i], '\t');
        }

        System.IO.File.Create(@"..\" + newScriptPath).Close();
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"..\" + newScriptPath))
        {
            file.Write( "/*This file was auto-generated. Any changes that are made will be overwritten*/\n");
            file.Write("/*Written By: Taylor Riviera*/\n");
            file.Write("/*Version 0.1*/\n");
            file.Write("using UnityEngine;\nusing System.Collections;\n\n");

            //Enum with unit types
            file.Write("public enum UnitType {");
            for (int i = 1; i < NumLines - 1; ++i)
            {
                file.Write(" " + StripQuotes(TokenArray[i][0]) + ",");
            }
            file.Write(" " + StripQuotes(TokenArray[NumLines - 1][0]) + ", Length };\n\n");

            //Class Starts Here
            file.Write("public static class StatsReference\n {\n");
            //Unit Stats Structure
            file.Write("\tpublic struct UnitStats {\n");

            for (int i = 1; i < TokenArray[0].Length; ++i)
            {
                file.Write("\t\tpublic " + ReadTokenTypeAsString(TokenArray[1][i]) + " " + StripQuotes(TokenArray[0][i]) + ";\n");
            }
            file.Write("\n\t\tpublic UnitStats(");
            for (int i = 1; i < TokenArray[0].Length; ++i)
            {
                file.Write(ReadTokenTypeAsString(TokenArray[1][i]) + " " + StripQuotes(TokenArray[0][i]) + "_");
                if (i < TokenArray[0].Length - 1)
                {
                    file.Write(", ");
                }
            }
            file.Write(")\n\t\t{");
            for(int i = 1; i < TokenArray[0].Length; ++i)
            {
                file.Write("\n\t\t\t" + StripQuotes(TokenArray[0][i]) + " = " + StripQuotes(TokenArray[0][i]) + "_;");
            }
            file.Write("\n\t\t}\n\t}\n");
            //End of Unit stats struct
            //Start Array Declaration
            file.Write("\tpublic static UnitStats[] UnitStatsArray = new UnitStats[] {");
            for (int i = 1; i < TokenArray.Length; ++i)
            {
                file.Write("\n\t\tnew UnitStats(");
                for (int j = 1; j < TokenArray[i].Length; ++j)
                {
                    if (ReadTokenType(TokenArray[i][j]) == TokenType.Bool)
                    {
                        if(TokenArray[i][j][0] == 'T')
                        {
                            file.Write("true");
                        }
                        else
                        {
                            file.Write("false");
                        }
                       
                    }
                    else
                    {
                        file.Write(TokenArray[i][j]);
                    }
                        
                    if (ReadTokenType(TokenArray[i][j]) == TokenType.Float)
                    {
                        file.Write("f ");
                    }
                    else
                    {
                        file.Write(" ");
                    }
                    
                    if(j < TokenArray[i].Length - 1)
                    {
                        file.Write(", ");
                    }
                }
                file.Write("),");
            }
            file.Write("\n\t};");
            file.Write ("\n" + serializeFunction(1, TokenArray[0]));
            //End of File
            file.Write("\n}");
            //file.Write();

        }
      //  print("File Written Successfully!");
    }
    //Wrap in quotes
    static string WIP(string input)
    {
        return "\"" + input + "\"";
    }

    //Break this into different static functions

    static string serializeFunction(int Tabs, string[] VariableNames)
    {
        string output = "";
        string tabs = "";
        for (int i = 0; i < Tabs; ++i)
        {
            tabs += "\t";
        }
        output = tabs + "public static string serializeStats(UnitStats stats, UnitType type) {\n";
        output += tabs + "\t return" + WIP("{") + " + type + \n";
        for (int i = 0; i < VariableNames.Length; ++i)
        {
            output += tabs + "\t" + WIP(",") + " + stats." + StripQuotes(VariableNames[i]) + " + \n";
        }
        output += tabs + "\t"+ WIP("}") + ";\n";
        output += tabs + "}\n";

        return output;
        /*public static string serializeStats(UnitStats stats, UnitType type)
        string output = "{" + type + "," + ... + "}";

        return output;*/
    }
    static string isEqualFunction(int Tabs, string[] VariableNames)
    {
        //TODO: this one
        return null;
    }


    static TokenType ReadTokenType(string input)
    {
        if (input[0] == '\"')
        {
            return TokenType.String;
        }
        else if (input[0] == 'T' || input[0] == 'F')
        {
            return TokenType.Bool;
        }
        else
        {
            for(int i = 0; i < input.Length; ++i)
            {
                if(input[i] == '.')
                {
                    return TokenType.Float;
                }
            }
            return TokenType.Int;
        }
       
    }

    static string ReadTokenTypeAsString(string input)
    {
        if (input[0] == '\"')
        {
            return "string";
        }
        else if (input[0] == 'T' || input[0] == 'F')
        {
            return "bool";
        }
        else
        {
            for (int i = 0; i < input.Length; ++i)
            {
                if (input[i] == '.')
                {
                    return "float";
                }
            }
            return "int";
        }
    }

    static string StripQuotes(string input)
    {
        return input.Substring(1, input.Length - 2);
    }

    static int StringToInt(string input)
    {
        int output = 0;
        for(int i = 0; i < input.Length; ++i)
        {
            output *= 10;
            output += input[i] - '0';
        }
        return output;
    }

    static float StringToFloat(string input)
    {
        float output = 0;
        int i;
        for (i = 0; i < input.Length; ++i)
        {
            if(input[i] ==  '.')
            {
                ++i;
                break;
            }
            output *= 10;
            output += input[i] - '0';
        }
        float Numberator = 0;
        float divisor = 1;
        for (; i < input.Length; ++i)
        {

            Numberator *= 10;
            Numberator += input[i] - '0';
            divisor *= 10;
        }
        output += Numberator / divisor;

        return output;
    }

    static string[] Tokenize(string input, char Delimiter)
    {
        List<string> tokens = new List<string>();
        int previousTab = -1;
        int LettersToTake = 0;
        for(int i = 0; i < input.Length; ++i)
        {
            
            if (input[i] == Delimiter)
            {
                tokens.Add(input.Substring(previousTab + 1, LettersToTake));
                LettersToTake = 0;
                previousTab = i;
            }
            else if(i == input.Length - 1)
            {
                tokens.Add(input.Substring(previousTab + 1, LettersToTake + 1));
            }
            else
            {
                ++LettersToTake;
            }
        }
        tokens.TrimExcess();
        return (string[])tokens.ToArray();
    }

    // Update is called once per frame
    static void Update () {
	
	}
}

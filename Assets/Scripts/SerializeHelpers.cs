using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SerializeHelpers  {

   /* public static TokenType ReadTokenType(string input)
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
            for (int i = 0; i < input.Length; ++i)
            {
                if (input[i] == '.')
                {
                    return TokenType.Float;
                }
            }
            return TokenType.Int;
        }

    }*/

    public static string ReadTokenTypeAsString(string input)
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

    public static string StripQuotes(string input)
    {
        return input.Substring(1, input.Length - 2);
    }

    public static int StringToInt(string input)
    {
        int output = 0;
        for (int i = 0; i < input.Length; ++i)
        {
            output *= 10;
            output += input[i] - '0';
        }
        return output;
    }

    public static float StringToFloat(string input)
    {
        float output = 0;
        int i;
        for (i = 0; i < input.Length; ++i)
        {
            if (input[i] == '.')
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

    public static string[] Tokenize(string input, char Delimiter)
    {
        List<string> tokens = new List<string>();
        int previousTab = -1;
        int LettersToTake = 0;
        for (int i = 0; i < input.Length; ++i)
        {

            if (input[i] == Delimiter)
            {
                tokens.Add(input.Substring(previousTab + 1, LettersToTake));
                LettersToTake = 0;
                previousTab = i;
            }
            else if (i == input.Length - 1)
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
}

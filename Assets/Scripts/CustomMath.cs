using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomMath
{
    public static float RoundToNearestFactorOf(float factorOf, float from)
    {
        float roundHigher = Mathf.Ceil(from / factorOf);
        float roundLower = Mathf.Floor(from / factorOf);

        float higherMulti = roundHigher * factorOf;
        float lowerMulti = roundLower * factorOf;

        return lowerMulti;
    }

    public static float Truncate(float value)
    {
        return Mathf.Round(value * 100) / 100.0f;
    }

    public static bool GreaterThanLessThan(float greaterThan, float value, float lessThan)
    {
        if (value > greaterThan && value < lessThan)
        {
            return true; 
        }
        else
        {
            return false; 
        }
    }

    public static float WorldSpaceToSongPos(float notePosition, int timeSig)
    {
        return Mathf.Abs(Truncate((notePosition / timeSig) / (SongInfo.instance.bpm / 60)));
    }

    public static float SongPosToWorldSpace(float songPosition, int timeSig)
    {
        return -(((SongInfo.instance.bpm * timeSig) * songPosition) / 60);
    }
}


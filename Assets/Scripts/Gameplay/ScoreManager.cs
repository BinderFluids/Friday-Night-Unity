using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class ScoreManager
{
    private static int points;
    private static int combo;
    private static float health;

    public static GameObject[] pointObjects;
    public static GameObject[] ratings;

    public static void AddCombo()
    {
        combo++;
    }

    public static void KillCombo()
    {
        combo = 0;
    }

    public static void AddPoints()
    {
        points += combo; 
    }
}

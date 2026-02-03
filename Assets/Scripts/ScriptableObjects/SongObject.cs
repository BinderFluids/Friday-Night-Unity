using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Song", menuName = "Song")]
public class SongObject : ScriptableObject
{
    public AudioClip music;
    public AudioClip voices;

    public int timeSigTop = 4;
    public int timeSigBottom = 4;

    public int gridCount;
}

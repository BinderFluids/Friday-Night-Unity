using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SongPicker : MonoBehaviour
{
    public SongInfo songInfo;
    public TMP_Dropdown dropdown;
    public List<SongObject> songs = new List<SongObject>();

    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource voices; 

    private void Start()
    {
        songInfo.songObject = songs[0];
        music.clip = songs[0].music;
        voices.clip = songs[0].voices;

        //StartCoroutine(songInfo.LoadSongJSON());
        dropdown.onValueChanged.AddListener(delegate { OnDropdownChange(dropdown); });
    }

    private void OnDropdownChange(TMP_Dropdown change)
    {
        songInfo.songObject = songs[dropdown.value];
        music.clip = songs[dropdown.value].music;
        voices.clip = songs[dropdown.value].voices;
        StartCoroutine(songInfo.LoadSongJSON());
    }

    public void SetDropDownValue(SongObject songObject)
    {
        dropdown.value = songs.IndexOf(songObject);
        songInfo.songObject = songs[dropdown.value];
    }
}

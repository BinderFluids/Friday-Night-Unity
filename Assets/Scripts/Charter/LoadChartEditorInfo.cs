using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;


public class LoadChartEditorInfo : MonoBehaviour
{
    private static bool loadedScenes = false; 

    public ArrowPicker arrowPicker;
    public StagePicker stagePicker;
    public ChartCharacterPicker[] characterPickers;
    public SongPicker songPicker; 

    public void Awake()
    {
        LoadArrows();
        LoadStages();
        StartCoroutine(LoadCharacters(0));
        StartCoroutine(LoadCharacters(1));
        LoadSongs();
    }

    public void LoadArrows()
    {
        foreach (ArrowObject arrowType in Resources.LoadAll<ArrowObject>("Arrows"))
        {
            arrowPicker.arrowTypes.Add(arrowType);
        }
    }
    public void LoadStages()
    {
        foreach (string stage in Cache.stageCache)
        {
            stagePicker.stages.Add(stage);
            stagePicker.dropdown.options.Add(new TMP_Dropdown.OptionData {text = Path.GetFileNameWithoutExtension(stage)});
        }
    }

    public IEnumerator LoadCharacters(int whichOne)
    {
        foreach (System.Collections.Generic.KeyValuePair<string, CharacterObject> dictionary in Cache.characterCache)
        {
            characterPickers[whichOne].characters.Add(dictionary.Value);
            characterPickers[whichOne].dropdown.options.Add(new TMP_Dropdown.OptionData { text = dictionary.Value.name });
        }

        yield return SongInfo.instance.BF != null && SongInfo.instance.enemy != null; 

        switch (whichOne)
        {
            case 0:
                characterPickers[whichOne].SetDropDownValue(SongInfo.instance.BF);
                break;
            case 1:
                characterPickers[whichOne].SetDropDownValue(SongInfo.instance.enemy);
                break;
        }
    }

    public void LoadSongs()
    {
        foreach (SongObject song in Resources.LoadAll<SongObject>("Songs"))
        {
            songPicker.songs.Add(song);
            songPicker.dropdown.options.Add(new TMP_Dropdown.OptionData { text = song.name });
        }
    }

    public CharacterObject LoadCharacterJSON(string jsonFileInfo)
    {
        CharacterObject newCharacterObject = ScriptableObject.CreateInstance<CharacterObject>();
        CharacterObjectSave save = new CharacterObjectSave();
        JsonUtility.FromJsonOverwrite(jsonFileInfo, save);

        newCharacterObject.name = save.name;

        newCharacterObject.defaultDirection = save.defaultDirection;

        return newCharacterObject;
    }
}
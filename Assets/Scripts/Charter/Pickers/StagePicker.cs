using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StagePicker : MonoBehaviour {

    public SongInfo songInfo;
    public TMP_Dropdown dropdown;
    public List<string> stages = new List<string>(); 

    private void Start()
    {
        dropdown.onValueChanged.AddListener(delegate { OnDropdownChange(dropdown);});
    }

    private void OnDropdownChange(TMP_Dropdown change)
    {
        songInfo.stage = dropdown.options[dropdown.value].text;
    }

    public void SetDropDownValue(string stage)
    {
        dropdown.value = stages.IndexOf(stage);
        songInfo.stage = stages[dropdown.value];
    }
}

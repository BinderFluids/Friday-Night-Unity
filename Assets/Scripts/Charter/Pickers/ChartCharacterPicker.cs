using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChartCharacterPicker : MonoBehaviour
{
    public SongInfo songInfo;
    public TMP_Dropdown dropdown;
    public List<CharacterObject> characters = new List<CharacterObject>();
    [SerializeField] private CharacterType characterType;
    private enum CharacterType {
        Player,
        Enemy
    }

    private void Start()
    {
        dropdown.onValueChanged.AddListener(delegate { OnDropdownChange(dropdown); });
    }

    private void OnDropdownChange(TMP_Dropdown change)
    {

        if (characterType == CharacterType.Player)
        {
            songInfo.BF = characters[dropdown.value];
        }

        if (characterType == CharacterType.Enemy)
        {
            songInfo.enemy = characters[dropdown.value];
        }
        
    }

    public void SetDropDownValue(CharacterObject characterObject)
    {
        dropdown.value = characters.IndexOf(characterObject);
        dropdown.SetValueWithoutNotify(dropdown.value);

        if (characterType == CharacterType.Player)
        {
            songInfo.BF = characters[dropdown.value];
        }

        if (characterType == CharacterType.Enemy)
        {
            songInfo.enemy = characters[dropdown.value];
        }
    }
}

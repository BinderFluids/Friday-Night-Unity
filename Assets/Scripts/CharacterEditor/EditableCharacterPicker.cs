using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditableCharacterPicker : MonoBehaviour
{
    public RuntimeEditableCharacter character;
    public TMP_Dropdown dropdown;
    public List<CharacterObject> characters = new List<CharacterObject>();

    private void Start()
    {
        dropdown.onValueChanged.AddListener(delegate { OnDropdownChange(dropdown); });
        OnDropdownChange(dropdown);
        //StartCoroutine(character.Bop());
    }

    private void OnDropdownChange(TMP_Dropdown change)
    {
        character.SetValues(characters[dropdown.value]);
    }

    public void SetDropDownValue(CharacterObject characterObject)
    {
        dropdown.value = characters.IndexOf(characterObject);
        character.SetValues(characters[dropdown.value]);
    }
}

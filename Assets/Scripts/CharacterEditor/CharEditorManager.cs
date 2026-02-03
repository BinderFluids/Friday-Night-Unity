using System.Collections;
using System.Collections.Generic;
using System.IO; 
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CharEditorManager : MonoBehaviour
{
    public RuntimeEditableCharacter runtimeCharacter;
    public GameObject characterGameObject; 

    public EditableCharacterPicker characterPicker;

    public TMP_InputField transformInput; 
    public float transformBy = .25f; 

    private void Awake()
    {
        LoadCharacters(); 
    }

    public void LoadCharacters()
    {
        foreach (System.Collections.Generic.KeyValuePair<string, CharacterObject> dictionary in Cache.characterCache)
        {
            characterPicker.characters.Add(dictionary.Value);
            characterPicker.dropdown.options.Add(new TMP_Dropdown.OptionData { text = dictionary.Value.name });
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            runtimeCharacter.ChangeOffset(new Vector2(-transformBy, 0));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            runtimeCharacter.ChangeOffset(new Vector2(0, -transformBy));
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            runtimeCharacter.ChangeOffset(new Vector2(0, transformBy));
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            runtimeCharacter.ChangeOffset(new Vector2(transformBy, 0));
        }
    }

    private CharacterObjectSave NewJSON()
    {
        CharacterObjectSave save = new CharacterObjectSave();
        CharacterObject character = runtimeCharacter.characterObject;

        save.name = character.name; 

        save.defaultDirection = character.defaultDirection;

        save.spriteSheetPath = character.spriteSheetPath; 

        
        foreach (AnimInfo animInfo in character.animInfos)
        {
            save.animInfos.Add(JsonUtility.ToJson(animInfo));
        }

        return save; 
    }

    //called from button
    public void CreateJSON()
    {
        CharacterObjectSave save = NewJSON();

        if (!File.Exists(Application.persistentDataPath + "/Characters/" + runtimeCharacter.characterObject.name + ".json"))
        {
            File.Create(Application.persistentDataPath + "/Characters/" + runtimeCharacter.characterObject.name + ".json");
        }

        string jsonFileInfo = JsonUtility.ToJson(save);

        File.WriteAllText(Application.persistentDataPath + "/Characters/" + runtimeCharacter.characterObject.name + ".json", jsonFileInfo);
    }

    public void GoToCreator()
    {
        SceneManager.LoadScene(2);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
}

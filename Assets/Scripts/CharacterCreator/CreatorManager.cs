using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement; 
using System.Xml.Linq;
using TMPro;

public class CreatorManager : MonoBehaviour {
    [SerializeField] private NewCharPicker newCharPicker;

    [SerializeField] private TMP_InputField inputField;
    private TMP_Dropdown directionPicker;
    private bool directionPicked;
    [SerializeField] private AnimPlayer animPlayer;
    [SerializeField] private SpriteRenderer charSprite;
    
    private int buttonIndex;
    private List<string> imagePaths = new List<string>();

    [SerializeField] private TMP_Text questionText; 
    public CharacterObject publicCharacter;
    //idle left down up right missleft missdown missup missright
    [SerializeField] private GameObject parentThing; 
    [SerializeField] private List<GameObject> directionButtons = new List<GameObject>();
    [SerializeField] private List<int> arrowDirections = new List<int>() { -1, 0, 1, 2, 3, 4, 5, 6, 7 };
    [SerializeField] private List<string> directionNames = new List<string> { "idle", "left", "down", "up", "right", "mleft", "mdown", "mup", "mright" };

    private bool stepCreation;

    private void Start()
    {
        foreach(GameObject button in directionButtons)
        {
            button.GetComponent<Button>().onClick.AddListener(delegate { StepButton(button); });
        }

        LoadImages();
    }

    //get images from /images/characters
    public void LoadImages()
    {
        foreach (string doc in Directory.GetFiles(Application.persistentDataPath + "/Images/Characters"))
        {
            //iterate through each xml file in the folder
            if (doc.Contains(".xml"))
            {
                XDocument xDoc = XDocument.Load(doc);
                IEnumerable<XElement> items = xDoc.Descendants();
                string imagePath = ""; 

                //get the image that each xml file references to 
                foreach (XElement item in items)
                {
                    if(item.ToString().Contains("imagePath"))
                    {
                        imagePath = item.Attribute("imagePath").Value; 
                    }
                }

                //add sprite to dropdown
                newCharPicker.sprites.Add(Cache.spriteCache[imagePath]);
                newCharPicker.dropdown.options.Add(new TMP_Dropdown.OptionData { text = Path.GetFileNameWithoutExtension(imagePath) });

                //add docs to dropdown list for later reference
                newCharPicker.docs.Add(xDoc);

                //add paths of docs to list
                imagePaths.Add(imagePath);
            }
        }
    }

    private CharacterObject CreateData(XDocument doc)
    {
        int animIndex = -1;

        IEnumerable<XElement> items = doc.Descendants().Elements<XElement>();  
        CharacterObject characterObject = ScriptableObject.CreateInstance<CharacterObject>();

        //loop through xml elements
        foreach (var item in items)
        {

            //string inputData = "Abundantcode.com-023";
            //var data = Regex.Match(inputData, @"\d+").Value;

            //seperates each animation
            if (item.Attribute("name").Value.Contains("0000"))
            {
                animIndex++;
                characterObject.animInfos.Add(new AnimInfo());
                characterObject.animInfos[animIndex].name = item.Attribute("name").Value;
            }

            //gets attributes from xml and turns them into integers
            int newX = int.Parse(item.Attribute("x").Value);
            int newY = int.Parse(item.Attribute("y").Value);
            int newWidth = int.Parse(item.Attribute("width").Value);
            int newHeight = int.Parse(item.Attribute("height").Value);

            int newFrameX = 0;
            int newFrameY = 0;

            if(item.ToString().Contains("frameX"))
            {
                newFrameX = int.Parse(item.Attribute("frameX").Value);
                newFrameY = int.Parse(item.Attribute("frameY").Value);
            }

            //applies information to character object
            characterObject.animInfos[animIndex].imagePositions.Add(new Vector2(-newX, newY));
            characterObject.animInfos[animIndex].maskSizes.Add(new Vector2(newWidth, newHeight));
            characterObject.animInfos[animIndex].framePositions.Add(new Vector2(-newFrameX, newFrameY));
            characterObject.animInfos[animIndex].offset = new Vector2(0, newHeight / 100f);
        }   

        return characterObject; 
    }

    //called from button
    public void PublicCreateNewCharacter()
    {
        StartCoroutine(CreateNewCharacter());
    }
    private IEnumerator CreateNewCharacter()
    {
        parentThing.SetActive(true); 

        CharacterObject character = CreateData(newCharPicker.docs[newCharPicker.dropdown.value]);
        stepCreation = false; 
        charSprite.sprite = newCharPicker.sprites[newCharPicker.dropdown.value];

        character.spriteSheetPath = imagePaths[newCharPicker.dropdown.value];

        questionText.text = "Name your character";

        while(!stepCreation)
        {
            yield return null; 
        }

        stepCreation = false; 
        character.name = inputField.text;
        int state = 0; 

        foreach (AnimInfo animInfo in character.animInfos)
        {
            questionText.text = "What is the name and direction of this animation?";

            //animate 
            animPlayer.Animate(character, state);
            directionPicked = false; 

            //wait till progress
            while (!stepCreation)
            {
                yield return null;
            }
            stepCreation = false;

            animInfo.name = inputField.text;
            
            if(directionPicked)
            {
                animInfo.arrowDirection = arrowDirections[buttonIndex];
                animInfo.name = directionNames[buttonIndex];

                arrowDirections.Remove(arrowDirections[buttonIndex]);
                directionNames.Remove(directionNames[buttonIndex]);
            }
            
            state++; 
        }

        questionText.text = "Finished, return to editor";
        parentThing.SetActive(false); 

        publicCharacter = character;

        Cache.characterCache.Add(publicCharacter.name, publicCharacter);

        StartCoroutine(CreateJSON());
    }

    //pick which int from weird list to apply to animation
    public void StepButton(GameObject button)
    {
        buttonIndex = directionButtons.IndexOf(button);
        directionPicked = true;

        directionButtons.Remove(button);

        StepCreation();

        button.SetActive(false); 
    }
    public void StepCreation()
    {
        stepCreation = true;
    }

    private CharacterObjectSave NewJSON()
    {
        CharacterObjectSave save = new CharacterObjectSave();
        CharacterObject character = publicCharacter;

        save.name = character.name;

        save.defaultDirection = character.defaultDirection;

        save.spriteSheetPath = character.spriteSheetPath;


        foreach (AnimInfo animInfo in character.animInfos)
        {
            save.animInfos.Add(JsonUtility.ToJson(animInfo));
        }

        return save;
    }

    public IEnumerator CreateJSON()
    {
        CharacterObjectSave save = NewJSON();

        if (!File.Exists(Application.persistentDataPath + "/Characters/" + publicCharacter.name + ".json"))
        {
            File.Create(Application.persistentDataPath + "/Characters/" + publicCharacter.name + ".json");
            yield return new WaitForSeconds(1);
        }

        string jsonFileInfo = JsonUtility.ToJson(save);

        File.WriteAllText(Application.persistentDataPath + "/Characters/" + publicCharacter.name + ".json", jsonFileInfo);
    }

    public void GoToEditor()
    {
        SceneManager.LoadScene(3);
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene(0); 
    }
}
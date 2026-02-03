using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Linq;

public static class Cache
{
    public static bool loaded = false; 

    public static Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();
    public static Dictionary<string, CharacterObject> characterCache = new Dictionary<string, CharacterObject>();
    public static List<string> stageCache = new List<string>(); 

    public static List<Sprite> spriteList = new List<Sprite>();

    public static void LoadAll()
    {
        loaded = false; 

        LoadImages();
        //LoadStages();
        LoadCharacters();
    }

    public static void LoadImages()
    {
        spriteCache.Clear(); 

        foreach (string doc in Directory.GetFiles(Application.persistentDataPath + "/Images/Characters"))
        {
            //iterate through each xml file in the folder
            if (doc.EndsWith(".xml"))
            {
                XDocument xDoc = XDocument.Load(doc); 
                IEnumerable<XElement> items = xDoc.Descendants();
                string imagePath = "";

                //get the image that each xml file references to 
                foreach (XElement item in items)
                {
                    if (item.ToString().Contains("imagePath"))
                    {
                        imagePath = item.Attribute("imagePath").Value;
                    }
                }

                //create sprite
                var imageBytes = File.ReadAllBytes(Application.persistentDataPath + "/Images/Characters/" + imagePath);
                Texture2D returningTex = new Texture2D(0, 0); //must start with a placeholder Texture object
                returningTex.LoadImage(imageBytes);

                Sprite newSprite = Sprite.Create(returningTex, new Rect(0, 0, returningTex.width, returningTex.height), new Vector2(0, 1), 100f, 0, SpriteMeshType.FullRect);

                spriteCache.Add(imagePath, newSprite);
            }
        }
    }

    public static void LoadStages()
    {
        stageCache.Clear();

        AssetBundle stages
          = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "stages"));

        foreach (string stage in stages.GetAllScenePaths())
        {
            stageCache.Add(stage);
        }
    }

    public static void LoadCharacters()
    {
        foreach (string character in Directory.GetFiles(Application.persistentDataPath + "/Characters"))
        {
            CharacterObject newCharacterObject = LoadCharacterJSON(File.ReadAllText(character));

            characterCache.Add(newCharacterObject.name, newCharacterObject);
        }
    }
    private static CharacterObject LoadCharacterJSON(string jsonFileInfo)
    {
        Debug.Log(jsonFileInfo);
        
        CharacterObject newCharacterObject = ScriptableObject.CreateInstance<CharacterObject>();
        CharacterObjectSave save = new CharacterObjectSave();
        JsonUtility.FromJsonOverwrite(jsonFileInfo, save);

        newCharacterObject.name = save.name;
        newCharacterObject.spriteSheetPath = save.spriteSheetPath;
        newCharacterObject.defaultDirection = save.defaultDirection;

        foreach (string animInfo in save.animInfos)
        {
            newCharacterObject.animInfos.Add(JsonUtility.FromJson<AnimInfo>(animInfo));
        }

        return newCharacterObject;
    }
}

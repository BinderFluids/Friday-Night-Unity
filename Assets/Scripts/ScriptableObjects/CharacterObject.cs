using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class CharacterObject : ScriptableObject
{
    public DefaultDirection defaultDirection;

    public string spriteSheetPath;
    public List<AnimInfo> animInfos = new List<AnimInfo>();

    public RuntimeCharacter runtimeCharacter; 

    public enum DefaultDirection {
        left,
        right
    }
}

public class CharacterObjectSave {
    public string name; 

    public RuntimeAnimatorController anim;
    public CharacterObject.DefaultDirection defaultDirection;


    public List<string> animInfos = new List<string>();

    public string spriteSheetPath;
}

[System.Serializable]
public class AnimInfo {
    public string name;
    public string type; 
    public int arrowDirection;

    public Vector3 offset;

    public List<string> stateNames = new List<string>();
    public List<Vector2> imagePositions = new List<Vector2>();
    public List<Vector2> maskSizes = new List<Vector2>();
    public List<Vector2> framePositions = new List<Vector2>(); 
}

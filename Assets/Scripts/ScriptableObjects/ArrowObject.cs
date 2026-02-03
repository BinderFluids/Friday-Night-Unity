using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Arrow", menuName = "Arrow")]
public class ArrowObject : ScriptableObject
{
    public List<Sprite> editorImages;
    public List<Sprite> gameImages;
    public string editorName;
}

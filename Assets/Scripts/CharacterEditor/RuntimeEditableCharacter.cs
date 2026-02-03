using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO; 

public class RuntimeEditableCharacter : MonoBehaviour
{
    private string[] hitStates = new string[] { "BF_Left", "BF_Down", "BF_Up", "BF_Right" };
    private string[] missStates = new string[] { "BF_MissLeft", "BF_MissDown", "BF_MissUp", "BF_MissRight" };
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Slider slider; 
    [SerializeField] private TMP_Text stateNameLabel;
    [SerializeField] private TMP_Text xLabel;
    [SerializeField] private TMP_Text yLabel;
    [SerializeField] private GameObject characterGameObject; 

    public AnimPlayer animPlayer; 
    public CharacterObject characterObject;
    public Animator anim;

    public int state;
    private string stateName; 

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        anim = GetComponent<Animator>();

        slider.onValueChanged.AddListener(delegate { OnSliderChanged(slider); });
    }

    private void OnSliderChanged(Slider slideThing)
    {
        state = Mathf.RoundToInt(slider.value);

        stateNameLabel.text = characterObject.animInfos[state].name;
        xLabel.text = "X offset: " + characterObject.animInfos[state].offset.x.ToString();
        yLabel.text = "Y offset: " + characterObject.animInfos[state].offset.y.ToString();

        characterGameObject.transform.position = new Vector3(startPos.x + characterObject.animInfos[state].offset.x,
            startPos.y + characterObject.animInfos[state].offset.y, 0); 

        animPlayer.Animate(characterObject, state);
    }

    public void SetValues(CharacterObject newCharacterObject)
    {
        characterObject = newCharacterObject;
        transform.position = startPos + characterObject.animInfos[0].offset;

        GetComponent<SpriteRenderer>().sprite = Cache.spriteCache[characterObject.spriteSheetPath];

        slider.value = 0;
        slider.maxValue = characterObject.animInfos.Count - 1; 

        OnSliderChanged(slider);
    }

    public void ChangeOffset(Vector3 howMuch)
    {
        characterObject.animInfos[state].offset += howMuch;
        characterGameObject.transform.position += new Vector3(howMuch.x, howMuch.y, 0); 

        stateNameLabel.text = characterObject.animInfos[state].name; 
        xLabel.text = "X offset: " + characterObject.animInfos[state].offset.x.ToString();
        yLabel.text = "Y offset: " + characterObject.animInfos[state].offset.y.ToString();
    }
}


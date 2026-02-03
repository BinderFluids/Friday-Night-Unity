using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RuntimeCharacter : MonoBehaviour
{
    public Vector3 startPos;

    public Dictionary<string, int> nameToAnim = new Dictionary<string, int>(); 
    

    public Animator anim;
    public CharacterObject characterObject;
    public float idleCooldown;
    public bool checkPressed;

    [SerializeField] private GameObject characterGameObject;
    [SerializeField] private AnimPlayer animPlayer; 

    public bool ableToIdle;

    [SerializeField] private List<Vector3> offSets = new List<Vector3>(); 

    public void SetValues()
    {
        ableToIdle = true;
        startPos = transform.position;

        characterObject.runtimeCharacter = this; 

        GetComponent<SpriteRenderer>().sprite = Cache.spriteCache[characterObject.spriteSheetPath];
        foreach(AnimInfo animInfo in characterObject.animInfos)
        {
            nameToAnim.Add(animInfo.name, characterObject.animInfos.IndexOf(animInfo));
        }

        PlayArrow(true, "idle");
    }

    public void PlayArrow(bool hit, string anim)
    {
        if (!hit)
        {
            anim = "m" + anim;  
        }

        characterGameObject.transform.position = startPos + characterObject.animInfos[nameToAnim[anim]].offset;
        animPlayer.Animate(characterObject, nameToAnim[anim]);
    }

    private IEnumerator YieldAnimation()
    {
        ableToIdle = false;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        ableToIdle = true; 
    }
}

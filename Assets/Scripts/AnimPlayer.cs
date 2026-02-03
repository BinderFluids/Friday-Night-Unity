using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPlayer : MonoBehaviour
{
    public Transform image;
    public Transform mask;
    public Transform parentObj; 

    public bool finished; 

    public void Animate(CharacterObject charObj, int state)
    {
        StopAllCoroutines(); 
        StartCoroutine(StepFrames(charObj.animInfos[state].imagePositions, 
            charObj.animInfos[state].maskSizes, charObj.animInfos[state].framePositions,
            24, charObj));
    }

    public IEnumerator StepFrames(List<Vector2> imagePositions, List<Vector2> maskSizes, List<Vector2> framePositions, int fps, CharacterObject charObj)
    {
        Vector2 startPos = parentObj.transform.position; 

        finished = false;

        int frame = 0;

        while (frame < imagePositions.Count)
        {
            parentObj.position = startPos + framePositions[frame] / 100f; 
            image.localPosition = imagePositions[frame] / 100f;
            mask.localScale = maskSizes[frame];

            yield return new WaitForSeconds(1f / fps);

            frame++;
        }

        finished = true;

        if (charObj.runtimeCharacter != null)
        {
            Animate(charObj, charObj.runtimeCharacter.nameToAnim["idle"]);
        }
        else
        {
            StepFrames(imagePositions, maskSizes, framePositions, fps, charObj);
        }
    }
}
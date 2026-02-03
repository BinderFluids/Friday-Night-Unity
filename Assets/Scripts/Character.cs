using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Character : MonoBehaviour
{
    public RuntimeAnimatorController[] hitAnims;
    public RuntimeAnimatorController[] missAnims;
    public Sprite healthIcon; 

    public Animator animator;
    public float idleCooldown;
    public bool checkPressed;
    public float cooldown;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //set idle animation when cooldown is 0
        if (idleCooldown <= 0)
        {
            idleCooldown = 0;
            checkPressed = false;
            animator.runtimeAnimatorController = hitAnims[0];
        }
    }

    public void PlayArrow(bool hit, int direction)
    {
        animator.runtimeAnimatorController = null;
        StartCoroutine("SetCooldown");
        if (hit)
        {
            animator.runtimeAnimatorController = hitAnims[direction + 1];
        }
        if (!hit)
        {
            animator.runtimeAnimatorController = missAnims[direction];
        }
    }

    private IEnumerator SetCooldown()
    {
        idleCooldown = cooldown;

        if (!checkPressed)
        {
            checkPressed = true;
            while (idleCooldown > 0)
            {
                yield return new WaitForSeconds(0.1f);
                idleCooldown -= 1;
            }
        }
    }
}

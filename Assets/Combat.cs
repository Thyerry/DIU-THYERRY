using Assets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class Combat : MonoBehaviour, IAnimatorController
{
    private Animator animator;

    #region Animator Controllers
    private bool atkCombo01;
    private bool atkCombo02;
    private bool atkCombo03;
    private bool atkJump;

    #endregion
    AnimatorClipInfo[] clipInfos;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorControllerInit();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (animator.GetBool("Jumping") || Input.GetKeyDown(KeyCode.Space))
            {
                atkJump = true;
            }

            else
                atkCombo01 = true;
        }

        AnimatorControllerUpdate();
    }

    public void AnimatorControllerInit()
    {
        clipInfos = animator.GetCurrentAnimatorClipInfo(0);
        atkCombo01 = false;
        atkCombo02 = false;
        atkCombo03 = false;
        atkJump = false;
    }

    public void AnimatorControllerUpdate()
    {
        animator.SetBool("Atk_Combo_01", atkCombo01);
        animator.SetBool("Atk_Jump", atkJump);
    }
}

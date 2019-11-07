using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowcaseAnimsOstages : MonoBehaviour
{
    [Header("                  ******* Use For Debug Purpose Only ********")]
    [Space(20)]
    public Animator animator;

    [Tooltip("0: idlePanic01   5: Walk    10: Run")]
    [Range(0, 10)]
    public float speed;

    [Tooltip("-10: idlePanic03   -5: idlePanic02     0: idlePanic01    10: Opening a door")]
    [Range(0, 10)]
    public float idleType;

    [Tooltip("0: idlePanic01   5: Walk    10: Run")]
    [Range(0, 1)]
    public float Action00Value;

    [Tooltip("0: idlePanic01   5: Walk    10: Run")]
    [Range(0, 1)]
    public float Action01Value;

    [Tooltip("0: idlePanic01   5: Walk    10: Run")]
    [Range(0, 1)]
    public float Action02Value;

    [Tooltip("0: idlePanic01   5: Walk    10: Run")]
    [Range(0, 1)]
    public float Action03Value;

    [Space(20)]
    public bool idleType00ToMove;
    public bool idleType01ToMove;
    public bool idleType02ToMove;

    [Space(20)]
    public bool idleType00ToAction00;
    public bool idleType01ToAction00;
    public bool idleType02ToAction00;

    [Space(20)]
    public bool idleType00ToAction01;
    public bool idleType01ToAction01;
    public bool idleType02ToAction01;

    [Space(20)]
    public bool idleType00ToAction02;
    public bool idleType01ToAction02;
    public bool idleType02ToAction02;

    [Space(20)]
    public bool idleType00ToAction03;
    public bool idleType01ToAction03;
    public bool idleType02ToAction03;

    [Space(20)]
    public bool death;

    public void Start()
    {
        if (animator == null && GetComponent<Animator>() != null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {

        animator.SetFloat("Speed", speed);
        animator.SetFloat("IdleValue", idleType);
        animator.SetFloat("Action00_Value", Action00Value);
        animator.SetFloat("Action02_Value", Action02Value);
        animator.SetFloat("Action03_Value", Action03Value);


        animator.SetFloat("Action01_Value", Action01Value);


        animator.SetBool("Idle00_To_Move", idleType00ToMove);
        animator.SetBool("Idle01_To_Move", idleType01ToMove);
        animator.SetBool("Idle02_To_Move", idleType02ToMove);

        animator.SetBool("Idle00_To_Action00", idleType00ToAction00);
        animator.SetBool("Idle01_To_Action00", idleType01ToAction00);
        animator.SetBool("Idle02_To_Action00", idleType02ToAction00);


        animator.SetBool("Idle00_To_Action01", idleType00ToAction01);
        animator.SetBool("Idle01_To_Action01", idleType01ToAction01);
        animator.SetBool("Idle02_To_Action01", idleType02ToAction01);


        animator.SetBool("Idle00_To_Action02", idleType00ToAction02);
        animator.SetBool("Idle01_To_Action02", idleType01ToAction02);
        animator.SetBool("Idle02_To_Action02", idleType02ToAction02);


        animator.SetBool("Idle00_To_Action03", idleType00ToAction03);
        animator.SetBool("Idle01_To_Action03", idleType01ToAction03);
        animator.SetBool("Idle02_To_Action03", idleType02ToAction03);

        animator.SetBool("Death", death);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowcaseAnimsOstages : MonoBehaviour
{

    //les idles ne nécessitent aucun bool pour être modifiées

    // POUR ACTIVER UNE ANIM "ACTION" ou MOVE
    /// checker le type de l'idle
    /// passer le bool de "tonIdleActif"To"tonAction" en true
    /// Lerp le float "nomDeTonAction" de 0 à la value souhaitée (1 pour une action, 5 ou 10 pour le mouvement)

    // le cheminement inverse s'applique pour retourner à l'idle



    [Header("                           ******* HOSTAGES ********")]
    [Header("                  ******* Use For Debug Purpose Only ********")]
    [Space(20)]
    public Animator animator;

    [Tooltip("0: idlePanic01   5: Walk    10: Run")]
    [Range(0, 10)]
    public float speed;

    [Tooltip("0: idlePanic00     5: idlePanic01     10: idlePanic02")]
    [Range(0, 10)]
    public float idleType;

    [Tooltip("0: idle   1: Open door")]
    [Range(0, 1)]
    public float open;

    [Space(20)]
    public bool idleType00ToMove;
    public bool idleType01ToMove;
    public bool idleType02ToMove;

    [Space(20)]
    public bool idleType00ToOpen;
    public bool idleType01ToOpen;
    public bool idleType02ToOpen;


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
        animator.SetFloat("Action00_Value", open);

        animator.SetBool("Idle00_To_Move", idleType00ToMove);
        animator.SetBool("Idle01_To_Move", idleType01ToMove);
        animator.SetBool("Idle02_To_Move", idleType02ToMove);

        animator.SetBool("Idle00_To_Action00", idleType00ToOpen);
        animator.SetBool("Idle01_To_Action00", idleType01ToOpen);
        animator.SetBool("Idle02_To_Action00", idleType02ToOpen);

    }
}


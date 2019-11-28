using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowcaseAnimsRaider : MonoBehaviour
{
    //les idles ne nécessitent aucun bool pour être modifiées

    // POUR ACTIVER UNE ANIM "ACTION" ou MOVE
    /// checker le type de l'idle
    /// passer le bool de "tonIdleActif"To"tonAction" en true
    /// Lerp le float "nomDeTonAction" de 0 à la value souhaitée (1 pour une action, 5 ou 10 pour le mouvement)

    // le cheminement inverse s'applique pour retourner à l'idle


    [Header("                           ******* RAIDERS ********")]
    [Header("                  ******* Use For Debug Purpose Only ********")]
    [Space(20)]
    public Animator animator;

    [Tooltip("0: idlePanic01   5: Walk    10: Run")]
    [Range(0, 10)]
    public float speed;

    [Tooltip("0: idle    1: idleCarrying")]
    [Range(0, 1)]
    public float idleType;

    [Tooltip("0: idle   1: LowTrial")]
    [Range(0, 1)]
    public float lowTrial;

    [Tooltip("0: idle   1: Open")]
    [Range(0, 1)]
    public float open;

    [Tooltip("0: idle   1: Hacking")]
    [Range(0, 1)]
    public float Hacking;

    [Space(20)]
    public bool idleToMove;
    public bool carryingToMove;

    [Space(20)]
    public bool idleToLowTrial;
    public bool carryingToLowTrial;

    [Space(20)]
    public bool idleToOpen;
    public bool carryingToOpen;

    [Space(20)]
    public bool idleToHacking;
    public bool carryingToHacking;


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
        animator.SetFloat("Action00_Value", lowTrial);
        animator.SetFloat("Action01_Value", open);
        animator.SetFloat("Action02_Value", Hacking);


        animator.SetBool("Idle00_To_Move", idleToMove);
        animator.SetBool("Idle01_To_Move", carryingToMove);

        animator.SetBool("Idle00_To_Action00", idleToLowTrial);
        animator.SetBool("Idle01_To_Action00", carryingToLowTrial);

        animator.SetBool("Idle00_To_Action01", idleToOpen);
        animator.SetBool("Idle01_To_Action01", carryingToOpen);

        animator.SetBool("Idle00_To_Action02", idleToHacking);
        animator.SetBool("Idle01_To_Action02", carryingToHacking);

        animator.SetBool("Death", death);


    }
}

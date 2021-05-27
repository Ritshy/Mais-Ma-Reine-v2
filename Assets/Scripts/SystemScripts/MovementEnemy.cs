using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class MovementEnemy : MonoBehaviour
{
    private FideleManager myFideleManager;
    private AnimationManager myAnimManager;
    public Collider2D myMoveZoneCollider;

    public bool isAttackableUnitInMyZone;

    private NavMeshAgent agent;

    private Interaction targetInteraction;
    private Collider2D targetInteractionZone;

    public bool hasMoved = false;
    public bool isMoving = false;
    public bool targetLanded = false;

    public GameObject deadZonetmp;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.isStopped = true;

        myFideleManager = GetComponentInParent<FideleManager>();
        myAnimManager = GetComponentInParent<AnimationManager>();

        deadZonetmp = myAnimManager.GetComponentInChildren<DeadZone>().gameObject;
        deadZonetmp.SetActive(false);
        //myMoveZoneCollider = GetComponentInChildren<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        /*Vector3 curMove = transform.position - previousPosition;
        currentSpeed = curMove.magnitude / Time.deltaTime;
        previousPosition = transform.position;*/

        //Liste qui défile un par un pour les déplacements FAIT
        //Quand c'est le tour de ce personnage : FAIT
        //Detecter la priorité principale A FAIRE
        //Faire le chemin pour l'atteindre FAIT (obstacles à ajouter)

        //Si le personnage :
        //Quitte sa zone de déplacemenet OU Quitte la MAP OU Atteint sa destination
        //L'arrêter FAIT
    }

    public void MoveToTarget()
    {
        Transform myTarget = null;
        myTarget = myFideleManager.DetermineMyTarget();

        targetInteraction = myTarget.GetComponentInChildren<Interaction>();
        targetInteractionZone = targetInteraction.GetComponent<PolygonCollider2D>();

        StartCoroutine(DragCamera2D.Instance.FollowTargetCamera(myFideleManager.gameObject));

        if (!myFideleManager.GetComponentInChildren<Interaction>().myCollideInteractionList.Contains(targetInteraction))
        {
            isMoving = true;
            agent.isStopped = false;

            agent.SetDestination(myTarget.GetComponentInChildren<Interaction>().transform.position);
        }
        else if (myFideleManager.GetComponentInChildren<Interaction>().myCollideInteractionList.Contains(targetInteraction))
        {
            myFideleManager.UpdateAttackableUnitInRange();
            CombatManager.Instance.EnemyLaunchCombat(myFideleManager, targetInteraction.GetComponentInParent<FideleManager>());

            StopMoving();
        }

        /*if (isAttackableUnitInMyZone)
        {
        }*/
    }

    public void StopMoving()
    {
        agent.velocity = new Vector3(0, 0, 0);

        myFideleManager.myTarget = null;
        agent.isStopped = true;

        transform.parent.position = transform.position;
        transform.localPosition = Vector3.zero;

        isMoving = false;
        hasMoved = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == targetInteractionZone && myFideleManager.myCamp == GameManager.Instance.currentCampTurn)
        {
            myFideleManager.UpdateAttackableUnitInRange();
            CombatManager.Instance.EnemyLaunchCombat(myFideleManager, targetInteraction.GetComponentInParent<FideleManager>());
            StopMoving();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == myMoveZoneCollider)
        {
            myAnimManager.CheckActionsLeftAmout();
            StopMoving();

            DragCamera2D.Instance.UnfollowTargetCamera();
            GameManager.Instance.MoveUnit();
        }
    }
}

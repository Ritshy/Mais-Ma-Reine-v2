/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Interaction;

public class Raycast : MonoBehaviour
{
    //private Interaction myClickedInteraction;
    public GameObject followTarget;

    private FideleManager interactionLauncher;
    private Interaction interactionReceiver;

    public float interactionClickingTime;
    private float currentInteractionClickingTime;

    public float movemementClickingTime;
    private float currentMovementClickingTime;

    public bool isLookingForInteraction = false;
    
    private Movement myCurrentMovement;
    private Interaction myCurrentInteraction;

    private Image myMovementClickFeedback;
    private Image myInteractionClickFeedback;

    private GameManager myGM;

    // Start is called before the first frame update
    void Start()
    {
        myGM = GetComponent<GameManager>();
        currentInteractionClickingTime = interactionClickingTime;
        currentMovementClickingTime = movemementClickingTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            LookForMovement();
        }

        if (Input.GetMouseButton(0) && isLookingForInteraction == false)
        {
            LookForInteractionLauncher();
        }

        if (Input.GetMouseButton(0) && isLookingForInteraction == true)
        {
            LaunchInteraction();
        }

        if (isLookingForInteraction)
        {
            InteractionFeedback();
        }

        if (Input.GetMouseButtonUp(0) && myCurrentMovement != null)
        {
            currentMovementClickingTime = movemementClickingTime;
            myCurrentMovement.GetComponentInParent<FideleManager>().movementClickFeedback.enabled = false;
            myCurrentMovement.GetComponentInParent<FideleManager>().movementClickFeedback.fillAmount = currentMovementClickingTime;
        }

        if (Input.GetMouseButtonUp(0) && interactionLauncher != null)
        {
            currentInteractionClickingTime = interactionClickingTime;
            interactionLauncher.interactionClickFeedback.enabled = false;
            interactionLauncher.interactionClickFeedback.fillAmount = interactionClickingTime;
        }
        
    }

    void LookForMovement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        if (hit.collider != null && hit.collider.gameObject.GetComponentInChildren<Movement>() && hit.collider.gameObject.GetComponentInChildren<Movement>().isMoving == false && hit.collider.gameObject.GetComponent<FideleManager>().currentCamp.ToString() == myGM.currentCampTurn.ToString())
        {
            myCurrentMovement = hit.collider.gameObject.GetComponentInChildren<Movement>();
            currentMovementClickingTime -= Time.deltaTime;

            myCurrentMovement.GetComponentInParent<FideleManager>().movementClickFeedback.enabled = true;
            myCurrentMovement.GetComponentInParent<FideleManager>().movementClickFeedback.fillAmount = currentMovementClickingTime / movemementClickingTime;

            if (currentMovementClickingTime <= 0)
            {
                hit.collider.gameObject.GetComponent<FideleManager>().isSelectable = false;

                myCurrentMovement.MovingCharacter();
                if (myCurrentInteraction)
                {
                    myCurrentInteraction = null;
                }

                myCurrentMovement = null;
            }
        }
        else
        {
            currentMovementClickingTime = movemementClickingTime;
            myCurrentMovement.GetComponentInParent<FideleManager>().movementClickFeedback.enabled = false;
            myCurrentMovement.GetComponentInParent<FideleManager>().movementClickFeedback.fillAmount = currentMovementClickingTime;
        }
    }

    void LookForInteractionLauncher()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        if (hit.collider != null && hit.collider.gameObject.GetComponent<FideleManager>() && hit.collider.gameObject.GetComponent<FideleManager>().isSelectable && hit.collider.gameObject.GetComponent<FideleManager>().currentCamp.ToString() == myGM.currentCampTurn.ToString()) //Si le raycast touche un fidèle qui est selectionnable...
        {
            interactionLauncher = hit.collider.gameObject.GetComponent<FideleManager>(); //Ce fidèle devient l'interactionLauncher
            interactionLauncher.ToggleLauncherOutline();

            foreach (Interaction myCollideInteraction in interactionLauncher.GetComponentInChildren<Interaction>().myCollideInteractionList)
            {
                myCollideInteraction.canInteract = true;
            }
            isLookingForInteraction = true; //L'interactionLauncher cherche une interaction
        }
    }

    void LaunchInteraction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        if (hit.collider != null && hit.collider.gameObject.GetComponentInChildren<Interaction>().canInteract && interactionLauncher.isSelectable && interactionLauncher.currentCamp.ToString() != hit.collider.gameObject.GetComponentInParent<FideleManager>().currentCamp.ToString()) //Si l'élément est Interactif et que l'interactionLaucher est sélectionnable...
        {
            interactionReceiver = hit.collider.gameObject.GetComponentInChildren<Interaction>();//L'élément Interactif devient l'interactionReceiver

            if (!interactionLauncher.GetComponentInChildren<Interaction>().alreadyInteractedList.Contains(interactionReceiver))
            {
                currentInteractionClickingTime -= Time.deltaTime; //Timer de clic actif
                interactionLauncher.interactionClickFeedback.enabled = true;
                interactionLauncher.interactionClickFeedback.fillAmount = currentInteractionClickingTime / interactionClickingTime;
            }

            if (interactionReceiver.canInteract == true && currentInteractionClickingTime <= 0) //Si l'interactionReceiver est Interactif et que le timer de clic est fini...
            {
                currentInteractionClickingTime = interactionClickingTime; //Reset timer
                interactionLauncher.interactionClickFeedback.enabled = false;
                interactionLauncher.interactionClickFeedback.fillAmount = interactionClickingTime;

                switch (interactionReceiver.interactionType) //Quel type d'interaction porte l'interactionReceiver ?
                {
                    case InteractionType.Dialogue:
                        interactionReceiver.GetComponent<Dialogue>().DisplayDialogueFeedback();
                        Debug.Log("Dialogue");
                        break;
                    case InteractionType.Recrutement:
                        Debug.Log("Recrutement");
                        break;
                    case InteractionType.Combat:
                        interactionReceiver.GetComponent<Combat>().DisplayCombatFeedback();
                        Debug.Log("Combat");
                        break;
                    case InteractionType.Event:
                        Debug.Log("Event");
                        break;
                    default:
                        break;
                }

                interactionLauncher.isSelectable = false;
                interactionLauncher.GetComponentInChildren<Interaction>().myInteractionIcon.sprite = null;
                interactionLauncher.GetComponentInChildren<Interaction>().hasInteract = true;

                foreach (Interaction myCollideInteraction in interactionLauncher.GetComponentInChildren<Interaction>().myCollideInteractionList)
                {
                    myCollideInteraction.GetComponentInParent<FideleManager>().DesactivateReceiverSelection();
                    myCollideInteraction.canInteract = false; //L'élément avec lequel il peut intéragir devient non interacif
                    myCollideInteraction.myInteractionIcon.sprite = null;
                }

                //interactionLauncher.SwitchBackInteractionLightColor();
                interactionReceiver.GetComponentInParent<FideleManager>().DesactivateReceiverSelection();
                interactionLauncher.GetComponentInChildren<Interaction>().alreadyInteractedList.Add(interactionReceiver);
                //Debug.Log("On est là");
                isLookingForInteraction = false; //L'interactionLauncher ne cherche plus d'interaction

                interactionLauncher.ToggleLauncherOutline();
                interactionLauncher.DesactivateLauncherSelection();
                interactionLauncher = null;
            }
        }
        else if (hit.collider != null && interactionLauncher != null && hit.collider.gameObject.GetComponent<FideleManager>() && hit.collider.gameObject.GetComponent<FideleManager>().isSelectable)
        {
            interactionLauncher.DesactivateLauncherSelection();
            foreach (Interaction myCollideInteraction in interactionLauncher.GetComponentInChildren<Interaction>().myCollideInteractionList)
            {
                myCollideInteraction.canInteract = false;
                myCollideInteraction.GetComponentInParent<FideleManager>().DesactivateReceiverSelection();
            }

            interactionLauncher = null;

            interactionLauncher = hit.collider.gameObject.GetComponent<FideleManager>(); //Ce fidèle devient l'interactionLauncher
            interactionLauncher.ActivateLauncherSelection();
            //interactionLauncher.SwitchInteractionLightColor();

            foreach (Interaction myCollideInteraction in interactionLauncher.GetComponentInChildren<Interaction>().myCollideInteractionList)
            {
                myCollideInteraction.canInteract = true;

                if (!interactionLauncher.GetComponentInChildren<Interaction>().alreadyInteractedList.Contains(myCollideInteraction) && interactionLauncher.currentCamp.ToString() != myCollideInteraction.GetComponentInParent<FideleManager>().currentCamp.ToString())
                {
                    myCollideInteraction.GetComponentInParent<FideleManager>().ActivateReceiverSelection();
                }
            }

            isLookingForInteraction = true; //L'interactionLauncher cherche une interaction
        }
        else if (hit.collider != null && interactionLauncher != null && hit.collider.gameObject.GetComponent<FideleManager>())
        {
            interactionLauncher.DesactivateLauncherSelection();
            interactionLauncher.ToggleLauncherOutline();
            foreach (Interaction myCollideInteraction in interactionLauncher.GetComponentInChildren<Interaction>().myCollideInteractionList)
            {
                myCollideInteraction.canInteract = false;
                myCollideInteraction.GetComponentInParent<FideleManager>().DesactivateReceiverSelection();
            }

            isLookingForInteraction = false;
            interactionLauncher = null;
        }
    } 
    
    void InteractionFeedback()
    {
        //Check si la souris survole une interaction possible -> Afficher le feedback correspondant
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);


        if (hit.collider != null)
        {
            if (hit.transform.GetComponentInChildren<Interaction>() != null && hit.transform.GetComponentInChildren<Interaction>().canInteract && hit.transform.GetComponentInChildren<Interaction>() != interactionLauncher
                && hit.transform.GetComponent<FideleManager>().currentCamp != interactionLauncher.GetComponent<FideleManager>().currentCamp
                && !interactionLauncher.GetComponentInChildren<Interaction>().alreadyInteractedList.Contains(hit.transform.GetComponentInChildren<Interaction>()))
            {
                if (myCurrentInteraction == null)
                {
                    switch (hit.transform.GetComponentInChildren<Interaction>().interactionType)//Quel type d'interaction porte l'élément interactif ?
                    {
                        case InteractionType.Dialogue:
                            hit.transform.GetComponentInChildren<Interaction>().myInteractionIcon.sprite = hit.transform.GetComponentInChildren<Interaction>().dialogueIcon;
                            interactionLauncher.GetComponentInChildren<Interaction>().myInteractionIcon.sprite = hit.transform.GetComponentInChildren<Interaction>().dialogueIcon;
                            break;
                        case InteractionType.Recrutement:
                            hit.transform.GetComponentInChildren<Interaction>().myInteractionIcon.sprite = hit.transform.GetComponentInChildren<Interaction>().recrutementIcon;
                            interactionLauncher.GetComponentInChildren<Interaction>().myInteractionIcon.sprite = hit.transform.GetComponentInChildren<Interaction>().recrutementIcon;
                            break;
                        case InteractionType.Combat:
                            hit.transform.GetComponentInChildren<Interaction>().myInteractionIcon.sprite = hit.transform.GetComponentInChildren<Interaction>().combatIcon;
                            interactionLauncher.GetComponentInChildren<Interaction>().myInteractionIcon.sprite = hit.transform.GetComponentInChildren<Interaction>().combatIcon;
                            break;
                        case InteractionType.Event:
                            break;
                        default:
                            break;
                    }
                    myCurrentInteraction = hit.transform.GetComponentInChildren<Interaction>();
                }
                else if (myCurrentInteraction != null && myCurrentInteraction != hit.transform.GetComponentInChildren<Interaction>())
                {
                    //Debug.Log("changement d'interaction");

                    myCurrentInteraction.myInteractionIcon.sprite = null;
                    interactionLauncher.GetComponentInChildren<Interaction>().myInteractionIcon.sprite = null;

                    myCurrentInteraction = null;
                }
            }
        }
        else
        {
            //Debug.Log("Rien en vue");

            if (myCurrentInteraction)
            {
                myCurrentInteraction.myInteractionIcon.sprite = null;
                interactionLauncher.GetComponentInChildren<Interaction>().myInteractionIcon.sprite = null;

                myCurrentInteraction = null;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            //Debug.Log("On annule");

            if (myCurrentInteraction)
            {
                myCurrentInteraction.myInteractionIcon.sprite = null;
                interactionLauncher.GetComponentInChildren<Interaction>().myInteractionIcon.sprite = null;
                myCurrentInteraction = null;
            }

            foreach (Interaction myCollideInteraction in interactionLauncher.GetComponentInChildren<Interaction>().myCollideInteractionList)
            {
                interactionLauncher.GetComponentInChildren<Interaction>().myInteractionIcon.sprite = null;
                myCollideInteraction.myInteractionIcon.sprite = null;

                myCollideInteraction.GetComponentInParent<FideleManager>().DesactivateReceiverSelection();
                myCollideInteraction.canInteract = false;
            }

            currentInteractionClickingTime = interactionClickingTime;
            isLookingForInteraction = false;

            interactionLauncher.ToggleLauncherOutline();
            interactionLauncher.DesactivateLauncherSelection();
            interactionLauncher = null;
        }
    }
}*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastInteraction : MonoBehaviour
{
    public Interaction interactionLauncherInteraction;
    public AnimationManager interactionLauncherAnim;
    public FideleManager interactionLauncherFM;

    public Interaction interactionReceiverInteraction;
    public AnimationManager interactionReceiverAnim;

    [Header ("Sounds")]

    public AK.Wwise.Event combatLancementInteractionSFX;
    public AK.Wwise.Event dialogueLancementInteractionSFX;
    public AK.Wwise.Event recrutementLancementInteractionSFX;

    #region Singleton

    public static RaycastInteraction Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isGamePaused == false)
        {
            if (GameManager.Instance.currentCampTurn == GameCamps.Fidele && Input.GetMouseButtonDown(0))
                LookForInteractionLauncher();

            if (GameManager.Instance.currentCampTurn == GameCamps.Fidele && Input.GetMouseButtonDown(0) && interactionLauncherAnim != null)
                LookForInteractionReceiver();
        }
    }

    public void LookForInteractionLauncher()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        if (interactionLauncherAnim == null && interactionLauncherInteraction == null)
        {
            if (hit.collider != null && hit.collider.gameObject.GetComponent<AnimationManager>() && hit.collider.gameObject.GetComponent<AnimationManager>().isSelectable && hit.collider.gameObject.GetComponent<FideleManager>().myCamp == GameCamps.Fidele)
            {
                SetFideleSelectedInteractionLauncher(hit.collider.gameObject.GetComponent<FideleManager>());
            }
        }
        else
        {
            if (hit.collider != null && hit.collider.gameObject.GetComponent<AnimationManager>() && hit.collider.gameObject.GetComponent<AnimationManager>().isSelectable && hit.collider.gameObject.GetComponent<FideleManager>().myCamp == GameCamps.Fidele)
            {
                interactionLauncherAnim.keepInteractionDisplayed = false;
                interactionLauncherAnim.HideInteraction();

                foreach (Interaction myCollideInteraction in interactionLauncherInteraction.myCollideInteractionList)
                {
                    myCollideInteraction.canInteract = false;
                    myCollideInteraction.GetComponentInParent<AnimationManager>().DesactivateReceiverSelection();
                    myCollideInteraction.GetComponentInParent<AnimationManager>().keepInteractionDisplayed = false;
                    myCollideInteraction.GetComponentInParent<AnimationManager>().HideInteraction();
                }

                ResetReceiverInteraction();
                ResetLauncherInteraction();

                SetFideleSelectedInteractionLauncher(hit.collider.gameObject.GetComponent<FideleManager>());
            }
        }
    }

    public void LookForInteractionReceiver()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        if (hit.collider != null && hit.collider.gameObject.GetComponentInChildren<Interaction>() && hit.collider.gameObject.GetComponentInChildren<Interaction>().canInteract 
            && hit.collider.gameObject.GetComponent<FideleManager>().myCamp != GameCamps.Fidele && interactionLauncherInteraction.myCollideInteractionList.Contains(hit.collider.gameObject.GetComponentInChildren<Interaction>()) 
            && !interactionLauncherInteraction.alreadyInteractedList.Contains(hit.collider.gameObject.GetComponentInChildren<Interaction>()))
        {
            if (CombatManager.Instance.isInFight == false && RecrutementManager.Instance.isRecruiting == false && DialogueManager.Instance.isInDialogue == false)
            {
                interactionReceiverAnim = hit.collider.GetComponent<AnimationManager>();
                interactionReceiverInteraction = hit.collider.GetComponentInChildren<Interaction>();
                FideleManager interactionReceiverFM = hit.collider.GetComponentInParent<FideleManager>();


                switch (interactionReceiverInteraction.interactionType) //Quel type d'interaction porte l'interactionReceiver ?
                {
                    case InteractionType.Dialogue:
                        //interactionLauncherInteraction.alreadyInteractedList.Add(interactionReceiverInteraction);
                        interactionReceiverInteraction.GetComponent<DialogueInteraction>().StartDialogue(interactionReceiverFM);
                        dialogueLancementInteractionSFX.Post(gameObject);
                        //Debug.Log("Dialogue");
                        break;
                    case InteractionType.Recrutement:
                        interactionLauncherInteraction.alreadyInteractedList.Add(interactionReceiverInteraction);
                        interactionReceiverInteraction.GetComponent<Recrutement>().LaunchRecruitement(interactionReceiverFM, interactionLauncherFM);
                        recrutementLancementInteractionSFX.Post(gameObject);
                        //Debug.Log("Recrutement");
                        break;
                    case InteractionType.Combat:
                        interactionLauncherInteraction.alreadyInteractedList.Add(interactionReceiverInteraction);
                        CombatManager.Instance.OpenCombatWindow(interactionLauncherFM, interactionReceiverFM);
                        combatLancementInteractionSFX.Post(gameObject);
                        //Debug.Log("Combat");
                        break;
                    default:
                        break;
                }

                interactionLauncherInteraction.CheckForAvaibleInteractions();

                interactionReceiverInteraction.OtherCampDisplayInteractionFeedbacks();
                interactionLauncherInteraction.FideleDisplayInteractionFeedbacks();

                Debug.Log("Fin interaction : " + interactionLauncherFM.name);

                ResetReceiverInteraction();
            }
        }
        else if (hit.collider == null || !hit.collider.GetComponentInChildren<Interaction>())
        {
            interactionLauncherAnim.keepInteractionDisplayed = false;
            interactionLauncherAnim.HideInteraction();

            foreach (Interaction myCollideInteraction in interactionLauncherInteraction.myCollideInteractionList)
            {
                myCollideInteraction.canInteract = false;
                myCollideInteraction.GetComponentInParent<AnimationManager>().DesactivateReceiverSelection();
                myCollideInteraction.GetComponentInParent<AnimationManager>().keepInteractionDisplayed = false;
                myCollideInteraction.GetComponentInParent<AnimationManager>().HideInteraction();
            }

            ResetLauncherInteraction();
        }
    }

    public void ResetLauncherInteraction()
    {
        if (interactionLauncherAnim != null)
        {
            interactionLauncherAnim.SetOutlineDefault();
            interactionLauncherAnim.keepInteractionDisplayed = false;
            interactionLauncherAnim.HideInteraction();

            foreach (AnimationManager cam in interactionLauncherInteraction.myCollideAnimationManagerList)
            {
                cam.keepInteractionDisplayed = false;
            }

            interactionLauncherInteraction.FideleDisplayInteractionFeedbacks();

            interactionLauncherAnim.isSelected = false;

            interactionLauncherAnim = null;
            interactionLauncherInteraction = null;

        }
    }

    public void ResetReceiverInteraction()
    {
        if (interactionReceiverAnim != null)
        {
            interactionReceiverInteraction.FideleDisplayInteractionFeedbacks();

            interactionReceiverInteraction = null;
            interactionReceiverAnim = null;
        }
    }

    public void SetFideleSelectedInteractionLauncher(FideleManager ilf)
    {
        if (ilf.GetComponent<AnimationManager>().isSelectable)
        {
            interactionLauncherAnim = ilf.GetComponent<AnimationManager>();
            interactionLauncherInteraction = ilf.GetComponentInChildren<Interaction>();
            interactionLauncherFM = ilf;

            interactionLauncherAnim.SetOutlineSelected();
            interactionLauncherAnim.keepInteractionDisplayed = true;
            interactionLauncherAnim.DisplayInteraction();

            interactionLauncherAnim.isSelected = true;

            foreach (Interaction myCollideInteraction in interactionLauncherInteraction.myCollideInteractionList)
            {
                if (!interactionLauncherInteraction.alreadyInteractedList.Contains(myCollideInteraction))
                {
                    myCollideInteraction.canInteract = true;
                    myCollideInteraction.GetComponentInParent<AnimationManager>().ActivateReceiverSelection();
                    myCollideInteraction.GetComponentInParent<AnimationManager>().keepInteractionDisplayed = true;
                    myCollideInteraction.GetComponentInParent<AnimationManager>().DisplayInteraction();
                }
            }
        }
    }

    public void CheckInteractionLauncherState()
    {
        if (interactionLauncherFM != null)
        {
            if (interactionLauncherInteraction.myCollideInteractionList.Count > 0)
            {
                for (int i = 0; i < interactionLauncherInteraction.myCollideInteractionList.Count; i++)
                {
                    if (interactionLauncherInteraction.myCollideInteractionList[i].canInteract)
                    {
                        return;
                    }
                    else
                    {
                        foreach (Interaction myCollideInteraction in interactionLauncherInteraction.myCollideInteractionList)
                        {
                            myCollideInteraction.GetComponentInParent<AnimationManager>().DesactivateReceiverSelection();
                        }
                        ResetLauncherInteraction();
                    }
                }
            }
            else
            {
                foreach (Interaction myCollideInteraction in interactionLauncherInteraction.myCollideInteractionList)
                {
                    myCollideInteraction.GetComponentInParent<AnimationManager>().DesactivateReceiverSelection();
                }
                ResetLauncherInteraction();
            }
        }
    }
}
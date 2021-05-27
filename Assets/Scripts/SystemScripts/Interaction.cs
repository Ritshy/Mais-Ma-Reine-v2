using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionType {Aucun ,Dialogue, Recrutement, Combat}

public class Interaction : MonoBehaviour
{
    public InteractionType interactionType;

    [Space]
    [Header("Dev only")]
    public float interactionTimer;
    public bool canInteract = false;
    public bool hasInteract = false;
    

    public List <Interaction> myCollideInteractionList = new List<Interaction>();
    public List <AnimationManager> myCollideAnimationManagerList = new List<AnimationManager>();

    public List<Interaction> alreadyInteractedList = new List<Interaction>();

    private AnimationManager myAnimationManager;
    public FideleManager myFideleManager;

    public Sprite combatIcon;
    public Sprite dialogueIcon;
    public Sprite recrutementIcon;

    public SpriteRenderer myInteractionIcon;

    private void Awake()
    {
        myAnimationManager = GetComponentInParent<AnimationManager>();
        myFideleManager = GetComponentInParent<FideleManager>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (myCollideInteractionList.Count == 0)
        {
            myAnimationManager.isSelectable = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Interaction>() != null && collision.GetComponentInParent<FideleManager>().myCamp != myFideleManager.myCamp)
        {
            myCollideInteractionList.Add(collision.GetComponent<Interaction>());
            myCollideAnimationManagerList.Add(collision.GetComponentInParent<AnimationManager>());

            if (myFideleManager.myCamp == GameCamps.Fidele)
            {
                FideleDisplayInteractionFeedbacks();
                CheckForAvaibleInteractions();
                collision.GetComponentInParent<AnimationManager>().ActivateReceiverSelection();
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Interaction>() != null)
        {
            Interaction myExitingCollision = collision.GetComponent<Interaction>();

            RemoveCollidingCharacterFromInteractionList(myExitingCollision);

            AnimationManager myExitingAM = collision.GetComponentInParent<AnimationManager>();

            RemoveCollidingCharacterFromAMList(myExitingAM);

            FideleDisplayInteractionFeedbacks();
        }
    }

    public void FideleDisplayInteractionFeedbacks()
    {
        if (myCollideAnimationManagerList.Count >= 1)
        {
            foreach (AnimationManager cam in myCollideAnimationManagerList)
            {
                if (cam.GetComponent<FideleManager>().myCamp != GameCamps.Fidele && myFideleManager.myCamp == GameCamps.Fidele)
                {
                    cam.haveAnInteraction = true;
                    //myCollideAnimationManager.DisplayInteraction();
                    //myAnimationManager.DisplayInteraction();

                    if (!alreadyInteractedList.Contains(cam.GetComponentInChildren<Interaction>()))
                    {
                        myAnimationManager.isSelectable = true;
                        cam.DisplayInteractionIcon();
                    }
                }
            }
        }
    }

    public void OtherCampDisplayInteractionFeedbacks()
    {
        if (myCollideAnimationManagerList.Count >= 1)
        {
            for (int i = 0; i < myCollideAnimationManagerList.Count; i++)
            {
                if (myCollideAnimationManagerList[i].GetComponent<FideleManager>().myCamp == GameCamps.Fidele && myFideleManager.myCamp != GameCamps.Fidele && !myCollideAnimationManagerList[i].GetComponentInChildren<Interaction>().alreadyInteractedList.Contains(this))
                {
                    myAnimationManager.DisplayInteractionIcon();
                    return;
                }
                myAnimationManager.HideInteractionIcon();
            }
        }
        else
        {
            myAnimationManager.HideInteractionIcon();
        }
    }

    public void CheckForAvaibleInteractions()
    {
        if (myFideleManager.myCamp == GameCamps.Fidele)
        {
            if (myFideleManager.GetComponentInChildren<Movement>().hasMoved)
            {
                if (myCollideInteractionList.Count >= 1)
                {
                    for (int i = 0; i < myCollideInteractionList.Count; i++)
                    {
                        if (!alreadyInteractedList.Contains(myCollideInteractionList[i]))
                        {
                            myAnimationManager.InteractionAvaibleColor();
                            myAnimationManager.isSelectable = true;
                            return;
                        }
                        myAnimationManager.NoMoreInteractionColor();
                        myAnimationManager.isSelectable = false;
                    }
                }
                else if (myCollideInteractionList.Count == 0)
                {
                    myAnimationManager.NoMoreInteractionColor();
                    myAnimationManager.isSelectable = false;
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }

    public void AnimationManagerUpdateIcon()
    {
        myAnimationManager.UpdateInteractionIcon();
    }

    public void RemoveCollidingCharacterFromAMList(AnimationManager aMToRemove)
    {
        if (myCollideAnimationManagerList.Count >= 1)
        {
            aMToRemove.haveAnInteraction = false;
            aMToRemove.HideInteractionIcon();

            if (aMToRemove != myAnimationManager && aMToRemove.GetComponent<FideleManager>().myCamp != myFideleManager.myCamp && myFideleManager.myCamp == GameManager.Instance.currentCampTurn)
            {
                aMToRemove.HideInteraction();
                aMToRemove.DesactivateReceiverSelection();

                myAnimationManager.HideInteraction();
            }
        }

        myCollideAnimationManagerList.Remove(aMToRemove);
        FideleDisplayInteractionFeedbacks();
    }

    public void RemoveCollidingCharacterFromInteractionList(Interaction interactionToRemove)
    {
        if (myCollideInteractionList.Count >= 1)
        {
            interactionToRemove.canInteract = false;
            interactionToRemove.myInteractionIcon.sprite = null;

            myAnimationManager.isSelectable = false;
            myInteractionIcon.sprite = null;
        }

        myCollideInteractionList.Remove(interactionToRemove);
        FideleDisplayInteractionFeedbacks();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class AnimationManager : MonoBehaviour
{
    public bool isSelectable = false;
    [HideInInspector]
    public bool isSelected = false;

    private FideleManager myFM;

    [HideInInspector]
    public bool isStatsDisplayed = false;
    [HideInInspector]
    public bool keepInteractionDisplayed = false;
    [HideInInspector]
    public bool isMovementDisplayed = false;

    
    public bool isInfoDisplayed = false;

    
    public bool haveAnInteraction = false;
    [HideInInspector]
    public bool movementZoneIsTouchingInteraction = false;

    private Animator myAnim;
    private Movement myMovement;
    private MovementEnemy myMovementEnemy;
    private Light2D myLight;
    private Interaction myInteraction;

    public Canvas myStatsCanvas;
    public TextMeshProUGUI currentHpOnCanvas;
    public TextMeshProUGUI currentAttackRangeOnCanvas;

    public Image healthAmountImage;

    public SpriteRenderer myMovementZone;

    public SpriteRenderer myOutline;

    // Start is called before the first frame update
    void Start()
    {
        UpdateMyReferences();
    }

    // Update is called once per frame
    void Update()
    {
        currentHpOnCanvas.text = myFM.currentHP.ToString();
        if (myFM.myCamp == GameCamps.Fidele)
        {
            currentAttackRangeOnCanvas.text = (myFM.minAttackRange.ToString() + " - " + myFM.maxAttackRange.ToString());
        }
        else
        {
            currentAttackRangeOnCanvas.text = ("??");
        }

        if (myMovement != null)
        {
            if (myMovement.hasMoved == false)
            {
                ActivateLauncherSelection();
            }
            else
            {
                DesactivateLauncherSelection();
            }
        }
    }

    public void UpdateMyReferences()
    {
        myAnim = GetComponent<Animator>();
        myFM = GetComponent<FideleManager>();
        myLight = GetComponentInChildren<Light2D>();
        myInteraction = GetComponentInChildren<Interaction>();

        if (GetComponentInChildren<Movement>() != null)
        {
            myMovement = GetComponentInChildren<Movement>();
        }
        else if (GetComponentInChildren<MovementEnemy>() != null)
        {
            myMovementEnemy = GetComponentInChildren<MovementEnemy>();
        }
    }

    /*public void AbleToPlay()
    {
        myAnim.SetBool("isCharacterAblePlay", true);
    }

    public void UnableToPlay()
    {
        myAnim.SetBool("isCharacterAblePlay", false);
    }*/

    public void ActivateCursorIndicator()
    {
        myAnim.SetBool("isTutoCursorVisible", true);
    }

    public void DesactivateCursorIndicator()
    {
        myAnim.SetBool("isTutoCursorVisible", false);
    }

    public void ReceiveDamage()
    {
        myAnim.SetTrigger("CharacterReceiveDamage");
    }

    public void Dying()
    {
        myAnim.SetBool("isCharacterDead", true);
    }

    public void ActivateLauncherSelection()
    {
        myAnim.SetBool("isInteractionLauncherSelected", true);
    }

    public void DesactivateLauncherSelection()
    {
        myAnim.SetBool("isInteractionLauncherSelected", false);
    }

    public void ActivateReceiverSelection()
    {
        myAnim.SetBool("isInteractionReceiverCanInteract", true);
    }

    public void DesactivateReceiverSelection()
    {
        myAnim.SetBool("isInteractionReceiverCanInteract", false);
    }

    #region Outline
    public void SetOutlineSelected()
    {
        myFM.currentFideleSprite.material.SetColor("_Color", Color.white);
    }

    public void SetOutlineDefault()
    {
        myFM.currentFideleSprite.material.SetColor("_Color", Color.gray);
    }
    #endregion

    #region Stats

    public void DisplayStats()
    {
        if (isStatsDisplayed == false)
        {
            myStatsCanvas.enabled = true;
            isStatsDisplayed = true;
        }
    }

    public void HideStats()
    {
        if (isStatsDisplayed)
        {
            myStatsCanvas.enabled = false;
            isStatsDisplayed = false;
        }
    }
    #endregion

    #region MovementZone
    public void UnableToLand()
    {
        myMovementZone.color = Color.red;
    }

    public void AbleToLand()
    {
        myMovementZone.color = Color.white;
    }

    public void DisplayMovement()
    {
        if (isMovementDisplayed == false)
        {
            myAnim.SetBool("ActivateMovementBool", true);
            isMovementDisplayed = true;
        }
    }

    public void HideMovement()
    {
        if (isMovementDisplayed)
        {
            myAnim.SetBool("ActivateMovementBool", false);
            isMovementDisplayed = false;
        }
    }
    #endregion

    #region Interaction
    public void DisplayInteraction()
    {
        myAnim.SetBool("ActivateInteractionBool", true);
        //keepInteractionDisplayed = true;
    }

    public void HideInteraction()
    {
        if (keepInteractionDisplayed == false)
        {
            myAnim.SetBool("ActivateInteractionBool", false);
            //keepInteractionDisplayed = false;
        }
    }

    public void UpdateInteractionIcon()
    {
        switch (myInteraction.interactionType)
        {
            case InteractionType.Dialogue:
                myInteraction.myInteractionIcon.sprite = myInteraction.dialogueIcon;
                break;
            case InteractionType.Recrutement:
                myInteraction.myInteractionIcon.sprite = myInteraction.recrutementIcon;
                break;
            case InteractionType.Combat:
                myInteraction.myInteractionIcon.sprite = myInteraction.combatIcon;
                break;
            default:
                break;
        }
    }

    public void DisplayInteractionIcon()
    {
        myInteraction.myInteractionIcon.enabled = true;
        UpdateInteractionIcon();
    }

    public void HideInteractionIcon()
    {
        myInteraction.myInteractionIcon.enabled = false;

        myInteraction.myInteractionIcon.sprite = null;
    }

    public void NoMoreInteractionColor()
    {
        myFM.currentFideleSprite.color = new Color(0.5f, 0.5f, 0.5f, 1f);
    }

    public void InteractionAvaibleColor()
    {
        myFM.currentFideleSprite.color = new Color(1f, 1f, 1f, 1f);
    }
    #endregion

    public void FillAmountHealth()
    {
        healthAmountImage.fillAmount = myFM.currentHP*1f / myFM.maxHp*1f;
    }

    public void LowerOpacity()
    {
        myAnim.SetBool("SetOpacity", true);
        /*Color tmp = myFM.fideleSprite.color;
        tmp.a = 0.2f;
        myFM.fideleSprite.color = tmp;*/
    }

    public void ResetOpacity()
    {
        myAnim.SetBool("SetOpacity", false);
        /*Color tmp = myFM.fideleSprite.color;
        tmp.a = 1f;
        myFM.fideleSprite.color = tmp;*/
    }

    public void CheckActionsLeftAmout()
    {
        if (GameManager.Instance.currentCampTurn == myFM.myCamp)
        {
            if (myFM.myCamp == GameCamps.Fidele)
            {
                if (myMovement.hasMoved)
                {
                    if (myInteraction.myCollideInteractionList.Count == 0)
                    {
                        myFM.isAllActionsDone = true;
                        GameManager.Instance.IsAllCampActionsDone();
                    }
                    else if (myInteraction.alreadyInteractedList.Count >= myInteraction.myCollideInteractionList.Count)
                    {
                        myFM.isAllActionsDone = true;
                        GameManager.Instance.IsAllCampActionsDone();
                    }
                    else
                    {
                        InteractionAvaibleColor();
                    }
                }
            }
            else
            {
                myFM.isAllActionsDone = true;
                GameManager.Instance.IsAllCampActionsDone();
            }
        }
    }
}

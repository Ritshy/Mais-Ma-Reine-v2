using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Runtime.InteropServices;

public class MouseEventsFidele : MonoBehaviour
{
    private FideleManager myFideleManager;
    private AnimationManager myAnimManager;

    private Interaction myInteraction;
    float tmpTimer = 0;

    #region Movement

    private Movement myMovement;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        myFideleManager = GetComponent<FideleManager>();
        myAnimManager = GetComponent<AnimationManager>();

        myInteraction = GetComponentInChildren<Interaction>();

        myMovement = GetComponentInChildren<Movement>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseEnter()
    {
        if (GameManager.Instance.isGamePaused == false)
        {
            if (myMovement.isMoving == false)
            {
                myAnimManager.DisplayInteraction();
                foreach (AnimationManager am in myInteraction.myCollideAnimationManagerList)
                {
                    am.DisplayInteraction();
                }
            }
        }
    }

    public void OnMouseDown()
    {
        if (GameManager.Instance.isGamePaused == false)
        {
            RaycastInteraction.Instance.ResetLauncherInteraction();
        }
    }

    public void OnMouseDrag()
    {
        if (GameManager.Instance.isGamePaused == false && myFideleManager.myCamp == GameCamps.Fidele)
        {
            tmpTimer += Time.deltaTime;

            if (tmpTimer > 0.3)
            {
                #region Movement

                //myAnimManager.isSelectable = false;
                RaycastInteraction.Instance.ResetLauncherInteraction();
                RaycastInteraction.Instance.ResetReceiverInteraction();

                myMovement.MovingCharacter();

                #endregion
            }
            else if (tmpTimer < 0.3)
            {
                RaycastInteraction.Instance.SetFideleSelectedInteractionLauncher(myFideleManager);
            }
        }
    }

    public void OnMouseOver()
    {
        if (GameManager.Instance.isGamePaused == false)
        {
            #region InformationDisplaying

            if (Input.GetMouseButtonDown(2) && myAnimManager.isInfoDisplayed == false)
            {
                myAnimManager.DisplayMovement();
                myAnimManager.DisplayStats();
                myAnimManager.isInfoDisplayed = true;
            }
            else if (Input.GetMouseButtonDown(2) && myAnimManager.isInfoDisplayed)
            {
                myAnimManager.HideMovement();
                myAnimManager.HideStats();
                myAnimManager.isInfoDisplayed = false;
            }

            #endregion
        }
    }

    public void OnMouseExit()
    {
        if (GameManager.Instance.isGamePaused == false)
        {
            #region InformationHiding

            /*if (myAnimManager.isInfoDisplayed && myMovement.isMoving == false)
            {
                myAnimManager.HideMovement();
                myAnimManager.HideStats();
                myAnimManager.isInfoDisplayed = false;
            }

            if (myMovement.isMoving == false && myInteraction.myCollideInteractionList.Count == 0)
            {
                myAnimManager.HideInteraction();
                myAnimManager.HideStats();
            }*/

            if (myMovement.isMoving == false)
            {
                myAnimManager.HideInteraction();
                foreach (AnimationManager am in myInteraction.myCollideAnimationManagerList)
                {
                    am.HideInteraction();
                }
            }
            #endregion
        }
    }

    public void OnMouseUp()
    {
        tmpTimer = 0;
    }
}
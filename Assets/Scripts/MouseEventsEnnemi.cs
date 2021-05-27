using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEventsEnnemi : MonoBehaviour
{
    private FideleManager myFideleManager;
    private AnimationManager myAnimManager;

    private Interaction myInteraction;

    #region Movement

    private MovementEnemy myMovement;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        myFideleManager = GetComponent<FideleManager>();
        myAnimManager = GetComponent<AnimationManager>();

        myInteraction = GetComponentInChildren<Interaction>();

        myMovement = GetComponentInChildren<MovementEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(myInteraction.canInteract + gameObject.name);

    }

    public void OnMouseEnter()
    {
        /*if (GameManager.Instance.isGamePaused == false)
        {
            myAnimManager.DisplayInteraction();
        }*/
    }

    public void OnMouseUp()
    {

    }

    public void OnMouseOver()
    {
        /*if (GameManager.Instance.isGamePaused == false)
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
        }*/
        /*if (myInteraction.canInteract == true)
        {
            myAnimManager.DisplayInteractionIcon();
        }*/
    }

    public void OnMouseExit()
    {
        /*if (GameManager.Instance.isGamePaused == false)
        {
            #region InformationHiding

            if (myAnimManager.isInfoDisplayed && myMovement.isMoving == false)
            {
                myAnimManager.HideMovement();
                myAnimManager.HideStats();
                myAnimManager.isInfoDisplayed = false;
            }

            if (myMovement.isMoving == false && myInteraction.myCollideInteractionList.Count == 0)
            {
                myAnimManager.HideInteraction();
                myAnimManager.HideStats();
            }
            #endregion
        }*/
        /*if (myInteraction.canInteract == true)
        {
            myAnimManager.HideInteractionIcon();
        }*/
    }
}

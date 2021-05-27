using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementZoneDetectionFidele : MonoBehaviour
{
    private FideleManager myFideleManager;
    private Movement myMovement;

    // Start is called before the first frame update
    void Start()
    {
        myFideleManager = GetComponentInParent<FideleManager>();
        myMovement = myFideleManager.GetComponentInChildren<Movement>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*public void OnTriggerStay2D(Collider2D collision)
    {
        FideleManager tmpFM = collision.GetComponentInParent<FideleManager>();

        if (tmpFM)
        {
            if (myMovement.isMoving && myFideleManager.myCamp != tmpFM.myCamp)
            {
                tmpFM.GetComponent<AnimationManager>().DisplayInteraction();
            }
            else if (myMovement.isMoving == false)
            {
                tmpFM.GetComponent<AnimationManager>().HideInteraction();
            }
        }
    }*/

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Interaction tmpIa = collision.GetComponent<Interaction>();
        if (tmpIa)
        {
            FideleManager tmpFM = collision.GetComponentInParent<FideleManager>();

            if (tmpFM == myFideleManager || tmpFM.myCamp == myFideleManager.myCamp)
            {
                return;
            }
            myFideleManager.AddUnitInRange(tmpFM);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        myFideleManager.RemoveUnitInRange(collision);
        /*if (collision.GetComponentInParent<AnimationManager>())
        {
            collision.GetComponentInParent<AnimationManager>().HideInteraction();
        }*/
    }
}

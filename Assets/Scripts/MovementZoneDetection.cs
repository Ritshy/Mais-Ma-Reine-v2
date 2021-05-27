using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementZoneDetection : MonoBehaviour
{
    private FideleManager myFideleManager;
    private Movement myMovement;

    // Start is called before the first frame update
    void Start()
    {
        myFideleManager = GetComponentInParent<FideleManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
            
            myFideleManager.UpdateAttackableUnitInRange();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        myFideleManager.RemoveUnitInRange(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Interaction>())
        {
            FideleManager tmpFM = collision.GetComponentInParent<FideleManager>();
            if (tmpFM != myFideleManager && !myFideleManager.unitsInRange.Contains(tmpFM))
            {
                myFideleManager.AddUnitInRange(tmpFM);
                myFideleManager.UpdateAttackableUnitInRange();
            }
        }
    }
}
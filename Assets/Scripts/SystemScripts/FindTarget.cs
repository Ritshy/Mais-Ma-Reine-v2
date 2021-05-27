using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindTarget : MonoBehaviour
{
    /*private FideleManager myFideleManager;
    private MovementEnemy myMovementEnemy;

    public Transform myTarget;

    public List<FideleManager> myDistanceList = new List<FideleManager>();
    public List<FideleManager> myHPList = new List<FideleManager>();
    private float myDistance;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*public void DistanceBetweenCharacters()
    {
        myDistanceList.Clear();
        
        for (int i = 0; i < GameManager.Instance.myFideleList.Count; i++)
        {
            myDistanceList.Add(GameManager.Instance.myFideleList[i]);
            //myDistance = Vector2.Distance(gameObject.transform.position, GameManager.Instance.myFideleList[i].transform.position);
            //myDistanceList.Add(myDistance);
            //myDistance = Mathf.Min(myDistanceList.ToArray());
        }
        for (int i = 0; i < myDistanceList.Count; i++)
        {
            myDistanceList.OrderBy(t => Vector2.Distance(transform.position, myDistanceList[i].transform.position)).ToList();
        }
    }

    public void LowestHP()
    {
        myHPList.AddRange(GameManager.Instance.myFideleList);

        for (int i = 0; i < myHPList.Count; i++)
        {
            myHPList.OrderBy(t => t.currentHP);
        }
    }*/
}

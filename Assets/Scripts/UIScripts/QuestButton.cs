using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestButton : MonoBehaviour
{
    public GameObject questFocusPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CameraFocusOnObjective()
    {
        StartCoroutine(DragCamera2D.Instance.FollowTargetCamera(questFocusPosition));
    }
}

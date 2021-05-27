using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPause : MonoBehaviour
{
    private Animator myAnim;
    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseValidationWindow()
    {
        myAnim.SetTrigger("triggerValidationWindow");
    }

    public void OpenValidationWindow()
    {
        myAnim.SetTrigger("triggerValidationWindow");
    }
}

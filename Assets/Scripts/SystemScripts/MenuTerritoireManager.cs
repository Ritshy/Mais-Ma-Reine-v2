using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuTerritoireManager : MonoBehaviour
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

    public void StartTransitionToTerritoire01()
    {
        StartCoroutine(DisplayTransitionTerritoire01());
    }

    public void StartTransitionToTerritoire02()
    {
        StartCoroutine(DisplayTransitionTerritoire02());
    }

    public IEnumerator DisplayTransitionTerritoire01()
    {
        myAnim.SetTrigger("triggerTransition");

        yield return new WaitForSeconds(.5f);

        SceneSwitcher.Instance.SwitchToTerritoire01Cinematique();
    }

    public IEnumerator DisplayTransitionTerritoire02()
    {
        myAnim.SetTrigger("triggerTransition");

        yield return new WaitForSeconds(.5f);

        SceneSwitcher.Instance.SwitchToTerritoire02Cinematique();
    }
}

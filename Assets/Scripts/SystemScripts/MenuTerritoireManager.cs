using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuTerritoireManager : MonoBehaviour
{
    private Animator myAnim;

    [Header("Territoire01")]

    public GameObject myTerritoire01Disponible;
    public GameObject myTerritoire01Conquis;

    [Header("Territoire01")]

    public GameObject myTerritoire02Disponible;
    public GameObject myTerritoire02Verrouille;
    public GameObject myTerritoire02Conquis;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();

        if (GameManager.isTerritoire01Completed)
        {
            myTerritoire01Disponible.SetActive(false);
            myTerritoire01Conquis.SetActive(true);

            myTerritoire02Verrouille.SetActive(false);
            myTerritoire02Disponible.SetActive(true);
            myTerritoire02Conquis.SetActive(false);

            Debug.Log("Territoire02 accessible !");
        }
        else
        {
            myTerritoire01Disponible.SetActive(true);
            myTerritoire01Conquis.SetActive(false);

            myTerritoire02Verrouille.SetActive(true);
            myTerritoire02Disponible.SetActive(false);
            myTerritoire02Conquis.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTransitionToTerritoire01()
    {
        if (GameManager.isTerritoire01Completed == false)
        {
            StartCoroutine(DisplayTransitionTerritoire01());
        }
    }

    public void StartTransitionToTerritoire02()
    {
        if (GameManager.isTerritoire01Completed)
        {
            StartCoroutine(DisplayTransitionTerritoire02());
        }
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

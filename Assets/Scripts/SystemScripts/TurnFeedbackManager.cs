using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnFeedbackManager : MonoBehaviour
{
    public TextMeshProUGUI mySwitchTurnText;
    public TextMeshProUGUI myQuestText;

    private Animator myAnim;

    public Image bandeauBG;
    public Image boutonTour;

    [Header ("Reine")]
    public Sprite boutonTourReine;
    public Sprite bandeauBGReine;

    [Header("Roi")]
    public Sprite boutonTourRoi;
    public Sprite bandeauBGRoi;

    [Header("Bandits")]
    public Sprite boutonTourBandit;
    public Sprite bandeauBGBandit;

    [Header("Calamité")]
    public Sprite boutonTourCalamite;
    public Sprite bandeauBGCalamite;

    public bool isFirstTurnPassed;

    #region Singleton
    public static TurnFeedbackManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
        #endregion
        // Start is called before the first frame update
        void Start()
        {
            myAnim = GetComponent<Animator>();
        }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchTurnFeedback(GameCamps newCamp)
    {
        switch (newCamp)
        {
            case GameCamps.Fidele:
                myAnim.SetTrigger("Displaying");
                mySwitchTurnText.text = "La Reine joue";
                myQuestText.enabled = true;

                bandeauBG.sprite = bandeauBGReine;
                boutonTour.sprite = boutonTourReine;

                StartCoroutine(ResetFeedback());
                break;
            case GameCamps.Roi:
                myAnim.SetTrigger("Displaying");
                mySwitchTurnText.text = "Le Roi joue";
                myQuestText.enabled = false;

                bandeauBG.sprite = bandeauBGRoi;
                boutonTour.sprite = boutonTourRoi;

                StartCoroutine(ResetFeedback());
                break;
            case GameCamps.Bandit:
                myAnim.SetTrigger("Displaying");
                mySwitchTurnText.text = "Les Bandits jouent";
                myQuestText.enabled = false;

                bandeauBG.sprite = bandeauBGBandit;
                boutonTour.sprite = boutonTourBandit;

                StartCoroutine(ResetFeedback());
                break;
            case GameCamps.BanditCalamiteux:
                myAnim.SetTrigger("Displaying");
                mySwitchTurnText.text = "Les Bandits jouent";
                myQuestText.enabled = false;

                bandeauBG.sprite = bandeauBGBandit;
                boutonTour.sprite = boutonTourBandit;

                StartCoroutine(ResetFeedback());
                break;
            case GameCamps.Calamite:
                break;
            case GameCamps.Villageois:
                break;
            case GameCamps.Converti:
                break;
            default:
                break;
        }
    }

    public IEnumerator ResetFeedback()
    {
        yield return new WaitForSeconds(1f);
        mySwitchTurnText.text = "";
    }

    public void TriggerCursorIndication()
    {
        if (isFirstTurnPassed == false)
        {
            myAnim.SetTrigger("DisplayCursor");
        }
    }
}

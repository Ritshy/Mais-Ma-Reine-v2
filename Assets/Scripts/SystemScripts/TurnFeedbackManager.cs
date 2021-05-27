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
    public Image boutonBG;
    public Image boutonIcon;

    [Header ("Reine")]
    public Sprite boutonBGReine;
    public Sprite boutonIconReine;
    public Sprite bandeauBGReine;

    [Header("Roi")]
    public Sprite boutonBGRoi;
    public Sprite boutonIconRoi;
    public Sprite bandeauBGRoi;

    [Header("Bandits")]
    public Sprite boutonBGBandit;
    public Sprite boutonIconBandit;
    public Sprite bandeauBGBandit;

    [Header("Calamité")]
    public Sprite boutonBGCalamite;
    public Sprite boutonIconCalamite;
    public Sprite bandeauBGCalamite;

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
                boutonBG.sprite = boutonBGReine;
                boutonIcon.sprite = boutonIconReine;

                StartCoroutine(ResetFeedback());
                break;
            case GameCamps.Roi:
                myAnim.SetTrigger("Displaying");
                mySwitchTurnText.text = "Le Roi joue";
                myQuestText.enabled = false;

                bandeauBG.sprite = bandeauBGRoi;
                boutonBG.sprite = boutonBGRoi;
                boutonIcon.sprite = boutonIconRoi;

                StartCoroutine(ResetFeedback());
                break;
            case GameCamps.Bandit:
                myAnim.SetTrigger("Displaying");
                mySwitchTurnText.text = "Les Bandits jouent";
                myQuestText.enabled = false;

                bandeauBG.sprite = bandeauBGBandit;
                boutonBG.sprite = boutonBGBandit;
                boutonIcon.sprite = boutonIconBandit;

                StartCoroutine(ResetFeedback());
                break;
            case GameCamps.BanditCalamiteux:
                myAnim.SetTrigger("Displaying");
                mySwitchTurnText.text = "Les Bandits jouent";
                myQuestText.enabled = false;

                bandeauBG.sprite = bandeauBGBandit;
                boutonBG.sprite = boutonBGBandit;
                boutonIcon.sprite = boutonIconBandit;

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
}

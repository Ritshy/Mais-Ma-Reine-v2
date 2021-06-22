using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoCharacter : MonoBehaviour
{
    [Header ("Stat Holders")]

    public TextMeshProUGUI hpText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI cAText;
    public TextMeshProUGUI critText;
    public TextMeshProUGUI missText;

    [Header("Action Holders")]

    public Image deplacementImage;
    public Image interactionImage;

    [Header("Information Holders")]

    public TextMeshProUGUI characterNameText;
    public Image characterCamp;
    public Image characterCampColor;

    [Space]
    [Header("Image and Color")]

    public Sprite roiCampIcon;
    public Color roiCampColor;

    public Sprite reineCampIcon;
    public Color reineCampColor;

    public Sprite banditCampIcon;
    public Color banditCampColor;

    public Sprite villageoisCampIcon;
    public Color villageoisCampColor;

    private bool isInformationDisplayed;

    private Animator myAnim;

    #region Singleton
    public static InfoCharacter Instance;

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
        if (isInformationDisplayed && Input.GetMouseButtonDown(1))
        {
            CheckForClosingWindow();
        }
    }

    public void OpenInformationWindow(FideleManager fmToDisplay)
    {
        myAnim.SetBool("OpenInformation", true);
        isInformationDisplayed = true;

        hpText.text = fmToDisplay.currentHP.ToString();
        attackText.text = fmToDisplay.minAttackRange.ToString() + " - " + fmToDisplay.maxAttackRange.ToString();
        cAText.text = fmToDisplay.minCounterAttackRange.ToString() + " - " + fmToDisplay.maxCounterAttackRange.ToString();
        critText.text = fmToDisplay.criticChances.ToString() + "%";
        missText.text = fmToDisplay.missChances.ToString() + "%";

        characterNameText.text = fmToDisplay.fidelePrenom + " " + fmToDisplay.fideleNom;

        switch (fmToDisplay.myCamp)
        {
            case GameCamps.Fidele:
                characterCamp.sprite = reineCampIcon;
                characterCampColor.color = reineCampColor;
                break;
            case GameCamps.Roi:
                characterCamp.sprite = roiCampIcon;
                characterCampColor.color = roiCampColor;
                break;
            case GameCamps.Bandit:
                characterCamp.sprite = banditCampIcon;
                characterCampColor.color = banditCampColor;
                break;
            case GameCamps.BanditCalamiteux:
                characterCamp.sprite = banditCampIcon;
                characterCampColor.color = banditCampColor;
                break;
            case GameCamps.Villageois:
                characterCamp.sprite = villageoisCampIcon;
                characterCampColor.color = villageoisCampColor;
                break;
            default:
                break;
        }
    }

    public void CloseInformationWindow()
    {
        myAnim.SetBool("OpenInformation", false);
        isInformationDisplayed = false;
    }

    private void CheckForClosingWindow()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        if (isInformationDisplayed && hit.collider.GetComponentInParent<FideleManager>() == null)
        {
            CloseInformationWindow();
        }
    }
}


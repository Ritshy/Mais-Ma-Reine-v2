using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    private Animator myAnim;
    public bool isInFight = false;
    public TextMeshProUGUI renderTextCombat;

    [Header("Sounds Interface")]

    public AK.Wwise.Event uiBoutonBastonSFX;

    [Header("Sounds Combat")]

    public AK.Wwise.Event attaqueEpeeSFX;
    public AK.Wwise.Event contreAttaqueEpeeSFX;
    public AK.Wwise.Event coupCritiqueSFX;
    public AK.Wwise.Event echecCritiqueSFX;
    public AK.Wwise.Event criSFX;
    public AK.Wwise.Event mortSFX;

    [Header("Attaquant Fenetre")]

    public TextMeshProUGUI atkHP;
    public TextMeshProUGUI atkRange;
    public TextMeshProUGUI atkCriticChance;
    public TextMeshProUGUI atkMissChance;

    public TextMeshProUGUI attaquantName;
    public Image attaquantIcone;

    private FideleManager attaquantFM;
    private AnimationManager attaquantAM;

    [Header("Défenseur Fenetre")]

    public TextMeshProUGUI defHP;
    public TextMeshProUGUI defCounterAttackRange;

    public TextMeshProUGUI defenseurName;
    public Image defenseurIcone;

    private FideleManager defenseurFM;
    private AnimationManager defenseurAM;

    [Header("Attaquant Bandeau")]

    public Image attaquantFideleSprite;
    public TextMeshProUGUI attaquantHP;
    public ParticleSystem attaquantDamageTextPS;

    [Header("Défenseur Bandeau")]

    public Image defenseurFideleSprite;
    public TextMeshProUGUI defenseurHP;
    public ParticleSystem defenseurDamageTextPS;

    [Header("Effects & Icones")]

    public Sprite reineSprite;
    public Sprite roiSprite;
    public Sprite calamiteSprite;
    public Sprite banditSprite;

    //private ParticleSystem myCombatEffect;

    //private ParticleSystem defenseurDamageEffect;
    //private ParticleSystem attaquantDamageEffect;

    private int isCritical;
    private int isMissed;

    //private TextMeshProUGUI myDamageFeedback;

    #region Singleton

    public static CombatManager Instance;

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

    public void OpenCombatWindow(FideleManager atkFM, FideleManager defFM)
    {
        GameManager.Instance.isGamePaused = true;

        attaquantFM = atkFM;
        attaquantAM = attaquantFM.GetComponent<AnimationManager>();

        defenseurFM = defFM;
        defenseurAM = defenseurFM.GetComponentInParent<AnimationManager>();


        myAnim.SetBool("OpenCombatWindow", true);

        criSFX.Post(gameObject);


        switch (atkFM.myCamp)
        {
            case GameCamps.Fidele:
                attaquantIcone.sprite = reineSprite;
                break;
            case GameCamps.Roi:
                attaquantIcone.sprite = roiSprite;
                break;
            case GameCamps.Bandit:
                attaquantIcone.sprite = banditSprite;
                break;
            case GameCamps.BanditCalamiteux:
                attaquantIcone.sprite = banditSprite;
                break;
            case GameCamps.Calamite:
                attaquantIcone.sprite = calamiteSprite;
                break;
            case GameCamps.Villageois:
                attaquantIcone.sprite = reineSprite;
                break;
            case GameCamps.Converti:
                attaquantIcone.sprite = roiSprite;
                break;
            default:
                break;
        }

        switch (defFM.myCamp)
        {
            case GameCamps.Fidele:
                defenseurIcone.sprite = reineSprite;
                break;
            case GameCamps.Roi:
                defenseurIcone.sprite = roiSprite;
                break;
            case GameCamps.Bandit:
                defenseurIcone.sprite = banditSprite;
                break;
            case GameCamps.BanditCalamiteux:
                defenseurIcone.sprite = banditSprite;
                break;
            case GameCamps.Calamite:
                defenseurIcone.sprite = calamiteSprite;
                break;
            case GameCamps.Villageois:
                defenseurIcone.sprite = reineSprite;
                break;
            case GameCamps.Converti:
                defenseurIcone.sprite = roiSprite;
                break;
            default:
                break;
        }

        attaquantName.text = atkFM.fideleNom + (" ") + atkFM.fidelePrenom;

        atkHP.text = atkFM.currentHP.ToString();
        atkRange.text = atkFM.minAttackRange.ToString() + (" - ") + atkFM.maxAttackRange.ToString();
        atkCriticChance.text = atkFM.criticChances.ToString() + ("%");
        atkMissChance.text = atkFM.missChances.ToString() + ("%");

        defenseurName.text = defFM.fideleNom + (" ") + defFM.fidelePrenom;

        defHP.text = defFM.currentHP.ToString();
        defCounterAttackRange.text = defFM.minCounterAttackRange.ToString() + (" - ") + defFM.maxCounterAttackRange.ToString();
    }

    public void PlayerLaunchCombat()
    {
        myAnim.SetBool("OpenCombatWindow", false);
        isInFight = true;

        attaquantFideleSprite.sprite = attaquantFM.currentFideleSprite.sprite;
        defenseurFideleSprite.sprite = defenseurFM.currentFideleSprite.sprite;

        attaquantHP.text = attaquantFM.currentHP.ToString();
        defenseurHP.text = defenseurFM.currentHP.ToString();

        myAnim.SetBool("OpenCombatBandeau", true);

        uiBoutonBastonSFX.Post(gameObject);

        defenseurAM.keepInteractionDisplayed = true;
        defenseurAM.DisplayInteraction();

        StartCoroutine(DragCamera2D.Instance.FollowTargetCamera(attaquantFM.gameObject));

        attaquantAM.keepInteractionDisplayed = true;
        attaquantAM.DisplayInteraction();

        //defenseurDamageEffect = defenseurFM.GetComponentInChildren<ParticleSystem>();
        //attaquantDamageEffect = attaquantFM.GetComponentInChildren<ParticleSystem>();

        attaquantFM.isInteracting = true;
        defenseurFM.isInteracting = true;

        GameManager.Instance.LowerOpacityFeedback();

        isCritical = Random.Range(0, 100);
        if (isCritical <= attaquantFM.criticChances)
        {
            if (attaquantFM != null && defenseurFM != null)
            {
                StartCoroutine(CriticalHit());
            }
        }
        else
        {
            isMissed = Random.Range(0, 100);
            if (isMissed <= attaquantFM.missChances)
            {
                if (attaquantFM != null && defenseurFM != null)
                {
                    StartCoroutine(Missed());
                }
            }
            else
            {
                if (attaquantFM != null && defenseurFM != null)
                {
                    StartCoroutine(Attack());
                }
            }
        }
    }

    public void EnemyLaunchCombat(FideleManager atkFM, FideleManager defFM)
    {
        if (attaquantFM == null && defenseurFM == null)
        {
            attaquantFM = atkFM;
            attaquantAM = attaquantFM.GetComponent<AnimationManager>();

            defenseurFM = defFM;
            defenseurAM = defenseurFM.GetComponentInParent<AnimationManager>();
        }

        GameManager.Instance.isGamePaused = true;

        myAnim.SetBool("OpenCombatBandeau", true);
        isInFight = true;

        attaquantFideleSprite.sprite = attaquantFM.currentFideleSprite.sprite;
        defenseurFideleSprite.sprite = defenseurFM.currentFideleSprite.sprite;

        attaquantHP.text = attaquantFM.currentHP.ToString();
        defenseurHP.text = defenseurFM.currentHP.ToString();

        defenseurAM.keepInteractionDisplayed = true;
        defenseurAM.DisplayInteraction();

        StartCoroutine(DragCamera2D.Instance.FollowTargetCamera(attaquantFM.gameObject));

        attaquantAM.keepInteractionDisplayed = true;
        attaquantAM.DisplayInteraction();

        //defenseurDamageEffect = defenseurFM.GetComponentInChildren<ParticleSystem>();
        //attaquantDamageEffect = attaquantFM.GetComponentInChildren<ParticleSystem>();

        attaquantFM.isInteracting = true;
        defenseurFM.isInteracting = true;

        GameManager.Instance.LowerOpacityFeedback();

        isCritical = Random.Range(0, 100);
        if (isCritical <= attaquantFM.criticChances)
        {
            if (attaquantFM != null && defenseurFM != null)
            {
                StartCoroutine(CriticalHit());
            }
        }
        else
        {
            isMissed = Random.Range(0, 100);
            if (isMissed <= attaquantFM.missChances)
            {
                if (attaquantFM != null && defenseurFM != null)
                {
                    StartCoroutine(Missed());
                }
            }
            else
            {
                if (attaquantFM != null && defenseurFM != null)
                {
                    StartCoroutine(Attack());
                }
            }
        }
    }

    public IEnumerator Attack()
    {    
        Debug.Log("Attack() " + defenseurFM.name + " " + " " + attaquantFM.name);

        if (attaquantFM.isAlive && defenseurFM.isAlive)
        {
            int attackValue = Random.Range(attaquantFM.minAttackRange, attaquantFM.maxAttackRange);
            defenseurFM.currentHP -= attackValue;

            if (defenseurFM.currentHP <= 0)
            {
                defenseurFM.currentHP = 0;
            }

            Debug.Log(attaquantFM.name + " inflige " + attackValue + " points de dégâts, laissant " + defenseurFM.name + " à " + defenseurFM.currentHP);

            // ICI jouer VFX d'attaque simple
            // ICI jouer SFX d'attaque simple

            //myDamageFeedback.text = "-" + attackValue.ToString();

            myAnim.SetTrigger("LaunchAttack");
            attaqueEpeeSFX.Post(gameObject);

            yield return new WaitForSeconds(0.3f);

            myAnim.SetTrigger("DefenseurReceiveDamage");
            //attaquantDamageEffect.Play();

            yield return new WaitForSeconds(1f);

            // Ici jouer Anim dégâts reçus sur defenseur
            renderTextCombat.text = "- " + attackValue.ToString();
            defenseurDamageTextPS.gameObject.SetActive(true);
            defenseurAM.ReceiveDamage();
            defenseurAM.FillAmountHealth();
            
            defenseurHP.text = defenseurFM.currentHP.ToString();

            yield return new WaitForSeconds(1f);

            CheckHP();
            yield return new WaitForSeconds(0.5f);

            defenseurDamageTextPS.gameObject.SetActive(false);

            if (attaquantFM != null && defenseurFM != null)
            {
                if (attaquantFM.isAlive && defenseurFM.isAlive)
                {
                    StartCoroutine(CounterAttack());
                }
            }
        }
    }

    public IEnumerator CounterAttack()
    {
        Debug.Log("CounterAttack() " + defenseurFM.name + " " + " " + attaquantFM.name);

        int counterAttackValue = Random.Range(defenseurFM.minCounterAttackRange, defenseurFM.maxCounterAttackRange);
        attaquantFM.currentHP -= counterAttackValue;

        if (attaquantFM.currentHP <= 0)
        {
            attaquantFM.currentHP = 0;
        }

        Debug.Log("Le défenseur contre-attaque et inflige" + counterAttackValue + "points de dégâts, laissant son adversaire à " + attaquantFM.currentHP);


        // ICI jouer VFX de contre-attaque simple
        // ICI jouer SFX de contre-attaque simple
        //myDamageFeedback.text = "-" + counterAttackValue.ToString();
        myAnim.SetTrigger("LaunchCounterAttack");
        contreAttaqueEpeeSFX.Post(gameObject);

        yield return new WaitForSeconds(0.3f);

        myAnim.SetTrigger("AttaquantReceiveDamage");

        //defenseurDamageEffect.Play();

        yield return new WaitForSeconds(1f);

        // Ici jouer Anim dégâts reçus sur attaquant

        renderTextCombat.text = "- " + counterAttackValue.ToString();
        attaquantDamageTextPS.gameObject.SetActive(true);

        attaquantAM.ReceiveDamage();
        attaquantAM.FillAmountHealth();

        attaquantHP.text = attaquantFM.currentHP.ToString();

        yield return new WaitForSeconds(1f);

        CheckHP();
        yield return new WaitForSeconds(0.5f);

        attaquantDamageTextPS.gameObject.SetActive(false);

        if (attaquantFM != null && defenseurFM != null)
        {
            if (attaquantFM.isAlive && defenseurFM.isAlive)
            {
                StartCoroutine(EndFightNoDead());
            }
        }
    }

    public void CheckHP()
    {
        if (attaquantFM != null && defenseurFM != null)
        {
            if (attaquantFM.currentHP <= 0)
            {
                attaquantFM.isAlive = false;
                Debug.Log("L'attaquant est vaincu !");

                StartCoroutine(Die(attaquantFM, defenseurFM));
            }
            else if (defenseurFM.currentHP <= 0)
            {
                defenseurFM.isAlive = false;
                Debug.Log("Le défenseur est vaincu !");

                StartCoroutine(Die(defenseurFM, attaquantFM));
            }
        }
    }

    public IEnumerator CriticalHit()
    {
        Debug.Log("Critical() " + defenseurFM.name + " " + " " + attaquantFM.name);

        defenseurFM.currentHP -= attaquantFM.maxAttackRange * 2;

        if (defenseurFM.currentHP <= 0)
        {
            defenseurFM.currentHP = 0;
        }

        Debug.Log("OUH ! CRITIQUE !!");
        Debug.Log("Avec un coup critique, " + attaquantFM.name + " inflige " + attaquantFM.maxAttackRange * 2 + " points de dégâts, laissant " + defenseurFM.name + " à " + defenseurFM.currentHP);


        // ICI jouer VFX de coup critiique
        // ICI jouer SFX de coup critique

        //myDamageFeedback.text = "-" + (attaquantFM.maxAttackRange * 2).ToString() + (" !!");
        //attaquantDamageEffect.Play();

        myAnim.SetTrigger("DefenseurReceiveDamage");
        myAnim.SetTrigger("LaunchCoupCritique");
        coupCritiqueSFX.Post(gameObject);

        yield return new WaitForSeconds(2f);

        // ICI jouer Anim dégâts reçus sur defenseur
        renderTextCombat.text = "- " + (attaquantFM.maxAttackRange * 2).ToString();
        defenseurDamageTextPS.gameObject.SetActive(true);
        defenseurAM.ReceiveDamage();
        defenseurAM.FillAmountHealth();
        
        defenseurHP.text = defenseurFM.currentHP.ToString();

        yield return new WaitForSeconds(2f);

        CheckHP();
        yield return new WaitForSeconds(0.5f);

        defenseurDamageTextPS.gameObject.SetActive(false);

        if (attaquantFM != null && defenseurFM != null)
        {
            if (attaquantFM.isAlive && defenseurFM.isAlive)
            {
                StartCoroutine(EndFightNoDead());
            }
        }
    }

    public IEnumerator Missed()
    {
        Debug.Log("Missed " + defenseurFM.name + " " + " " + attaquantFM.name);

        if (attaquantFM != null && defenseurFM != null)
        {
            attaquantFM.currentHP -= defenseurFM.maxCounterAttackRange;

            if (attaquantFM.currentHP <= 0)
            {
                attaquantFM.currentHP = 0;
            }

            Debug.Log("Loupé !! Aie aie aie !!");
            Debug.Log("Avec un l'échec critique de l'attaquant, le défenseur contre attaque et inflige " + defenseurFM.maxCounterAttackRange + "points de dégâts, laissant son adversaire à " + attaquantFM.currentHP);


            // ICI jouer VFX d'echec critiique
            // ICI jouer SFX d'echec critique

            myAnim.SetTrigger("AttaquantReceiveDamage");
            myAnim.SetTrigger("LaunchEchecCritique");
            //echecCritiqueSFX.Post(gameObject);

            //myDamageFeedback.text = "-" + defenseurFM.maxCounterAttackRange.ToString() + " !!";
            //defenseurDamageEffect.Play();

            yield return new WaitForSeconds(2f);

            // ICI jouer Anim dégâts reçus sur attaquant
            renderTextCombat.text = "- " + (defenseurFM.maxCounterAttackRange).ToString();
            attaquantDamageTextPS.gameObject.SetActive(true);

            attaquantAM.ReceiveDamage();
            attaquantAM.FillAmountHealth();

            attaquantHP.text = attaquantFM.currentHP.ToString();

            yield return new WaitForSeconds(2f);           

            CheckHP();
            yield return new WaitForSeconds(0.5f);

            attaquantDamageTextPS.gameObject.SetActive(false);

            if (attaquantFM != null && defenseurFM != null)
            {
                if (attaquantFM.isAlive && defenseurFM.isAlive)
                {
                    StartCoroutine(EndFightNoDead());
                }
            }
        }
    }

    public IEnumerator Die(FideleManager deadFM, FideleManager winFM)
    {
        Debug.Log("Die() " + defenseurFM.name + " " + " " + attaquantFM.name);

        //Debug.Log("On Tue quelqu'un");
        myAnim.SetBool("OpenCombatBandeau", false);

        //myDamageFeedback.text = "";
        //myDamageFeedback = null;

        if (attaquantFM.myCamp == GameCamps.Fidele)
        {
            GameManager.Instance.isGamePaused = false;
        }

        DragCamera2D.Instance.UnfollowTargetCamera();
        QuestManager.Instance.OnKillUnit(deadFM);

        defenseurAM.CheckActionsLeftAmout();
        attaquantAM.CheckActionsLeftAmout();

        if (deadFM.myCamp != GameCamps.Bandit)
        {
            deadFM.GetComponent<AnimationManager>().Dying();
            // ICI jouer Anim de mort
            // ICI jouer SFX de mort

            yield return new WaitForSeconds(1f);


            if (deadFM.myCamp == GameCamps.Fidele)
            {
                GameManager.Instance.AddCharismeValue(-5);
            }

            foreach (AnimationManager dfmcam in deadFM.GetComponentInChildren<Interaction>().myCollideAnimationManagerList)
            {
                dfmcam.GetComponentInChildren<Interaction>().RemoveCollidingCharacterFromAMList(deadFM.GetComponent<AnimationManager>());

                dfmcam.GetComponent<AnimationManager>().haveAnInteraction = false;

                dfmcam.HideInteraction();
                dfmcam.DesactivateReceiverSelection();
            }

            foreach (Interaction dfmci in deadFM.GetComponentInChildren<Interaction>().myCollideInteractionList)
            {
                dfmci.RemoveCollidingCharacterFromInteractionList(deadFM.GetComponentInChildren<Interaction>());

                dfmci.FideleDisplayInteractionFeedbacks();
            }

            foreach (FideleManager dfmcfm in deadFM.unitsInRange)
            {
                dfmcfm.unitsInRange.Remove(deadFM);
            }

            deadFM.GetComponentInChildren<Interaction>().myCollideAnimationManagerList.Clear();
            deadFM.GetComponentInChildren<Interaction>().myCollideInteractionList.Clear();

            deadFM.GetComponent<AnimationManager>().haveAnInteraction = false;

            deadFM.GetComponent<AnimationManager>().HideInteraction();
            deadFM.GetComponent<AnimationManager>().DesactivateReceiverSelection();

            //winFM.GetComponentInChildren<Interaction>().myCollideAnimationManagerList.Remove(deadFM.GetComponent<AnimationManager>());
            //winFM.GetComponentInChildren<Interaction>().myCollideInteractionList.Remove(deadFM.GetComponentInChildren<Interaction>());

            attaquantAM.keepInteractionDisplayed = false;
            attaquantAM.HideInteraction();

            defenseurAM.keepInteractionDisplayed = false;
            defenseurAM.HideInteraction();

            attaquantFM.GetComponentInChildren<Interaction>().OtherCampDisplayInteractionFeedbacks();
            defenseurFM.GetComponentInChildren<Interaction>().OtherCampDisplayInteractionFeedbacks();


            attaquantFM.isInteracting = false;
            defenseurFM.isInteracting = false;
            GameManager.Instance.ResetOpacityFeedback();

            GameManager.Instance.RemoveAMapUnit(deadFM);
            winFM.KillUnit(deadFM);
            Destroy(deadFM.gameObject);
        }
        else if (deadFM.myCamp == GameCamps.Bandit && winFM.myCamp == GameCamps.Fidele)
        {
            attaquantFM.isInteracting = false;
            defenseurFM.isInteracting = false;

            SwitchInteractionType(deadFM);

            attaquantAM.keepInteractionDisplayed = false;
            attaquantAM.HideInteraction();

            defenseurAM.keepInteractionDisplayed = false;
            defenseurAM.HideInteraction();
        }

        mortSFX.Post(gameObject);

        yield return new WaitForSeconds(0.4f);

        if (deadFM.myCamp == GameCamps.Fidele)
        {
            GameManager.Instance.CheckIfPlayerLost();
        }

        attaquantFideleSprite.sprite = null;
        defenseurFideleSprite.sprite = null;

        attaquantFM = null;
        attaquantAM = null;

        defenseurFM = null;
        defenseurAM = null;

        isInFight = false;

        GameManager.Instance.MoveUnit();
    }

    public IEnumerator EndFightNoDead()
    {
        Debug.Log("EndFightNoDead " + defenseurFM.name + " " + " " + attaquantFM.name);

        DragCamera2D.Instance.UnfollowTargetCamera();

        //Debug.Log("Combat terminé");
        myAnim.SetBool("OpenCombatBandeau", false);

        defenseurAM.CheckActionsLeftAmout();
        attaquantAM.CheckActionsLeftAmout();

        attaquantFM.GetComponentInChildren<Interaction>().OtherCampDisplayInteractionFeedbacks();
        defenseurFM.GetComponentInChildren<Interaction>().OtherCampDisplayInteractionFeedbacks();

        //myDamageFeedback.text = "";
        //myDamageFeedback = null;

        attaquantFM.isInteracting = false;
        defenseurFM.isInteracting = false;
        GameManager.Instance.ResetOpacityFeedback();

        attaquantAM.keepInteractionDisplayed = false;
        attaquantAM.HideInteraction();
        attaquantAM.DesactivateReceiverSelection();

        defenseurAM.keepInteractionDisplayed = false;
        defenseurAM.HideInteraction();
        defenseurAM.DesactivateReceiverSelection();

        if (attaquantFM.myCamp == GameCamps.Fidele)
        {
            GameManager.Instance.isGamePaused = false;
        }

        yield return new WaitForSeconds(0.4f);

        attaquantFideleSprite.sprite = null;
        defenseurFideleSprite.sprite = null;

        attaquantFM = null;
        attaquantAM = null;

        defenseurFM = null;
        defenseurAM = null;

        isInFight = false;

        GameManager.Instance.MoveUnit();
    }

    public void SwitchInteractionType(FideleManager deadFM)
    {
        // ICI jouer SFX de mort
        // ICI jouer anim du Bandit qui change de camp
        // ICI changer graphisme du bandit qui change de camp
        GetComponent<Interaction>().interactionType = InteractionType.Recrutement;
        GetComponent<Interaction>().AnimationManagerUpdateIcon();


        deadFM.isAlive = false;
    }

    public void CancelCombat()
    {
        myAnim.SetBool("OpenCombatWindow", false);
        GameManager.Instance.isGamePaused = false;

        attaquantFM.GetComponentInChildren<Interaction>().alreadyInteractedList.Remove(defenseurFM.GetComponentInChildren<Interaction>());

        attaquantAM.CheckActionsLeftAmout();

        attaquantFM.GetComponentInChildren<Interaction>().OtherCampDisplayInteractionFeedbacks();

        attaquantFM = null;
        attaquantAM = null;

        defenseurFM = null;
        defenseurAM = null;
    }
}

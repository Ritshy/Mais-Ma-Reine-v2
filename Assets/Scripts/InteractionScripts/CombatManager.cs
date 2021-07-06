using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Vulnerability { Faible, Neutre, Resistant }

public class CombatManager : MonoBehaviour
{
    private Animator myAnim;
    public bool isInFight = false;
    public TextMeshProUGUI renderTextCombat;
    public List<CanvasGroup> objectsToDisableWhileInFight;

    public int vulnerabilityMultiplier;

    public float criticalMultiplier;
    public float missMultiplier;

    public Sprite reineCampBG;
    public Sprite roiCampBG;
    public Sprite banditCampBG;

    public Sprite epeisteIcone;
    public Sprite magicianIcone;
    public Sprite lancierIcone;

    public Sprite avantageIcone;
    public Sprite desavantageIcone;

    public Image iconeModificateurCombat;

    [Header("Sounds Interface")]

    public AK.Wwise.Event uiBoutonBastonSFX;

    [Header("Sounds Combat")]

    public AK.Wwise.Event criSFX;
    public AK.Wwise.Event mortSFX;

    [Space]

    [Header("Attaquant Fenetre")]

    public ParticleSystem deadEffect;

    public TextMeshProUGUI atkHP;
    public TextMeshProUGUI atkRange;
    public TextMeshProUGUI atkCriticChance;
    public TextMeshProUGUI atkMissChance;

    public TextMeshProUGUI attaquantName;
    public Image attaquantIcone;
    public Image attaquantClasseIcone;

    private FideleManager attaquantFM;
    private AnimationManager attaquantAM;

    [Header("Défenseur Fenetre")]

    public TextMeshProUGUI defHP;
    public TextMeshProUGUI defCounterAttackRange;

    public TextMeshProUGUI defenseurName;
    public Image defenseurIcone;
    public Image defenseurClasseIcone;

    private FideleManager defenseurFM;
    private AnimationManager defenseurAM;

    [Header("Attaquant Bandeau")]

    public Image attaquantFideleSprite;
    public TextMeshProUGUI attaquantHP;
    public ParticleSystem attaquantDamagePS;

    public Image attaquantBrasImage;

    [Header("Défenseur Bandeau")]

    public Image defenseurFideleSprite;
    public TextMeshProUGUI defenseurHP;
    public ParticleSystem defenseurDamagePS;

    public Image defenseurBrasImage;

    private int isCritical;
    private int isMissed;
    
    private Vulnerability attaquantVulnerability;
    private Vulnerability defenseurVulnerability;

    private float currentAttackValue;

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
        /*if (isInFight && Input.GetMouseButton(0))
        {
            Time.timeScale = 3;
        }
        else
        {
            Time.timeScale = 1;
        }*/
    }

    public void OpenCombatWindow(FideleManager atkFM, FideleManager defFM)
    {
        GameManager.Instance.isGamePaused = true;

        attaquantFM = atkFM;
        attaquantAM = attaquantFM.GetComponent<AnimationManager>();

        defenseurFM = defFM;
        defenseurAM = defenseurFM.GetComponentInParent<AnimationManager>();

        DetermineVulnerability(atkFM, defFM);

        switch (attaquantFM.myCamp)
        {
            case GameCamps.Fidele:
                attaquantIcone.sprite = reineCampBG;
                break;
            case GameCamps.Roi:
                attaquantIcone.sprite = roiCampBG;
                break;
            case GameCamps.Bandit:
                attaquantIcone.sprite = banditCampBG;
                break;
            case GameCamps.BanditCalamiteux:
                attaquantIcone.sprite = banditCampBG;
                break;
            default:
                break;
        }

        switch (defenseurFM.myCamp)
        {
            case GameCamps.Fidele:
                defenseurIcone.sprite = reineCampBG;
                break;
            case GameCamps.Roi:
                defenseurIcone.sprite = roiCampBG;
                break;
            case GameCamps.Bandit:
                defenseurIcone.sprite = banditCampBG;
                break;
            case GameCamps.BanditCalamiteux:
                defenseurIcone.sprite = banditCampBG;
                break;
            default:
                break;
        }

        switch (attaquantFM.fideleClasse)
        {
            case Classes.Epeiste:
                attaquantClasseIcone.sprite = epeisteIcone;
                break;
            case Classes.Magicien:
                attaquantClasseIcone.sprite = magicianIcone;
                break;
            case Classes.Lancier:
                attaquantClasseIcone.sprite = lancierIcone;
                break;
            default:
                break;
        }

        switch (defenseurFM.fideleClasse)
        {
            case Classes.Epeiste:
                defenseurClasseIcone.sprite = epeisteIcone;
                break;
            case Classes.Magicien:
                defenseurClasseIcone.sprite = magicianIcone;
                break;
            case Classes.Lancier:
                defenseurClasseIcone.sprite = lancierIcone;
                break;
            default:
                break;
        }

        switch (attaquantVulnerability)
        {
            case Vulnerability.Faible:
                iconeModificateurCombat.sprite = desavantageIcone;
                iconeModificateurCombat.color = new Color(1, 1, 1, 1);
                break;
            case Vulnerability.Neutre:
                iconeModificateurCombat.sprite = null;
                iconeModificateurCombat.color = new Color(1, 1, 1, 0);
                break;
            case Vulnerability.Resistant:
                iconeModificateurCombat.sprite = avantageIcone;
                iconeModificateurCombat.color = new Color(1, 1, 1, 1);
                break;
            default:
                break;
        }

        myAnim.SetBool("OpenCombatWindow", true);

        criSFX.Post(gameObject);

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

        CameraZooming.Instance.ShakeScreen();

        attaquantFideleSprite.sprite = attaquantFM.inFightSprite;
        defenseurFideleSprite.sprite = defenseurFM.inFightSprite;

        attaquantHP.text = attaquantFM.currentHP.ToString();
        defenseurHP.text = defenseurFM.currentHP.ToString();

        myAnim.SetBool("OpenCombatBandeau", true);

        for (int i = 0; i < objectsToDisableWhileInFight.Count; i++)
        {
            objectsToDisableWhileInFight[i].alpha = 0;
        }

        CameraZooming.Instance.ShakeScreen();

        attaquantBrasImage.fillAmount = attaquantFM.currentHP * 1f / attaquantFM.maxHp * 1f;
        defenseurBrasImage.fillAmount = defenseurFM.currentHP * 1f / defenseurFM.maxHp * 1f;

        EffectManager.Instance.versusEffect.Play();
        CameraZooming.Instance.ShakeScreen();

        uiBoutonBastonSFX.Post(gameObject);

        defenseurAM.keepInteractionDisplayed = true;
        defenseurAM.DisplayInteraction();

        StartCoroutine(DragCamera2D.Instance.FollowTargetCamera(attaquantFM.gameObject));

        attaquantAM.keepInteractionDisplayed = true;
        attaquantAM.DisplayInteraction();

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
        attaquantFM = atkFM;
        attaquantAM = attaquantFM.GetComponent<AnimationManager>();

        defenseurFM = defFM;
        defenseurAM = defenseurFM.GetComponentInParent<AnimationManager>();

        attaquantFM.hasFought = true;

        DetermineVulnerability(atkFM, defFM);

        if (attaquantFM.isAlive && defenseurFM.isAlive)
        {
            GameManager.Instance.isGamePaused = true;

            myAnim.SetBool("OpenCombatBandeau", true);

            for (int i = 0; i < objectsToDisableWhileInFight.Count; i++)
            {
                objectsToDisableWhileInFight[i].alpha = 0;
            }

            attaquantBrasImage.fillAmount = attaquantFM.currentHP * 1f / attaquantFM.maxHp * 1f;
            defenseurBrasImage.fillAmount = defenseurFM.currentHP * 1f / defenseurFM.maxHp * 1f;

            EffectManager.Instance.versusEffect.Play();
            CameraZooming.Instance.ShakeScreen();

            isInFight = true;

            attaquantFideleSprite.sprite = attaquantFM.inFightSprite;
            defenseurFideleSprite.sprite = defenseurFM.inFightSprite;

            attaquantHP.text = attaquantFM.currentHP.ToString();
            defenseurHP.text = defenseurFM.currentHP.ToString();

            defenseurAM.keepInteractionDisplayed = true;
            defenseurAM.DisplayInteraction();

            StartCoroutine(DragCamera2D.Instance.FollowTargetCamera(attaquantFM.gameObject));

            attaquantAM.keepInteractionDisplayed = true;
            attaquantAM.DisplayInteraction();

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
    }

    public void DetermineVulnerability(FideleManager atkFM, FideleManager defFM)
    {
        switch (atkFM.fideleClasse)
        {
            case Classes.Epeiste:
                switch (defenseurFM.fideleClasse)
                {
                    case Classes.Epeiste:
                        defenseurVulnerability = Vulnerability.Neutre;
                        attaquantVulnerability = Vulnerability.Neutre;
                        break;
                    case Classes.Magicien:
                        defenseurVulnerability = Vulnerability.Resistant;
                        attaquantVulnerability = Vulnerability.Faible;
                        break;
                    case Classes.Lancier:
                        defenseurVulnerability = Vulnerability.Faible;
                        attaquantVulnerability = Vulnerability.Resistant;
                        break;
                    default:
                        break;
                }
                break;
            case Classes.Magicien:
                switch (defenseurFM.fideleClasse)
                {
                    case Classes.Epeiste:
                        defenseurVulnerability = Vulnerability.Faible;
                        attaquantVulnerability = Vulnerability.Resistant;
                        break;
                    case Classes.Magicien:
                        defenseurVulnerability = Vulnerability.Neutre;
                        attaquantVulnerability = Vulnerability.Neutre;
                        break;
                    case Classes.Lancier:
                        defenseurVulnerability = Vulnerability.Resistant;
                        attaquantVulnerability = Vulnerability.Faible;
                        break;
                    default:
                        break;
                }
                break;
            case Classes.Lancier:
                switch (defenseurFM.fideleClasse)
                {
                    case Classes.Epeiste:
                        defenseurVulnerability = Vulnerability.Resistant;
                        attaquantVulnerability = Vulnerability.Faible;
                        break;
                    case Classes.Magicien:
                        defenseurVulnerability = Vulnerability.Faible;
                        attaquantVulnerability = Vulnerability.Resistant;
                        break;
                    case Classes.Lancier:
                        defenseurVulnerability = Vulnerability.Neutre;
                        attaquantVulnerability = Vulnerability.Neutre;
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }

    public IEnumerator Attack()
    {
        if (attaquantFM != null && defenseurFM != null)
        {
            if (attaquantFM.isAlive && defenseurFM.isAlive)
            {
                switch (defenseurVulnerability)
                {
                    case Vulnerability.Faible:
                        currentAttackValue = Random.Range(attaquantFM.minAttackRange, attaquantFM.maxAttackRange);
                        Debug.Log("Valeur d'attaque avant modification : " + currentAttackValue);

                        currentAttackValue = currentAttackValue + vulnerabilityMultiplier;
                        Debug.Log("Coup puissant infligé ! Nouvelle valeur d'attaque : " + currentAttackValue);
                        break;
                    case Vulnerability.Neutre:
                        currentAttackValue = Random.Range(attaquantFM.minAttackRange, attaquantFM.maxAttackRange);
                        Debug.Log("Coup neutre infligé !");
                        break;
                    case Vulnerability.Resistant:
                        currentAttackValue = Random.Range(attaquantFM.minAttackRange, attaquantFM.maxAttackRange);
                        Debug.Log("Valeur d'attaque avant modification : " + currentAttackValue);

                        currentAttackValue = currentAttackValue - vulnerabilityMultiplier;
                        Debug.Log("Coup faible infligé ! Nouvelle valeur d'attaque : " + currentAttackValue);
                        break;
                    default:
                        break;
                }

                currentAttackValue = Mathf.RoundToInt(currentAttackValue);

                if (currentAttackValue <= 0)
                {
                    currentAttackValue = 1;
                }

                defenseurFM.currentHP -= (int)currentAttackValue;

                if (defenseurFM.currentHP <= 0)
                {
                    defenseurFM.currentHP = 0;
                }

                //Debug.Log(attaquantFM.name + " inflige " + attackValue + " points de dégâts, laissant " + defenseurFM.name + " à " + defenseurFM.currentHP);

                // ICI jouer VFX d'attaque simple
                // ICI jouer SFX d'attaque simple

                //myDamageFeedback.text = "-" + attackValue.ToString();

                yield return new WaitForSeconds(.7f);

                EffectManager.Instance.attackTextEffect.Play();

                switch (attaquantFM.fideleClasse)
                {
                    case Classes.Epeiste:

                        yield return new WaitForSeconds(.5f);

                        myAnim.SetTrigger("EPLaunchAttack");

                        StartCoroutine(EffectManager.Instance.PlayEpeisteAttackEffect());

                        yield return new WaitForSeconds(.15f);

                        criSFX.Post(gameObject);
                        CameraZooming.Instance.ShakeScreen();

                        myAnim.SetTrigger("DefenseurReceiveDamage");

                        break;
                    case Classes.Magicien:

                        yield return new WaitForSeconds(.5f);

                        myAnim.SetTrigger("MALaunchAttack");

                        yield return new WaitForSeconds(.1f);

                        StartCoroutine(EffectManager.Instance.PlayMagicianAttackEffect());

                        yield return new WaitForSeconds(3.2f);

                        criSFX.Post(gameObject);
                        CameraZooming.Instance.ShakeScreen();

                        myAnim.SetTrigger("DefenseurReceiveDamage");

                        break;
                    case Classes.Lancier:

                        yield return new WaitForSeconds(.5f);

                        myAnim.SetTrigger("LALaunchAttack");

                        StartCoroutine(EffectManager.Instance.PlayLancierAttackEffect());

                        yield return new WaitForSeconds(3.2f);

                        criSFX.Post(gameObject);
                        CameraZooming.Instance.ShakeScreen();

                        myAnim.SetTrigger("DefenseurReceiveDamage");
                        break;
                    default:
                        break;
                }

                //attaquantDamageEffect.Play();

                yield return new WaitForSeconds(.5f);

                // Ici jouer Anim dégâts reçus sur defenseur

                renderTextCombat.text = "- " + currentAttackValue.ToString();
                defenseurAM.ReceiveDamage();
                defenseurAM.FillAmountHealth();

                attaquantBrasImage.fillAmount = attaquantFM.currentHP * 1f / attaquantFM.maxHp * 1f;
                defenseurBrasImage.fillAmount = defenseurFM.currentHP * 1f / defenseurFM.maxHp * 1f;


                myAnim.SetTrigger("PanneauTrigger");

                yield return new WaitForSeconds(1.5f);

                defenseurDamagePS.Play();
                defenseurHP.text = defenseurFM.currentHP.ToString();
                CheckHP();

                yield return new WaitForSeconds(1f);

                myAnim.SetTrigger("PanneauTrigger");

                if (attaquantFM != null && defenseurFM != null)
                {
                    if (attaquantFM.isAlive && defenseurFM.isAlive)
                    {
                        StartCoroutine(CounterAttack());
                    }
                }
            }
        }
        else
        {
            EndFightNoDead();
        }
    }

    public IEnumerator CounterAttack()
    {
        if (attaquantFM != null && defenseurFM != null)
        {
            switch (attaquantVulnerability)
            {
                case Vulnerability.Faible:
                    currentAttackValue = Random.Range(defenseurFM.minCounterAttackRange, defenseurFM.maxCounterAttackRange);
                    Debug.Log("Valeur d'attaque avant modification : " + currentAttackValue);

                    currentAttackValue = currentAttackValue + vulnerabilityMultiplier;
                    Debug.Log("Coup puissant infligé, Valeur d'attaque après modification : " + currentAttackValue);
                    break;
                case Vulnerability.Neutre:
                    currentAttackValue = Random.Range(defenseurFM.minCounterAttackRange, defenseurFM.maxCounterAttackRange);
                    Debug.Log("Coup neutre infligé !");
                    break;
                case Vulnerability.Resistant:
                    currentAttackValue = Random.Range(defenseurFM.minCounterAttackRange, defenseurFM.maxCounterAttackRange);
                    Debug.Log("Valeur d'attaque avant modification : " + currentAttackValue);

                    currentAttackValue = currentAttackValue - vulnerabilityMultiplier;
                    Debug.Log("Coup faible infligé, Valeur d'attaque après modification : " + currentAttackValue);
                    break;
                default:
                    break;
            }

            currentAttackValue = Mathf.RoundToInt(currentAttackValue);

            if (currentAttackValue <= 0)
            {
                currentAttackValue = 1;
            }

            attaquantFM.currentHP -= (int)currentAttackValue;


            if (attaquantFM.currentHP <= 0)
            {
                attaquantFM.currentHP = 0;
            }

            // ICI jouer VFX de contre-attaque simple
            // ICI jouer SFX de contre-attaque simple
            //myDamageFeedback.text = "-" + counterAttackValue.ToString();

            yield return new WaitForSeconds(.7f);

            EffectManager.Instance.counterAttackTextEffect.Play();

            switch (defenseurFM.fideleClasse)
            {
                case Classes.Epeiste:

                    yield return new WaitForSeconds(.5f);

                    myAnim.SetTrigger("EPLaunchCounterAttack");

                    StartCoroutine(EffectManager.Instance.PlayEpeisteCounterAttackEffect());

                    yield return new WaitForSeconds(.15f);

                    criSFX.Post(gameObject);
                    CameraZooming.Instance.ShakeScreen();

                    myAnim.SetTrigger("AttaquantReceiveDamage");

                    break;
                case Classes.Magicien:

                    yield return new WaitForSeconds(.5f);

                    myAnim.SetTrigger("MALaunchCounterAttack");

                    StartCoroutine(EffectManager.Instance.PlayMagicianCounterAttackEffect());

                    yield return new WaitForSeconds(0.7f);

                    //attaqueSimpleSlashSFX.Post(gameObject);

                    yield return new WaitForSeconds(.5f);

                    //attaqueSimpleImpactSFX.Post(gameObject);

                    yield return new WaitForSeconds(.15f);

                    criSFX.Post(gameObject);
                    CameraZooming.Instance.ShakeScreen();

                    myAnim.SetTrigger("AttaquantReceiveDamage");

                    break;
                case Classes.Lancier:

                    yield return new WaitForSeconds(.5f);

                    myAnim.SetTrigger("LALaunchCounterAttack");

                    StartCoroutine(EffectManager.Instance.PlayLancierCounterAttackkEffect());

                    yield return new WaitForSeconds(0.7f);

                    //attaqueSimpleSlashSFX.Post(gameObject);

                    yield return new WaitForSeconds(.5f);

                    //attaqueSimpleImpactSFX.Post(gameObject);

                    yield return new WaitForSeconds(.15f);

                    criSFX.Post(gameObject);
                    CameraZooming.Instance.ShakeScreen();

                    myAnim.SetTrigger("AttaquantReceiveDamage");

                    break;
                default:
                    break;
            }

            //defenseurDamageEffect.Play();

            yield return new WaitForSeconds(.5f);

            // Ici jouer Anim dégâts reçus sur attaquant

            renderTextCombat.text = "- " + currentAttackValue.ToString();

            attaquantAM.ReceiveDamage();
            attaquantAM.FillAmountHealth();

            attaquantBrasImage.fillAmount = attaquantFM.currentHP * 1f / attaquantFM.maxHp * 1f;
            defenseurBrasImage.fillAmount = defenseurFM.currentHP * 1f / defenseurFM.maxHp * 1f;


            myAnim.SetTrigger("PanneauTrigger");

            yield return new WaitForSeconds(1.5f);

            attaquantDamagePS.Play();
            attaquantHP.text = attaquantFM.currentHP.ToString();
            CheckHP();

            yield return new WaitForSeconds(1f);

            myAnim.SetTrigger("PanneauTrigger");

            if (attaquantFM != null && defenseurFM != null)
            {
                if (attaquantFM.isAlive && defenseurFM.isAlive)
                {
                    StartCoroutine(EndFightNoDead());
                }
            }
        }
        else
        {
            EndFightNoDead();
        }
    }

    public void CheckHP()
    {
        if (attaquantFM != null && defenseurFM != null)
        {
            if (attaquantFM.currentHP <= 0)
            {
                attaquantFM.isAlive = false;

                StartCoroutine(Die(attaquantFM, defenseurFM));
            }
            else if (defenseurFM.currentHP <= 0)
            {
                defenseurFM.isAlive = false;

                StartCoroutine(Die(defenseurFM, attaquantFM));
            }
        }
    }

    public IEnumerator CriticalHit()
    {
        if (attaquantFM != null && defenseurFM != null)
        {
            switch (defenseurVulnerability)
            {
                case Vulnerability.Faible:
                    Debug.Log("Valeur d'attaque avant modification de critique : " + (attaquantFM.maxAttackRange + vulnerabilityMultiplier));
                    currentAttackValue = (attaquantFM.maxAttackRange + vulnerabilityMultiplier) * criticalMultiplier;
                    Debug.Log("Coup puissant infligé, valeur d'attaque après modification de critique : " + currentAttackValue);
                    break;
                case Vulnerability.Neutre:
                    currentAttackValue = attaquantFM.maxAttackRange * criticalMultiplier;
                    Debug.Log("Coup neutre infligé !");
                    break;
                case Vulnerability.Resistant:
                    Debug.Log("Valeur d'attaque avant modification de critique : " + (attaquantFM.maxAttackRange - vulnerabilityMultiplier));
                    currentAttackValue = (attaquantFM.maxAttackRange - vulnerabilityMultiplier) * criticalMultiplier;
                    Debug.Log("Coup puissant infligé, valeur d'attaque après modification de critique : " + currentAttackValue);
                    break;
                default:
                    break;
            }

            currentAttackValue = Mathf.RoundToInt(currentAttackValue);

            if (currentAttackValue <= 0)
            {
                currentAttackValue = 1;
            }

            defenseurFM.currentHP -= (int)currentAttackValue;

            if (defenseurFM.currentHP <= 0)
            {
                defenseurFM.currentHP = 0;
            }

            yield return new WaitForSeconds(.7f);

            EffectManager.Instance.coupCritiqueTextEffect.Play();


            switch (attaquantFM.fideleClasse)
            {
                case Classes.Epeiste:

                    yield return new WaitForSeconds(.5f);

                    StartCoroutine(EffectManager.Instance.PlayEpeisteCriticalEffect());

                    yield return new WaitForSeconds(.4f);

                    myAnim.SetTrigger("DefenseurReceiveDamage");
                    myAnim.SetTrigger("EPLaunchCoupCritique");

                    yield return new WaitForSeconds(.5f);

                    break;
                case Classes.Magicien:

                    yield return new WaitForSeconds(.5f);

                    myAnim.SetTrigger("MALaunchCoupCritique");
                    StartCoroutine(EffectManager.Instance.PlayMagicianCriticalEffect());

                    yield return new WaitForSeconds(.4f);

                    myAnim.SetTrigger("DefenseurReceiveDamage");

                    yield return new WaitForSeconds(6f);

                    break;
                case Classes.Lancier:

                    yield return new WaitForSeconds(.5f);

                    StartCoroutine(EffectManager.Instance.PlayLancierCriticalEffect());

                    yield return new WaitForSeconds(.4f);

                    myAnim.SetTrigger("DefenseurReceiveDamage");
                    myAnim.SetTrigger("LALaunchCoupCritique");

                    yield return new WaitForSeconds(3.5f);

                    break;
                default:
                    break;
            }

            // ICI jouer VFX de coup critiique
            // ICI jouer SFX de coup critique

            //myDamageFeedback.text = "-" + (attaquantFM.maxAttackRange * 2).ToString() + (" !!");
            //attaquantDamageEffect.Play();


            // ICI jouer Anim dégâts reçus sur defenseur

            yield return new WaitForSeconds(.5f);

            renderTextCombat.text = "- " + currentAttackValue.ToString();
            defenseurAM.ReceiveDamage();
            defenseurAM.FillAmountHealth();

            attaquantBrasImage.fillAmount = attaquantFM.currentHP * 1f / attaquantFM.maxHp * 1f;
            defenseurBrasImage.fillAmount = defenseurFM.currentHP * 1f / defenseurFM.maxHp * 1f;


            myAnim.SetTrigger("PanneauTrigger");

            yield return new WaitForSeconds(1.5f);

            defenseurDamagePS.Play();
            defenseurHP.text = defenseurFM.currentHP.ToString();
            CheckHP();

            yield return new WaitForSeconds(1f);

            myAnim.SetTrigger("PanneauTrigger");

            if (attaquantFM != null && defenseurFM != null)
            {
                if (attaquantFM.isAlive && defenseurFM.isAlive)
                {
                    StartCoroutine(EndFightNoDead());
                }
            }
        }
        else
        {
            EndFightNoDead();
        }
    }

    public IEnumerator Missed()
    {
        if (attaquantFM != null && defenseurFM != null)
        {
            currentAttackValue = attaquantFM.maxCounterAttackRange * missMultiplier;

            currentAttackValue = Mathf.RoundToInt(currentAttackValue);

            if (currentAttackValue <= 0)
            {
                currentAttackValue = 1;
            }

            attaquantFM.currentHP -= (int)currentAttackValue;

            if (attaquantFM.currentHP <= 0)
            {
                attaquantFM.currentHP = 0;
            }

            // ICI jouer VFX d'echec critiique
            // ICI jouer SFX d'echec critique

            //myDamageFeedback.text = "-" + defenseurFM.maxCounterAttackRange.ToString() + " !!";
            //defenseurDamageEffect.Play();

            yield return new WaitForSeconds(.7f);

            EffectManager.Instance.echecTextEffect.Play();

            switch (attaquantFM.fideleClasse)
            {
                case Classes.Epeiste:

                    yield return new WaitForSeconds(.5f);

                    StartCoroutine(EffectManager.Instance.PlayEpeisteMissEffect());

                    yield return new WaitForSeconds(.4f);

                    myAnim.SetTrigger("AttaquantReceiveDamage");
                    myAnim.SetTrigger("EPLaunchEchecCritique");


                    break;
                case Classes.Magicien:

                    yield return new WaitForSeconds(.5f);

                    StartCoroutine(EffectManager.Instance.PlayMagicianMissEffect());

                    yield return new WaitForSeconds(.4f);

                    myAnim.SetTrigger("DefenseurReceiveDamage");
                    myAnim.SetTrigger("MALaunchEchecCritique");

                    yield return new WaitForSeconds(.5f);

                    break;
                case Classes.Lancier:

                    yield return new WaitForSeconds(.5f);

                    StartCoroutine(EffectManager.Instance.PlayLancierMissEffect());

                    yield return new WaitForSeconds(.4f);

                    myAnim.SetTrigger("DefenseurReceiveDamage");
                    myAnim.SetTrigger("LALaunchEchecCritique");

                    yield return new WaitForSeconds(.3f);

                    break;
                default:
                    break;
            }

            // ICI jouer Anim dégâts reçus sur attaquant

            yield return new WaitForSeconds(.5f);

            renderTextCombat.text = "- " + currentAttackValue.ToString();

            attaquantAM.ReceiveDamage();
            attaquantAM.FillAmountHealth();

            attaquantBrasImage.fillAmount = attaquantFM.currentHP * 1f / attaquantFM.maxHp * 1f;
            defenseurBrasImage.fillAmount = defenseurFM.currentHP * 1f / defenseurFM.maxHp * 1f;

            myAnim.SetTrigger("PanneauTrigger");

            yield return new WaitForSeconds(1.5f);

            attaquantDamagePS.Play();
            attaquantHP.text = attaquantFM.currentHP.ToString();
            CheckHP();

            yield return new WaitForSeconds(1f);

            myAnim.SetTrigger("PanneauTrigger");

            if (attaquantFM != null && defenseurFM != null)
            {
                if (attaquantFM.isAlive && defenseurFM.isAlive)
                {
                    StartCoroutine(EndFightNoDead());
                }
            }
        }
        else
        {
            EndFightNoDead();
        }
    }

    public IEnumerator Die(FideleManager deadFM, FideleManager winFM)
    {
        //Debug.Log("On Tue quelqu'un");
        myAnim.SetBool("OpenCombatBandeau", false);

        for (int i = 0; i < objectsToDisableWhileInFight.Count; i++)
        {
            objectsToDisableWhileInFight[i].alpha = 1;
        }

        //myDamageFeedback.text = "";
        //myDamageFeedback = null;

        if (attaquantFM.myCamp == GameCamps.Fidele)
        {
            GameManager.Instance.isGamePaused = false;
        }

        DragCamera2D.Instance.UnfollowTargetCamera();
        QuestManager.Instance.OnKillUnit(deadFM);

        if (deadFM.myCamp != GameCamps.Bandit)
        {
            deadFM.GetComponent<AnimationManager>().Dying();
            // ICI jouer Anim de mort
            // ICI jouer SFX de mort

            yield return new WaitForSeconds(1f);

            deadEffect.transform.position = deadFM.transform.position;
            deadEffect.Play();

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

            attaquantFM.GetComponentInChildren<Interaction>().OtherCampDisplayInteractionFeedbacks();
            defenseurFM.GetComponentInChildren<Interaction>().OtherCampDisplayInteractionFeedbacks();
            GameManager.Instance.ResetOpacityFeedback();

            SwitchInteractionType(deadFM);

            attaquantAM.keepInteractionDisplayed = false;
            attaquantAM.HideInteraction();

            defenseurAM.keepInteractionDisplayed = false;
            defenseurAM.HideInteraction();
        }

        mortSFX.Post(gameObject);

        isInFight = false;

        if (deadFM.myCamp == GameCamps.Fidele)
        {
            CameraZooming.Instance.ShakeScreen();
            StartCoroutine(GameManager.Instance.CheckIfPlayerLost());
        }

        yield return new WaitForSeconds(0.4f);
        
        attaquantAM.CheckActionsLeftAmout();

        attaquantFideleSprite.sprite = null;
        defenseurFideleSprite.sprite = null;

        attaquantFM = null;
        attaquantAM = null;

        defenseurFM = null;
        defenseurAM = null;

        GameManager.Instance.MoveUnit();
    }

    public IEnumerator EndFightNoDead()
    {
        DragCamera2D.Instance.UnfollowTargetCamera();

        //Debug.Log("Combat terminé");
        myAnim.SetBool("OpenCombatBandeau", false);

        for (int i = 0; i < objectsToDisableWhileInFight.Count; i++)
        {
            objectsToDisableWhileInFight[i].alpha = 1;
        }

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
        deadFM.GetComponentInChildren<Interaction>().interactionType = InteractionType.Recrutement;
        deadFM.GetComponentInChildren<Interaction>().AnimationManagerUpdateIcon();


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

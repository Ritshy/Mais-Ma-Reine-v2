using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [Header ("Movement Effects")]
    public List<ParticleSystem> launchDeplacementPS;
    public List<ParticleSystem> endDeplacementPS;

    public AK.Wwise.Event lancementDeplacementSFX;
    public AK.Wwise.Event finDeplacementSFX;


    [Header("Launching Combat Effect")]

    public ParticleSystem versusEffect;


    public ParticleSystem attackTextEffect;

    public ParticleSystem counterAttackTextEffect;

    public ParticleSystem echecTextEffect;

    public ParticleSystem coupCritiqueTextEffect;

    [Header("Epeiste Effects")]

    [Header("Critical Effects")]

    public ParticleSystem coupCritiqueSlashEffect;
    public ParticleSystem coupCritiqueImpactEffect;

    [Space]

    public AK.Wwise.Event samuraiSlashSFX;
    public AK.Wwise.Event sautArriereSFX;
    public AK.Wwise.Event explosionEnnemiSFX;

    [Header("Miss Effects")]

    public ParticleSystem echecEpeeFreineEffect;
    public ParticleSystem echecImpactEffect;

    [Space]

    public AK.Wwise.Event publicChocSFX;
    public AK.Wwise.Event degatsSubitsSFX;

    public AK.Wwise.Event freinageEpeeSFX;
    public AK.Wwise.Event lancementEpeeSFX;
    public AK.Wwise.Event virageEpeeSFX;

    [Header("Attack Effects")]

    public ParticleSystem attackEpeeSlashEffect;

    public AK.Wwise.Event attaqueSimpleImpactSFX;
    public AK.Wwise.Event attaqueSimpleSlashSFX;

    [Header("CounterAttack Effects")]

    public ParticleSystem counterAttackEpeeSlashEffect;

    [Space]

    [Header("Magician Effects")]

    [Header("Attack Effects")]

    public ParticleSystem fireballSpinningCanalisation;
    public ParticleSystem fireballInitialBall;
    public ParticleSystem fireballThrowing;

    public ParticleSystem fireballImpact;

    [Header("CounterAttack Effects")]

    public ParticleSystem counterAttackBonk;

    [Header("Critical Effects")]

    public ParticleSystem lightningFalling;
    public ParticleSystem staticElectricity;

    public ParticleSystem cloudAccumulation;

    public ParticleSystem spellCanalisation;
    public ParticleSystem electricRay;

    [Header("Miss Effects")]

    public ParticleSystem missingCanalisation;
    public ParticleSystem missBoom;
    public ParticleSystem missSmokeEffect;

    #region Singleton

    public static EffectManager Instance;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LaunchDeplacement(Transform locationToPlayEffect, bool isEffectAlreadyPlaying)
    {
        if (isEffectAlreadyPlaying == false)
        {
            lancementDeplacementSFX.Post(gameObject);

            int rndm = Random.Range(0, launchDeplacementPS.Count);
            launchDeplacementPS[rndm].transform.position = locationToPlayEffect.position;
            launchDeplacementPS[rndm].Play();

        }
        else
        {
            return;
        }
    }

    public void EndDeplacement(Transform locationToPlayEffect)
    {
        finDeplacementSFX.Post(gameObject);

        int rndm = Random.Range(0, endDeplacementPS.Count);
        endDeplacementPS[rndm].transform.position = locationToPlayEffect.position;
        endDeplacementPS[rndm].Play();
    }

    #region Combat Epeiste Effects
    public IEnumerator PlayEpeisteCriticalEffect()
    {
        //coupCritiqueTextEffect.Play();

        yield return new WaitForSeconds(.8f);

        coupCritiqueSlashEffect.Play();

        samuraiSlashSFX.Post(gameObject);

        yield return new WaitForSeconds(1f);

        sautArriereSFX.Post(gameObject);

        yield return new WaitForSeconds(.5f);

        explosionEnnemiSFX.Post(gameObject);

        yield return new WaitForSeconds(1f);

        coupCritiqueImpactEffect.Play();
        CameraZooming.Instance.ShakeScreen();
    }

    public IEnumerator PlayEpeisteMissEffect()
    {
        publicChocSFX.Post(gameObject);

        yield return new WaitForSeconds(1f);

        lancementEpeeSFX.Post(gameObject);

        yield return new WaitForSeconds(.3f);

        freinageEpeeSFX.Post(gameObject);

        yield return new WaitForSeconds(.6f);

        virageEpeeSFX.Post(gameObject);

        yield return new WaitForSeconds(.3f);

        virageEpeeSFX.Post(gameObject);

        yield return new WaitForSeconds(.3f);

        virageEpeeSFX.Post(gameObject);

        yield return new WaitForSeconds(1f);

        degatsSubitsSFX.Post(gameObject);

        yield return new WaitForSeconds(1.1f);

        echecEpeeFreineEffect.Play();

        yield return new WaitForSeconds(1.8f);

        echecImpactEffect.Play();
        CameraZooming.Instance.ShakeScreen();
    }

    public IEnumerator PlayEpeisteAttackEffect()
    {
        yield return new WaitForSeconds(1.2f);

        attackEpeeSlashEffect.Play();

        attaqueSimpleImpactSFX.Post(gameObject);
        attaqueSimpleSlashSFX.Post(gameObject);
    }

    public IEnumerator PlayEpeisteCounterAttackEffect()
    {
        //counterAttackTextEffect.Play();

        yield return new WaitForSeconds(1.7f);

        counterAttackEpeeSlashEffect.Play();

        yield return new WaitForSeconds(0.7f);

        attaqueSimpleSlashSFX.Post(gameObject);

        yield return new WaitForSeconds(.5f);

        attaqueSimpleImpactSFX.Post(gameObject);
    }
    #endregion

    #region Combat Magician Effects

    public IEnumerator PlayMagicianAttackEffect()
    {
        fireballSpinningCanalisation.Play();
        fireballInitialBall.Play();

        yield return new WaitForSeconds(2.5f);

        fireballThrowing.Play();

        yield return new WaitForSeconds(.5f);

        fireballImpact.Play();
    }

    public IEnumerator PlayMagicianCounterAttackEffect()
    {
        yield return new WaitForSeconds(.5f);

        counterAttackBonk.Play();
    }

    public IEnumerator PlayMagicianCriticalEffect()
    {
        spellCanalisation.Play();

        yield return new WaitForSeconds(2.2f);

        electricRay.Play();

        yield return new WaitForSeconds(.5f);

        cloudAccumulation.Play();

        yield return new WaitForSeconds(2f);

        lightningFalling.Play();

        yield return new WaitForSeconds(3f);

        staticElectricity.Play();
    }

    public IEnumerator PlayMagicianMissEffect()
    {
        missingCanalisation.Play();

        yield return new WaitForSeconds(2f);

        missBoom.Play();
        missSmokeEffect.Play();

        yield return new WaitForSeconds(1.2f);
    }


    #endregion
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    #region movement effects
    [Header ("Movement Effects")]
    public List<ParticleSystem> launchDeplacementPS;
    public List<ParticleSystem> endDeplacementPS;

    public AK.Wwise.Event lancementDeplacementSFX;
    public AK.Wwise.Event finDeplacementSFX;
    #endregion

    #region combat effects
    [Header("Launching Combat Effect")]

    public ParticleSystem versusEffect;


    public ParticleSystem attackTextEffect;

    public ParticleSystem counterAttackTextEffect;

    public ParticleSystem echecTextEffect;

    public ParticleSystem coupCritiqueTextEffect;

    #region epeiste effects
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

    #endregion

    [Space]

    #region magician effects
    [Header("Magician Effects")]

    [Header("Attack Effects")]

    public ParticleSystem fireballSpinningCanalisation;
    public ParticleSystem fireballInitialBall;
    public ParticleSystem fireballThrowing;

    public ParticleSystem fireballImpact;

    [Space]

    public AK.Wwise.Event canalisationBdFSFX;
    public AK.Wwise.Event mouvementBdFSFX;
    public AK.Wwise.Event impactBdFSFX;

    [Header("CounterAttack Effects")]

    public ParticleSystem counterAttackBonk;

    [Space]

    public AK.Wwise.Event bonkCounterSFX;

    [Header("Critical Effects")]

    public ParticleSystem lightningFalling;
    public ParticleSystem staticElectricity;

    public ParticleSystem cloudAccumulation;

    public ParticleSystem spellCanalisation;
    public ParticleSystem electricRay;

    [Space]

    public AK.Wwise.Event canalisationThunderSFX;
    public AK.Wwise.Event electricParalysisSFX;

    public AK.Wwise.Event cloudAccumulationSFX;
    public AK.Wwise.Event thunderPreBurstSFX;
    public AK.Wwise.Event thunderChocSFX;

    [Header("Miss Effects")]

    public ParticleSystem missingCanalisation;
    public ParticleSystem missBoom;
    public ParticleSystem missSmokeEffect;

    [Space]

    public AK.Wwise.Event canalisationFailSFX;
    public AK.Wwise.Event explosionFailSFX;

    #endregion

    #region lancier effects

    [Header ("Attack Effects")]

    public ParticleSystem lanceBonkEffect;

    [Space]

    public AK.Wwise.Event lanceBonkSFX;

    [Header ("CounterAttack Effects")]

    public AK.Wwise.Event lanceFlyingSFX;
    public AK.Wwise.Event lanceFlyingCounterHit;

    [Header("Critical Effects")]

    public ParticleSystem lanceHitImpactEffect;

    [Space]

    public AK.Wwise.Event lanceFlyHitSFX;
    public AK.Wwise.Event lanceFlySlashSFX;

    public AK.Wwise.Event lanceFlyReacteurSFX;
    public AK.Wwise.Event lanceFlyProjectionSFX;

    [Header("Miss Effects")]

    public ParticleSystem lanceSkyStarEffect;

    [Space]

    public AK.Wwise.Event lanceSkyStarSFX;
    public AK.Wwise.Event lancePlanteSFX;

    public AK.Wwise.Event lanceDecollageSFX;

    #endregion

    #endregion

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

        yield return new WaitForSeconds(1.5f);
        canalisationBdFSFX.Post(gameObject);

        yield return new WaitForSeconds(1f);

        fireballThrowing.Play();
        mouvementBdFSFX.Post(gameObject);

        yield return new WaitForSeconds(.5f);

        fireballImpact.Play();
        impactBdFSFX.Post(gameObject);
    }

    public IEnumerator PlayMagicianCounterAttackEffect()
    {
        yield return new WaitForSeconds(.2f);

        counterAttackBonk.Play();
        yield return new WaitForSeconds(.4f);
        bonkCounterSFX.Post(gameObject);
    }

    public IEnumerator PlayMagicianCriticalEffect()
    {
        spellCanalisation.Play();
        yield return new WaitForSeconds(1.4f);
        canalisationThunderSFX.Post(gameObject);

        yield return new WaitForSeconds(.2f);

        cloudAccumulationSFX.Post(gameObject);
        electricRay.Play();

        yield return new WaitForSeconds(.5f);

        cloudAccumulation.Play();

        yield return new WaitForSeconds(2f);

        lightningFalling.Play();
        thunderChocSFX.Post(gameObject);

        yield return new WaitForSeconds(.3f);

        thunderPreBurstSFX.Post(gameObject);

        yield return new WaitForSeconds(1.5f);

        staticElectricity.Play();
        electricParalysisSFX.Post(gameObject);
    }

    public IEnumerator PlayMagicianMissEffect()
    {
        missingCanalisation.Play();
        yield return new WaitForSeconds(1f);

        canalisationFailSFX.Post(gameObject);
        yield return new WaitForSeconds(.5f);

        missBoom.Play();
        missSmokeEffect.Play();

        explosionFailSFX.Post(gameObject);
    }

    #endregion

    #region Combat Lancier Effects

    public IEnumerator PlayLancierAttackEffect()
    {
        yield return new WaitForSeconds(1.1f);

        lanceBonkEffect.Play();

        lanceBonkSFX.Post(gameObject);
    }

    public IEnumerator PlayLancierCounterAttackkEffect()
    {
        yield return new WaitForSeconds(.3f);

        lanceFlyingSFX.Post(gameObject);

        yield return new WaitForSeconds(.4f);

        lanceFlyingCounterHit.Post(gameObject);
    }

    public IEnumerator PlayLancierCriticalEffect()
    {
        yield return new WaitForSeconds(.55f);

        lanceFlyReacteurSFX.Post(gameObject);

        yield return new WaitForSeconds(1f);

        //lanceFlyProjectionSFX.Post(gameObject);

        yield return new WaitForSeconds(.2f);

        lanceFlySlashSFX.Post(gameObject);
        lanceFlyHitSFX.Post(gameObject);

        yield return new WaitForSeconds(.8f);

        lanceFlySlashSFX.Post(gameObject);
        lanceFlyHitSFX.Post(gameObject);

        yield return new WaitForSeconds(.3f);

        lanceFlySlashSFX.Post(gameObject);
        lanceFlyHitSFX.Post(gameObject);
    }

    public IEnumerator PlayLancierMissEffect()
    {
        yield return new WaitForSeconds(.6f);

        lancePlanteSFX.Post(gameObject);

        yield return new WaitForSeconds(.8f);

        lanceDecollageSFX.Post(gameObject);

        yield return new WaitForSeconds(.8f);

        lanceSkyStarSFX.Post(gameObject);
        lanceSkyStarEffect.Play();
    }

    #endregion
}
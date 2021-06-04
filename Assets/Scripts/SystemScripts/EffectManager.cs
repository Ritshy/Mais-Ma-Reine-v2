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

    [Header("Critical Effects")]

    public ParticleSystem coupCritiqueSlashEffect;
    public ParticleSystem coupCritiqueImpactEffect;

    public ParticleSystem coupCritiqueTextEffect;

    [Header("Miss Effects")]

    public ParticleSystem echecEpeeFreineEffect;
    public ParticleSystem echecImpactEffect;

    public ParticleSystem echecTextEffect;

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

    public IEnumerator PlayCriticalEffects()
    {
        coupCritiqueTextEffect.Play();

        yield return new WaitForSeconds(.8f);

        coupCritiqueSlashEffect.Play();

        yield return new WaitForSeconds(2.1f);

        coupCritiqueImpactEffect.Play();
        CameraZooming.Instance.ShakeScreen();
    }

    public IEnumerator PlayMissEffects()
    {
        echecTextEffect.Play();

        yield return new WaitForSeconds(1.1f);

        echecEpeeFreineEffect.Play();

        yield return new WaitForSeconds(1.8f);

        echecImpactEffect.Play();
        CameraZooming.Instance.ShakeScreen();
    }
}

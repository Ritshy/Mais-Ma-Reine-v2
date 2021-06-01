using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public List<ParticleSystem> launchDeplacementSystem;

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

    public void LaunchDeplacementSystem(Transform locationToPlayEffect, bool isEffectAlreadyPlaying)
    {
        if (isEffectAlreadyPlaying == false)
        {
            int rndm = Random.Range(0, launchDeplacementSystem.Count);
            launchDeplacementSystem[rndm].transform.position = locationToPlayEffect.position;
            launchDeplacementSystem[rndm].Play();

        }
        else
        {
            return;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Recrutement : MonoBehaviour
{
    private FideleManager myFM;
    private MovementEnemy myMovementEnemy;

    [Serializable]
    public struct RecrutementSprites
    {
        public Sprite idleSprite;
        public Sprite movingSprite;
    }

    public List<RecrutementSprites> myRecruitedSprites;

    // Start is called before the first frame update
    void Start()
    {
        myFM = GetComponentInParent<FideleManager>();
        myMovementEnemy = GetComponentInParent<MovementEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LaunchRecruitement(FideleManager fmToRecruit, FideleManager recruiterFM)
    {
        GameManager.Instance.isGamePaused = true;

        int idleSpriteIndex = UnityEngine.Random.Range(0, myRecruitedSprites.Count);
        RecrutementManager.Instance.OpenRecruitementWindow(fmToRecruit, myRecruitedSprites[idleSpriteIndex].idleSprite, myRecruitedSprites[idleSpriteIndex].movingSprite, recruiterFM);
    }
}

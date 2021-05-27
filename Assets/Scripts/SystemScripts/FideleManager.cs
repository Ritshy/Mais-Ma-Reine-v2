using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class FideleManager : MonoBehaviour
{
    public GameCamps myCamp;

    [Header("Caractéristiques")]
    public string fideleNom;
    public string fidelePrenom;

    public int maxHp;
    public int currentHP;
    public bool isAlive;

    public float moveZoneValue;
    
    public int maxAttackRange;
    public int minAttackRange;

    public int maxCounterAttackRange;
    public int minCounterAttackRange;

    public bool isInteracting;

    [Range(0, 100)]
    public int criticChances;
    [Range(0, 100)]
    public int missChances;

    [Range(0, 100)]
    public int charismaCost;

    public enum FidelePeuple {Humain, Gnome, Golem, Elfe, Animal}
    public FidelePeuple fidelePeuple;

    public SpriteRenderer currentFideleSprite;
    public Sprite movingFideleSprite;
    public Sprite idleFideleSprite;
    public SpriteRenderer questIcon;

    //[HideInInspector]
    public FideleManager myTarget;

    public bool isAllActionsDone = false;

    private List<GameCamps> attackableUnits = new List<GameCamps>();


    private Interaction myInteraction;
    
    public List<FideleManager> unitsInRange = new List<FideleManager>();
    private List<FideleManager> attackableUnitsInRange = new List<FideleManager>();
    private List<FideleManager> attackableUnitsNotInRange = new List<FideleManager>();

    void Start()
    {
        myInteraction = GetComponentInChildren<Interaction>();
        GameManager.Instance.AddAMapUnit(this);
        SetAttackableUnits();
        currentHP = maxHp;
        isAlive = true;
    }

    private void Update()
    {
        if (GameManager.Instance.currentCampTurn == GameCamps.Fidele)
        {
            foreach (FideleManager uir in unitsInRange)
            {
                AnimationManager uirAM = GetComponent<AnimationManager>();
                //uirAM.DisplayInteraction();
            }
        }
    }

    private void SetAttackableUnits()
    {
        switch (myCamp)
        {
            case GameCamps.Roi:
                attackableUnits.Add(GameCamps.Fidele);
                attackableUnits.Add(GameCamps.Calamite);
                break;
            case GameCamps.Bandit:
                attackableUnits.Add(GameCamps.Fidele);
                attackableUnits.Add(GameCamps.Roi);
                attackableUnits.Add(GameCamps.Converti);
                break;
            case GameCamps.BanditCalamiteux:
                attackableUnits.Add(GameCamps.Fidele);
                attackableUnits.Add(GameCamps.Roi);
                attackableUnits.Add(GameCamps.Converti);
                break;
            case GameCamps.Calamite:
                attackableUnits.Add(GameCamps.Fidele);
                attackableUnits.Add(GameCamps.Roi);
                attackableUnits.Add(GameCamps.Villageois);
                attackableUnits.Add(GameCamps.Converti);
                attackableUnits.Add(GameCamps.Bandit);
                break;
            case GameCamps.Villageois:
                break;
            case GameCamps.Converti:
                attackableUnits.Add(GameCamps.Calamite);
                attackableUnits.Add(GameCamps.BanditCalamiteux);
                break;
            default:
                break;
        }
    }

    public void SetToConverti()
    {
        myCamp = GameCamps.Converti;
    }

    public Transform DetermineMyTarget()
    {
        if (unitsInRange.Count > 0)
        {
            UpdateAttackableUnitInRange();
            if (attackableUnitsInRange.Count == 1)
            {
                myTarget = attackableUnitsInRange[0];
                return myTarget.transform;
            }
            else if (attackableUnitsInRange.Count > 1)
            {
                myTarget = GetInRangeUnitWithSmallestHp();
                return myTarget.transform;
            }
            else if (attackableUnitsInRange.Count == 0)
            {
                attackableUnitsNotInRange = AttackableUnitsNotInRangeAmount();
                myTarget = GetClosestUnitNotInRange();
                return myTarget.transform;
            }
        }
        else if (unitsInRange.Count == 0)
        {
            attackableUnitsNotInRange = AttackableUnitsNotInRangeAmount();
            myTarget = GetClosestUnitNotInRange();
            return myTarget.transform;
        }

        return null;

        /*switch (myCamp)
        {
            case GameCamps.Roi:
                break;
            case GameCamps.Bandit:
                break;
            case GameCamps.Calamite:
                break;
            case GameCamps.Converti:
                break;
            default:
                break;
        }*/
    }

    public void AddUnitInRange (FideleManager uir)
    {
        if (attackableUnitsNotInRange.Contains(uir))
        {
            attackableUnitsNotInRange.Remove(uir);
        }
        unitsInRange.Add(uir);
    }

    public void UpdateAttackableUnitInRange ()
    {
        attackableUnitsInRange = AttackableUnitsInRangeAmount();
    }

    public void RemoveUnitInRange(Collider2D cir)
    {
        FideleManager tmpFM = cir.GetComponentInParent<FideleManager>();
        if (tmpFM)
        {
            if (unitsInRange.Contains(tmpFM))
            {
                if (attackableUnitsInRange.Contains(tmpFM))
                {
                    attackableUnitsInRange.Remove(tmpFM);
                }
                unitsInRange.Remove(tmpFM);
                tmpFM.GetComponent<AnimationManager>().HideInteraction();
                if (tmpFM == myTarget)
                {
                    myTarget = null;
                }
            }
        }
    }

    public void KillUnit (FideleManager deadUnit)
    {
        if (attackableUnitsNotInRange.Contains(deadUnit))
        {
            attackableUnitsNotInRange.Remove(deadUnit);
        }

        if (attackableUnitsInRange.Contains(deadUnit))
        {
            attackableUnitsInRange.Remove(deadUnit);
        }

        if (unitsInRange.Contains(deadUnit))
        {
            unitsInRange.Remove(deadUnit);
        }
    }

    public List<FideleManager> AttackableUnitsInRangeAmount()
    {
        List<FideleManager> tmpauir = new List<FideleManager>();
        foreach (FideleManager fm in unitsInRange)
        {
            if (attackableUnits.Contains(fm.myCamp))
            {
                tmpauir.Add(fm);
            }
        }
        return tmpauir;
    }

    private List<FideleManager> AttackableUnitsNotInRangeAmount()
    {
        List<FideleManager> tmpaunir = new List<FideleManager>();
        foreach (FideleManager fm in GameManager.Instance.GetAllMapUnits())
        {
            if (fm == this)
            {
                continue;
            }
            if (attackableUnits.Contains(fm.myCamp))
            {
                tmpaunir.Add(fm);
            }
        }
        return tmpaunir;
    }

    private FideleManager GetInRangeUnitWithSmallestHp()
    {
        float tmpSmallHp = 9999f;
        FideleManager tmpFm = attackableUnitsInRange[0];

        foreach (FideleManager fm in attackableUnitsInRange)
        {
            if (fm.currentHP < tmpSmallHp)
            {
                tmpSmallHp = fm.currentHP;
                tmpFm = fm;
            }
        }
        return tmpFm;
    }

    private FideleManager GetClosestUnitNotInRange()
    {
        float tmpDist = 9999f;
        FideleManager tmpFm = new FideleManager();

        foreach (FideleManager fm in attackableUnitsNotInRange)
        {
            float cDist = Vector2.Distance(transform.position, fm.transform.position);
            if (cDist < tmpDist)
            {
                tmpDist = cDist;
                tmpFm = fm;
            }
        }
        return tmpFm;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatRandomizer : MonoBehaviour
{
    [Header("Randomizer Settings")]

    [Range(1, 50)]
    public int minRandomHp;
    [Range(1, 50)]
    public int maxRandomHp;

    [Range(1, 20)]
    public int minRandomLowAttackRange;
    [Range(1, 20)]
    public int maxRandomLowAttackRange;
    [Range(1, 20)]
    public int minRandomHighAttackRange;
    [Range(1, 20)]
    public int maxRandomHighAttackRange;

    [Range(1, 20)]
    public int minRandomLowCounterAttackRange;
    [Range(1, 20)]
    public int maxRandomLowCounterAttackRange;
    [Range(1, 20)]
    public int minRandomHighCounterAttackRange;
    [Range(1, 20)]
    public int maxRandomHighCounterAttackRange;

    [Range(0, 100)]
    public int minRandomCriticChances;
    [Range(0, 100)]
    public int maxRandomCriticChances;

    [Range(0, 100)]
    public int minRandomMissChances;
    [Range(0, 100)]
    public int maxRandomMissChances;

    [Range(0, 100)]
    public int minRandomCharismaCost;
    [Range(0, 100)]
    public int maxRandomCharismaCost;

    [Space]
    [Header("Randomizer Results")]
    
    public int finalHP;

    public int finalMinAtkRange;
    public int finalMaxAtkRange;

    public int finalMinCounterAtkRange;
    public int finalMaxCounterAtkRange;

    public int finalCritChance;
    public int finalMissChance;

    public int finalCharismaCost;

    private FideleManager myFM;

    // Start is called before the first frame update
    void Start()
    {
        myFM = GetComponent<FideleManager>();
        if (myFM.isStatRandomize)
        {
            GenerateStatistics(myFM);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateStatistics(FideleManager receivingStatFM)
    {
        finalHP = RandomizeStat(minRandomHp, maxRandomHp);

        finalMinAtkRange = RandomizeStat(minRandomLowAttackRange, maxRandomLowAttackRange);
        finalMaxAtkRange = RandomizeStat(minRandomHighAttackRange, maxRandomHighAttackRange);

        finalMinCounterAtkRange = RandomizeStat(minRandomLowCounterAttackRange, maxRandomLowCounterAttackRange);
        finalMaxCounterAtkRange = RandomizeStat(minRandomHighCounterAttackRange, maxRandomHighCounterAttackRange);

        finalCritChance = RandomizeStat(minRandomCriticChances, maxRandomCriticChances);
        finalMissChance = RandomizeStat(minRandomMissChances, maxRandomMissChances);

        finalCharismaCost = RandomizeStat(minRandomCharismaCost, maxRandomCharismaCost);
        
        receivingStatFM.maxHp = finalHP;
        receivingStatFM.minAttackRange = finalMinAtkRange;
        receivingStatFM.maxAttackRange = finalMaxAtkRange;
        receivingStatFM.minCounterAttackRange = finalMinCounterAtkRange;
        receivingStatFM.maxCounterAttackRange = finalMaxCounterAtkRange;
        receivingStatFM.criticChances = finalCritChance;
        receivingStatFM.missChances = finalMissChance;
        receivingStatFM.charismaCost = finalCharismaCost;

        receivingStatFM.currentHP = receivingStatFM.maxHp;
    }

    public int RandomizeStat(int minimumStatValue, int maximumStatValue)
    {
        int finalStatValue = Random.Range(minimumStatValue, maximumStatValue);
        return finalStatValue;
    }
}

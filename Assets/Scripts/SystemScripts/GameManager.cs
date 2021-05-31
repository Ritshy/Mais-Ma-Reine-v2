using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public enum GameCamps { Fidele, Roi, Bandit, BanditCalamiteux, Calamite, Villageois, Converti}

public class GameManager : MonoBehaviour
{
    public GameCamps currentCampTurn;

    public List<GameCamps> campsInTerritoire;

    public float timeValue;
    public bool canMoveEnemy = true; //Public pour playtest

    public List<FideleManager> allMapUnits = new List<FideleManager>();

    public int charismeAmount;

    private int territoryBeginCharisme;

    public bool isGamePaused;

    static public int charismeAmountStatic = 15;

    static public bool isTerritoire01Completed;

    [Header ("Tuto")]

    public bool isMapTuto;

    public FideleManager firstFideleToMove;
    public bool firstFideleToMoveHasMoved;

    public FideleManager firstFideleToInteractWith;
    public bool firstFideleToInteractWithHasInteracted;

    #region Singleton
    public static GameManager Instance;

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

        #endregion
    }

    // Start is called before the first frame update
    void Start()
    {
        isGamePaused = true;
        charismeAmount = charismeAmountStatic;

        territoryBeginCharisme = charismeAmount;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMapTuto)
        {
            if (firstFideleToMoveHasMoved)
            {
                if (firstFideleToInteractWithHasInteracted == false)
                {
                    firstFideleToInteractWith.GetComponent<AnimationManager>().ActivateCursorIndicator();
                }
            }

            if (firstFideleToMoveHasMoved == false && firstFideleToMove.GetComponentInChildren<Movement>().isMoving == false)
            {
                firstFideleToMove.GetComponent<AnimationManager>().ActivateCursorIndicator();
            }
        }
    }

    public void GlobalActionsCheck()
    {
        for (int i = 0; i < allMapUnits.Count; i++)
        {
            allMapUnits[i].GetComponent<AnimationManager>().CheckActionsLeftAmout();
        }
    }

    public void IsAllCampActionsDone()
    {
        for (int i = 0; i < allMapUnits.Count; i++)
        {
            if (allMapUnits[i].myCamp == currentCampTurn)
            {
                if (allMapUnits[i].isAllActionsDone == false)
                {
                    return;
                }                
            }
        }
        SwitchTurn();
    }

    public void LoadCharismeValueBetweenScenes()
    {
        charismeAmountStatic = charismeAmount;
    }

    public void AddCharismeValue(int addedCharismeValue)
    {
        charismeAmount += addedCharismeValue;

        if (charismeAmount <= 0)
        {
            charismeAmount = 0;
        }

        if (addedCharismeValue > 0)
        {
            StartCoroutine(RecrutementManager.Instance.AddCharismeAmount(addedCharismeValue));
        }
    }

    public void LowerCharismeValue(int lowerCharismeValue)
    {
        charismeAmount -= lowerCharismeValue;

        if (charismeAmount <= 0)
        {
            charismeAmount = 0;
        }

        if (lowerCharismeValue > 0)
        {
            StartCoroutine(RecrutementManager.Instance.LowerCharismeAmount(lowerCharismeValue));
        }
    }

    public void PlayerSwitchTurn()
    {
        if (isGamePaused == false && currentCampTurn == GameCamps.Fidele)
        {
            if (isMapTuto == false)
            {
                SwitchTurn();
            }
            else
            {
                if (firstFideleToInteractWithHasInteracted)
                {
                    SwitchTurn();
                }
            }
        }
    }

    public void SwitchTurn()
    {
        for (int i = 0; i < allMapUnits.Count; i++)
        {
            if (allMapUnits[i].isAllActionsDone == true)
            {
                allMapUnits[i].isAllActionsDone = false;
            }
        }

        if ((int)currentCampTurn+1 < campsInTerritoire.Count())
        {
            currentCampTurn++;
        }
        else if ((int)currentCampTurn+1 >= campsInTerritoire.Count())
        {
            currentCampTurn = 0;
        }

        List<FideleManager> cclfm = new List<FideleManager>();

        if (currentCampTurn == GameCamps.Bandit || currentCampTurn == GameCamps.BanditCalamiteux || currentCampTurn == GameCamps.Roi || currentCampTurn == GameCamps.Fidele)
        {
            foreach (FideleManager fmocc in allMapUnits)
            {
                if (fmocc.myCamp == currentCampTurn && fmocc.GetComponentInChildren<Interaction>().interactionType != InteractionType.Recrutement)
                {
                    cclfm.Add(fmocc);
                }
            }

            if (cclfm.Count == 0)
            {
                SwitchTurn();
            }
        }
        else
        {
            SwitchTurn();
        }

        ResetTurn();
        TurnFeedbackManager.Instance.SwitchTurnFeedback(currentCampTurn);

        if (currentCampTurn == GameCamps.Fidele)
        {
            isGamePaused = false;
            RaycastInteraction.Instance.ResetLauncherInteraction();


            foreach (FideleManager fm in allMapUnits)
            {
                if (fm.myCamp == GameCamps.Fidele)
                {
                    fm.GetComponentInChildren<Movement>().hasMoved = false;
                    fm.GetComponent<AnimationManager>().InteractionAvaibleColor();
                    fm.GetComponentInChildren<Interaction>().FideleDisplayInteractionFeedbacks();
                }
                fm.GetComponentInChildren<Interaction>().OtherCampDisplayInteractionFeedbacks();
            }
            for (int i = allMapUnits.Count-1; i > 0; i--)
            {
                if (allMapUnits[i].myCamp == GameCamps.Fidele)
                {
                    StartCoroutine(DragCamera2D.Instance.FollowTargetCamera(allMapUnits[i].gameObject));
                    return;
                }
            }
        }
        else
        {
            isGamePaused = true;

            foreach (FideleManager fm in allMapUnits)
            {
                fm.GetComponent<AnimationManager>().HideInteractionIcon();
                if (fm.myCamp == currentCampTurn)
                {
                    fm.GetComponentInChildren<MovementEnemy>().hasMoved = false;
                    fm.hasFought = false;
                }
            }
            MoveUnit();
        }
    }

    public void AddAMapUnit(FideleManager newUnit)
    {
        if (!allMapUnits.Contains(newUnit))
        {
            allMapUnits.Add(newUnit);
        }
    }

    public void RemoveAMapUnit(FideleManager removeUnit)
    {
        if (allMapUnits.Contains(removeUnit))
        {
            allMapUnits.Remove(removeUnit);
        }
    }

    public List<FideleManager> GetAllMapUnits()
    {
        return allMapUnits;
    }

    public void CheckIfTurnIsEnded()
    {
        /*foreach (FideleManager fmpc in allMapUnits)
        {
            if (fmpc.myCamp == currentCampTurn && fmpc.myCamp != GameCamps.Fidele)
            {
                if (fmpc.GetComponentInChildren<MovementEnemy>().hasMoved)
                {

                }
            }
        }*/
    }

    public void ResetTurn()
    {
        foreach (FideleManager fm in allMapUnits)
        {
            fm.GetComponentInChildren<Interaction>().alreadyInteractedList.Clear();
        }
    }

    public void MoveUnit()
    {
        for (int i = 0; i < allMapUnits.Count; i++)
        {
            if (allMapUnits[i].myCamp == currentCampTurn && allMapUnits[i].myCamp != GameCamps.Fidele)
            {
                if (allMapUnits[i].GetComponentInChildren<MovementEnemy>() != null)
                {
                    if (allMapUnits[i].waitForUnitToEnterZone)
                    {
                        if (allMapUnits[i].isAlive && allMapUnits[i].GetComponentInChildren<MovementEnemy>().hasMoved == false)
                        {
                            allMapUnits[i].GetComponentInChildren<MovementEnemy>().MoveToTarget();
                        }
                    }
                    else if (allMapUnits[i].GetComponentInChildren<MovementEnemy>().hasMoved == false && allMapUnits[i].isAlive)
                    {
                        allMapUnits[i].GetComponentInChildren<MovementEnemy>().MoveToTarget();
                        return;
                    }
                }
            }
        }
    }

    public IEnumerator MoveBandit()
    {
        for (int i = 0; i < allMapUnits.Count; i++)
        {
            if (allMapUnits[i].myCamp == GameCamps.Bandit)
            {
                allMapUnits[i].GetComponentInChildren<MovementEnemy>().MoveToTarget();
                yield return new WaitForSeconds(timeValue);
            }
        }
    }

    public IEnumerator MoveCalamite()
    {
        for (int i = 0; i < allMapUnits.Count; i++)
        {
            if (allMapUnits[i].myCamp == GameCamps.Calamite)
            {
                allMapUnits[i].GetComponentInChildren<MovementEnemy>().MoveToTarget();
                yield return new WaitForSeconds(timeValue);
            }
        }
    }    

    public void LowerOpacityFeedback()
    {
        foreach (FideleManager fmu in allMapUnits)
        {
            if (fmu.isInteracting == false)
            {
                fmu.GetComponent<AnimationManager>().LowerOpacity();
            }
        }
    }

    public void ResetOpacityFeedback()
    {
        foreach (FideleManager fmu in allMapUnits)
        {
            if (fmu.isInteracting == false)
            {
                fmu.GetComponent<AnimationManager>().ResetOpacity();
            }
        }
    }

    public IEnumerator CheckIfPlayerLost()
    {
        for (int i = 0; i < allMapUnits.Count; i++)
        {
            if (allMapUnits[i].myCamp == GameCamps.Fidele)
            {
                yield break;
            }
        }
        DialogueManager.Instance.myAnim.SetTrigger("triggerDefaiteFin");

        yield return new WaitForSeconds(2f);

        charismeAmountStatic = territoryBeginCharisme;

        SceneSwitcher.Instance.SwitchToMenuTerritoire();
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum territoireMarkers { Territoire01, territoire02 }

public class CinematicDialogue : MonoBehaviour
{
    public territoireMarkers currentTerritoire;

    public TextMeshProUGUI characterName;
    public TextMeshProUGUI characterLines;

    private bool lineIsDisplayed;

    [Header("Dialogue")]

    public List<Dialogue> dialogueScenario;

    public bool isStartDialogueExisting;

    private Dialogue currentDialogue;

    private Animator myAnim;
    public Animator levelDesignAnimator;

    private Queue<string> lines;

    public Image firstCharacter;
    public Image secondCharacter;

    #region Singleton
    public static CinematicDialogue Instance;

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
        lines = new Queue<string>();
        myAnim = GetComponent<Animator>();

        OpenDialogueWindow(dialogueScenario[0]);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(currentDialogue.name);
    }

    public void OpenDialogueWindow(Dialogue dialogue)
    {
        myAnim.SetBool("isTextDisplayed", true);
        currentDialogue = dialogue;

        characterName.text = currentDialogue.characterName;

        if (currentDialogue.characterName == "La Reine")
        {
            firstCharacter.color = new Color(1, 1, 1, 1);

            secondCharacter.color = new Color(.5f, .5f, .5f, 1);
        }
        else if(currentDialogue.characterName == "Elya")
        {
            secondCharacter.color = new Color(1, 1, 1, 1);

            firstCharacter.color = new Color(.5f, .5f, .5f, 1);
        }

        lines.Clear();

        foreach (string line in currentDialogue.myDialogue)
        {
            lines.Enqueue(line);
        }

        StartCoroutine(DisplayNextLine());
    }

    public void PlayCoroutineDisplayNextLine()
    {
        if (lineIsDisplayed)
        {
            StartCoroutine(DisplayNextLine());
        }
    }

    public IEnumerator DisplayNextLine()
    {
        lineIsDisplayed = false;
        myAnim.SetBool("isTextDisplayed", false);

        if (lines.Count == 0)
        {
            if (currentDialogue.isStartingADialogue == true)
            {
                OpenDialogueWindow(dialogueScenario[currentDialogue.dialogueIndexToStart]);
            }
            else
            {
                yield return new WaitForSeconds(.5f);
                characterLines.text = "";
                characterName.text = "";
                StartCoroutine(EndDialogue());
            }
        }
        else
        {
            yield return new WaitForSeconds(.5f);
            myAnim.SetBool("isTextDisplayed", true);
            lineIsDisplayed = true;

            string line = lines.Dequeue();

            characterLines.text = line;
            
            // ICI animation de texte qui s'affiche
            // ICI jouer SFX de dialogue qui passe à l'étape suivante
        }
    }

    public IEnumerator EndDialogue()
    {
        //Afficher bouton pour lancer le territoire

        myAnim.SetTrigger("triggerBoiteDialogueDisparition");
        myAnim.SetTrigger("triggerEndCinematic");
        levelDesignAnimator.SetTrigger("triggerEndCinematicLD");

        yield return new WaitForSeconds(2.3f);

        switch (currentTerritoire)
        {
            case territoireMarkers.Territoire01:
                SceneSwitcher.Instance.SwitchToTerritoire01();
                break;
            case territoireMarkers.territoire02:
                SceneSwitcher.Instance.SwitchToTerritoire02();
                break;
            default:
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI characterLines;

    public Animator mapTransitionAnim;

    [Header("Dialogue début territoire")]

    public List<Dialogue> dialogueScenario;

    public bool isStartDialogueExisting;

    private Dialogue currentDialogue;

    public Animator myAnim;
    private FideleManager talkingFM;

    private Queue<string> lines;

    public Image myTalkingCharacter;

    public Sprite elyaSprite;

    public bool isInDialogue = false;

    public AK.Wwise.Event territoireWinTrompettesfx;
    public AK.Wwise.Event territoireWinConfetisfx;

    public AK.Wwise.Event territoireLooseSFX;

    private bool isTypingLine;

    #region Singleton
    public static DialogueManager Instance;

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

        if (isStartDialogueExisting)
        {
            OpenDialogueWindow(dialogueScenario[0], null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(2))
        {
            myAnim.SetTrigger("triggerDefaiteFin");
        }
    }

    public void OpenDialogueWindow(Dialogue dialogue, FideleManager talkedFM)
    {
        isInDialogue = true;

        talkingFM = null;
        currentDialogue = null;

        if (talkedFM != null)
        {
            talkingFM = talkedFM;
        }

        GameManager.Instance.isGamePaused = true;
        myAnim.SetBool("isOpen", true);

        currentDialogue = dialogue;
        characterName.text = currentDialogue.characterName;

        if (currentDialogue.characterName == "Elya")
        {
            myTalkingCharacter.color = new Color(1, 1, 1, 1);
            myTalkingCharacter.sprite = elyaSprite;
        }
        else
        {
            myTalkingCharacter.color = new Color(0, 0, 0, 0);
            myTalkingCharacter.sprite = null;
        }

        if (currentDialogue.cameraStartPos != null)
        {
            StartCoroutine(DragCamera2D.Instance.FollowTargetCamera(currentDialogue.cameraStartPos));
        }

        lines.Clear();

        foreach (string line in currentDialogue.myDialogue)
        {
            lines.Enqueue(line);
        }

        if (currentDialogue.particleToPlay != null)
        {
            currentDialogue.particleToPlay.Play();
        }

        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        currentDialogue.characterVoiceSFX.Stop(gameObject);

        if (lines.Count == 0)
        {
            StartCoroutine(EndDialogue());
            return;
        }
        else
        {
            // ICI jouer SFX de dialogue qui passe à l'étape suivante
            currentDialogue.characterVoiceSFX.Post(gameObject);
        }

        string line = lines.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeLines(line));
    }

    IEnumerator TypeLines(string line)
    {
        characterLines.text = "";
        foreach (char letter  in line.ToCharArray())
        {
            characterLines.text += letter;
            yield return null;
        }
    }

    public IEnumerator EndDialogue()
    {
        // ICI jouer SFX de fin de dialogue
        GameManager.Instance.isGamePaused = false;
        myAnim.SetBool("isOpen", false);

        if (currentDialogue.cameraEndPos != null)
        {
            StartCoroutine(DragCamera2D.Instance.FollowTargetCamera(currentDialogue.cameraEndPos));
        }

        if (talkingFM != null)
        {
            if (currentDialogue.nextInteractionType != InteractionType.Aucun)
            {
                talkingFM.GetComponentInChildren<Interaction>().interactionType = currentDialogue.nextInteractionType;
                talkingFM.GetComponentInChildren<Interaction>().AnimationManagerUpdateIcon();
            }
        }

        if (currentDialogue.isStartingQuest)
        {
            yield return new WaitForSeconds(.2f);

            for (int i = 0; i < currentDialogue.questIndexToStart.Count; i++)
            {
                QuestManager.Instance.SetupQuest(currentDialogue.questIndexToStart[i]);
            }
        }

        if (currentDialogue.unitsToSpawn.Count >= 1)
        {
            foreach (FideleManager fmToSpawn in currentDialogue.unitsToSpawn)
            {
                fmToSpawn.gameObject.SetActive(true);
            }
        }

        if (currentDialogue.isStartingADialogue)
        {
            yield return new WaitForSeconds(.2f);
            OpenDialogueWindow(dialogueScenario[currentDialogue.dialogueIndexToStart], null);
        }
        else if (currentDialogue.isPlayingFinTerritoireAnimation)
        {
            GameManager.Instance.isGamePaused = true;

            yield return new WaitForSeconds(.3f);
            myAnim.SetTrigger("triggerTerritoireFin");

            yield return new WaitForSeconds(4f);
            mapTransitionAnim.SetTrigger("triggerMapSwitch");

            yield return new WaitForSeconds(1f);
            GameManager.Instance.isGamePaused = false;
            SceneSwitcher.Instance.SwitchToMenuTerritoire();
        }
        else
        {
            talkingFM = null;
            currentDialogue = null;
        }
        isInDialogue = false;
    }

    public IEnumerator PlayWinSFX()
    {
        territoireWinConfetisfx.Post(gameObject);

        yield return new WaitForSeconds(1.5f);

        territoireWinTrompettesfx.Post(gameObject);
    }

    public void PlayLooseSFX()
    {
        territoireLooseSFX.Post(gameObject);
    }
}

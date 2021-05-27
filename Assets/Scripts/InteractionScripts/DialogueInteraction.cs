using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteraction : MonoBehaviour
{
    public Dialogue myDialogue;

    private bool hasTalked;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Mettre tous les scripts d'interaction sur tous les personnages, l'interaction jouée est celle définie dans le script Interaction
        //Dialogue : Liste de String qui incrémente à chaque lancement de dialogue pour avancer. Il n'est pas possible de relire
        //A la fin d'une interaction, pouvoir choisir si l'interaction mets une nouvelle interaction
    }

    public void StartDialogue(FideleManager thisFM)
    {
        if (hasTalked == false)
        {
            DialogueManager.Instance.OpenDialogueWindow(myDialogue, thisFM);
            QuestManager.Instance.OnTalkedUnit(thisFM);
        }
        else
        {
            return;
        }
    }
}

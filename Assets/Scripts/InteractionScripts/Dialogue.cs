using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string characterName;
    [TextArea]
    public string[] myDialogue;

    public bool isStartingQuest;
    public List<int> questIndexToStart;

    public bool isStartingADialogue;
    public int dialogueIndexToStart;

    public ParticleSystem particleToPlay;

    public AK.Wwise.Event characterVoiceSFX;

    public bool isPlayingFinTerritoireAnimation;

    public InteractionType nextInteractionType;

    public GameObject cameraStartPos;
    public GameObject cameraEndPos;

    public List<FideleManager> unitsToSpawn;
}

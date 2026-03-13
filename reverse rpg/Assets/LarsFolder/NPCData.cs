using UnityEngine;

public class NPCData : MonoBehaviour
{
    public string npcName;
    
    [Header("Initial Dialogue")]
    [TextArea(3, 10)]
    public string[] dialoguePages; 

    [Header("Reward Dialogue")]
    [TextArea(3, 10)]
    public string[] rewardDialoguePages; // NEW: The dialogue after quest is done

    [Header("Quest Settings")]
    public string questName;
    public string questTask;
    public int questAmount;
    public string requiredTag;

    [Header("Completion Reward")]
    public GameObject objectToSpawn; 
}
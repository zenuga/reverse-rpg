using UnityEngine;

public class NPCData : MonoBehaviour
{
    // 1. This creates a text box in the Unity Inspector
    public string npcName;

    // 2. This creates a LIST (Array) of text boxes in the Inspector
    [TextArea(3, 10)]
    public string[] dialoguePages; 
}
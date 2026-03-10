using UnityEngine;
using TMPro; // Needed for TextMeshPro

public class TestUIRaycast : MonoBehaviour
{
    public DialogueManager dialogueManager; 
    public TextMeshProUGUI npcNameLabel; // Drag your UI Name Text here!
    public float interactDistance = 5f;

    void Update()
{
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit hit;

    if (Physics.Raycast(ray, out hit, interactDistance))
    {
        NPCData hitNPC = hit.collider.GetComponentInParent<NPCData>();

        // ONLY update the name if the Dialogue UI is CLOSED
        // If the UI is open, we want the name to STAY on the person we clicked
        if (dialogueManager.mainFrame.activeSelf == false)
        {
            if (hitNPC != null)
            {
                npcNameLabel.text = hitNPC.npcName;
            }
            else
            {
                npcNameLabel.text = ""; // Clear name when looking at ground/walls
            }
        }

        // The Interaction logic
        if (hitNPC != null && Input.GetKeyDown(KeyCode.E))
        {
            // Lock the name in by opening the dialogue
            npcNameLabel.text = hitNPC.npcName; 
            dialogueManager.OpenDialogue(hitNPC); 
        }
    }
}
}
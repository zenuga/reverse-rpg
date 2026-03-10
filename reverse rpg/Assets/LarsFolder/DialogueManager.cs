using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI textLabel;
    public TextMeshProUGUI npcNameLabel;
    public GameObject activeQuests;
    public Button nextButton;
    public Button acceptButton;
    public Button exitButton;
    public GameObject mainFrame;

    public int currentPage = 0;
    
    // This stores the data of the NPC we are currently talking to
    public NPCData activeNPC;

    void Start()
    {
        nextButton.onClick.AddListener(OnNextButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
        acceptButton.onClick.AddListener(onAcceptButtonClicked);
        
        // Hide UI and buttons at start
        mainFrame.SetActive(false);
        acceptButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
    }

    // This is the "Bridge" function the Raycast will call
    // This MUST be 'public' so the Raycast script can "see" it
public void OpenDialogue(NPCData npc) 
{
    activeNPC = npc; 
    currentPage = 0;
    mainFrame.SetActive(true);
    
    // Reset buttons for the new conversation
    nextButton.gameObject.SetActive(true);
    acceptButton.gameObject.SetActive(false);
    exitButton.gameObject.SetActive(false);
}

    void Update()
    {
        // If no NPC is selected, don't do anything
        if (activeNPC == null) return;

        // Automatically pull the text from the NPC's list based on the page number
        if (currentPage < activeNPC.dialoguePages.Length)
        {
            textLabel.text = activeNPC.dialoguePages[currentPage];
        }

        // Check if we are on the LAST page of that specific NPC's dialogue
        if (currentPage == activeNPC.dialoguePages.Length - 1)
        {
            nextButton.gameObject.SetActive(false);
            acceptButton.gameObject.SetActive(true);
            exitButton.gameObject.SetActive(true);
        }
    }

    void OnNextButtonClicked()
    {
        // Only increment if there is another page to go to
        if (activeNPC != null && currentPage < activeNPC.dialoguePages.Length - 1)
        {
            currentPage++;
        }
    }

    void OnExitButtonClicked()
    {
        activeNPC = null; 

        // NOW this line will work because we defined it at the top!
        if (npcNameLabel != null)
        {
            npcNameLabel.text = ""; 
        }

        mainFrame.SetActive(false);
    }

    void onAcceptButtonClicked()
    {
        activeNPC = null; 

        // NOW this line will work because we defined it at the top!
        if (npcNameLabel != null)
        {
            npcNameLabel.text = ""; 
        }

        mainFrame.SetActive(false);
        activeQuests.SetActive(true);
    }
}
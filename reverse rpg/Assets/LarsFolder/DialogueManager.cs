using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI textLabel;
    public TextMeshProUGUI npcNameLabel;
    public GameObject activeQuests;
    public QuestControl questControl;
    public Button nextButton;
    public Button acceptButton;
    public Button exitButton;
    public GameObject mainFrame;

    public int currentPage = 0;
    public NPCData activeNPC;
    private bool isShowingRewardDialogue = false; // Tracks which list we are using

    void Start()
    {
        nextButton.onClick.AddListener(OnNextButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
        acceptButton.onClick.AddListener(onAcceptButtonClicked);
        
        mainFrame.SetActive(false);
        acceptButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
    }

    public void OpenDialogue(NPCData npc) 
    {
        activeNPC = npc; 
        currentPage = 0;
        mainFrame.SetActive(true);

        if (npcNameLabel != null) npcNameLabel.text = npc.npcName;

        // CHECK: Is the quest finished?
        if (questControl != null && questControl.IsQuestDone(npc.requiredTag))
        {
            isShowingRewardDialogue = true;
        }
        else 
        {
            isShowingRewardDialogue = false;
        }

        UpdateDialogueUI();
    }

    void UpdateDialogueUI()
    {
        if (activeNPC == null) return;

        string[] currentPages = isShowingRewardDialogue ? activeNPC.rewardDialoguePages : activeNPC.dialoguePages;

        if (currentPage < currentPages.Length)
        {
            textLabel.text = currentPages[currentPage];
        }

        // Handle Button Visibility
        if (currentPage == currentPages.Length - 1)
        {
            nextButton.gameObject.SetActive(false);
            
            if (isShowingRewardDialogue)
            {
                // If it's the reward dialogue, just show exit (the "Collect" happens)
                exitButton.gameObject.SetActive(true);
                acceptButton.gameObject.SetActive(false);
            }
            else
            {
                // If it's the first time, show accept
                acceptButton.gameObject.SetActive(true);
                exitButton.gameObject.SetActive(true);
            }
        }
        else
        {
            nextButton.gameObject.SetActive(true);
            acceptButton.gameObject.SetActive(false);
            exitButton.gameObject.SetActive(false);
        }
    }

    void OnNextButtonClicked()
    {
        string[] currentPages = isShowingRewardDialogue ? activeNPC.rewardDialoguePages : activeNPC.dialoguePages;
        
        if (activeNPC != null && currentPage < currentPages.Length - 1)
        {
            currentPage++;
            UpdateDialogueUI();
        }
    }

    void OnExitButtonClicked()
    {
        activeNPC = null; 
        mainFrame.SetActive(false);
    }

    void onAcceptButtonClicked()
    {
        if (activeNPC != null && questControl != null)
        {
            questControl.AcceptQuest(activeNPC);
        }
        activeNPC = null; 
        mainFrame.SetActive(false);
        if (activeQuests != null) activeQuests.SetActive(true);
    }
}
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic; // Added for List

public class QuestControl : MonoBehaviour
{
    [System.Serializable]
    public class QuestSlot
    {
        public GameObject frame;
        public TextMeshProUGUI nameUI;
        public TextMeshProUGUI taskUI;
        public TextMeshProUGUI amountUI;

        [HideInInspector] public bool isActive = false;
        [HideInInspector] public int currentAmount;
        [HideInInspector] public int goalAmount;
        [HideInInspector] public string targetTag; 
        [HideInInspector] public GameObject rewardObject; 
    }

    public QuestSlot slot1;
    public QuestSlot slot2;
    public QuestSlot slot3;

    // NEW: List to remember completed quest tags so NPCs can reward you
    private List<string> completedQuests = new List<string>();

    public void AcceptQuest(NPCData npc)
    {
        if (npc == null || completedQuests.Contains(npc.requiredTag)) return;
        QuestSlot targetSlot = GetEmptySlot();

        if (targetSlot != null)
        {
            targetSlot.isActive = true;
            targetSlot.frame.SetActive(true);
            targetSlot.currentAmount = 0;
            targetSlot.goalAmount = npc.questAmount;
            targetSlot.targetTag = npc.requiredTag; 
            targetSlot.rewardObject = npc.objectToSpawn; 

            targetSlot.nameUI.text = npc.questName;
            targetSlot.taskUI.text = npc.questTask;
            UpdateAmountDisplay(targetSlot);
        }
    }

    public void ProcessInteraction(QuestItem itemScript)
    {
        QuestSlot matchingQuest = FindQuestByTag(itemScript.gameObject.tag);

        if (matchingQuest != null)
        {
            matchingQuest.currentAmount++;
            UpdateAmountDisplay(matchingQuest);
            
            if (itemScript.type == QuestItem.InteractionType.Item)
                itemScript.gameObject.SetActive(false);

            if (matchingQuest.currentAmount >= matchingQuest.goalAmount)
            {
                matchingQuest.amountUI.text = "Done!";
                if (matchingQuest.rewardObject != null) matchingQuest.rewardObject.SetActive(true);
                
                // Add to completed list before clearing UI
                if(!completedQuests.Contains(matchingQuest.targetTag))
                    completedQuests.Add(matchingQuest.targetTag);

                StartCoroutine(WaitAndClear(matchingQuest));
            }
        }
    }

    IEnumerator WaitAndClear(QuestSlot slot)
    {
        yield return new WaitForSeconds(1.0f);
        slot.frame.SetActive(false);
        slot.isActive = false;
        slot.nameUI.text = "";
        slot.taskUI.text = "";
        slot.amountUI.text = "";
    }

    public bool IsQuestDone(string tagToCheck)
    {
        return completedQuests.Contains(tagToCheck);
    }

    void UpdateAmountDisplay(QuestSlot slot) => slot.amountUI.text = slot.currentAmount + " / " + slot.goalAmount;

    QuestSlot FindQuestByTag(string tagToFind)
    {
        if (slot1.isActive && slot1.targetTag == tagToFind) return slot1;
        if (slot2.isActive && slot2.targetTag == tagToFind) return slot2;
        if (slot3.isActive && slot3.targetTag == tagToFind) return slot3;
        return null;
    }

    QuestSlot GetEmptySlot()
    {
        if (!slot1.isActive) return slot1;
        if (!slot2.isActive) return slot2;
        if (!slot3.isActive) return slot3;
        return null;
    }
}
using UnityEngine;

public class QuestItem : MonoBehaviour
{
    public enum InteractionType { Item, Button }
    public InteractionType type;
    public QuestControl questControl;

    private bool wasUsed = false;

    void OnMouseDown()
    {
        if (wasUsed) return;

        if (questControl != null)
        {
            // If it's a button, we "use" it. Items get deactivated anyway.
            if (type == InteractionType.Button) wasUsed = true;
            
            questControl.ProcessInteraction(this);
        }
    }
}
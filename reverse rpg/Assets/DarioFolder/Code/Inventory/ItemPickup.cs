using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemScript itemData;
        private void OnValidate()
    {
        if (itemData != null && GetComponent<Collider>() == null)
        {
            //Debug.LogWarning("ItemPickup needs a Collider component!");
        }
    }
}

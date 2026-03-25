using System.Collections.Generic;
using UnityEngine;

public class LootDropper : MonoBehaviour
{
    public List<LootItem> lootTable = new List<LootItem>();

    public void DropLoot()
    {
        float totalWeight = 0f;

        // tel alle kansen op
        foreach (LootItem item in lootTable)
        {
            totalWeight += item.dropChance;
        }

        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        // kies 1 item
        foreach (LootItem item in lootTable)
        {
            cumulative += item.dropChance;

            if (roll <= cumulative)
            {
                Instantiate(item.itemPrefab, transform.position, Quaternion.identity);
                Debug.Log("Dropped: " + item.itemName);
                return;
            }
        }
    }
}
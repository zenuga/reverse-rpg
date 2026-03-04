using UnityEngine;

public class ExpGiver : MonoBehaviour
{
    public int expAmount = 25;

    public void GiveExp(PlayerStats player)
    {
        player.AddExp(expAmount);
    }

    // Voor test: geeft exp wanneer object wordt vernietigd
    void OnDestroy()
    {
        PlayerStats player = FindObjectOfType<PlayerStats>();

        if (player != null)
        {
            player.AddExp(expAmount);
        }
    }
}
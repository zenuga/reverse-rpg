using UnityEngine;

public class ExpGiver : MonoBehaviour
{
    public PlayerStats player;
    public int expAmount = 25;

    void OnDestroy()
    {
        if (player != null)
        {
            player.AddExp(expAmount);
        }
    }
}
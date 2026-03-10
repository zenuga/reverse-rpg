using UnityEngine;
using UnityEngine.UI;

public class MainUi : MonoBehaviour
{
public Text money;
public Text health;
public PlayerStats playerStats;
    void Update()
    {
        //money.text = "Money: " + playerStats.Money.ToString();  remove the  // after code is made
        //health.text = "Health: " + playerStats.Health.ToString();
    }
}

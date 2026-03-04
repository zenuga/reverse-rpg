using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI textLabel;
    public Button nextButton;
    public Button acceptButton;
    public Button exitButton;

    public int currentPage = 0;
    private int maxPages = 5; // Adjust this based on how many pages you have

    void Start()
    {
        // Link the button click to our custom function
        nextButton.onClick.AddListener(OnNextButtonClicked);
    }

    void Update()
    {
        switch (currentPage)
        {
        case 0:
            textLabel.text = "Hello Traveler! I have a quest for you!";
            break;
        case 1:
            textLabel.text = "The quest requires you to defeat a mighty boss, are you sure you can handle this quest?";
            break;
        case 2:
            textLabel.text = "Perfect! Let me inform you about the weakness of the boss then!";
            break;
        case 3:
            textLabel.text = "The boss is extremely slow, so if you were to run around him and keep hitting him from the back, you can get much damage in without taking damage yourself!";
            break;
        case 4:
            textLabel.text = "That is probaly all you need to defeat the Skeleton king, goodluck traveler! Come back to me when your done!";
            break;
        default:
            textLabel.text = "The End.";
            break;
        }
    }

    // This runs ONCE every time the button is clicked
    void OnNextButtonClicked()
    {
        if (currentPage < maxPages)
        {
            currentPage++; // Add 1 to the current page
        }
        else 
        {
            Debug.Log("End of dialogue reached!");
        }
    }
}
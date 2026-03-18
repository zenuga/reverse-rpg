using UnityEngine;

public class IsTalking : StateMachineBehaviour
{
    public bool isTalking = false;
    private Animator animator;



    public void StartTalking()
    {
        isTalking = true;
        if (animator != null)
        {
            animator.speed = 0;
        }
    }

    public void StopTalking()
    {
        isTalking = false;
        if (animator != null)
        {
            animator.speed = 1;
        }
    }
}

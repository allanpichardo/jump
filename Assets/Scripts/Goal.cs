using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour
{
    public UnityEvent onGoalReached;
    private static readonly int ReachedGoal = Animator.StringToHash("reachedGoal");

    private void OnTriggerEnter(Collider other)
    {
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger(ReachedGoal);
    }

    public void OnGoalReached()
    {
        if (onGoalReached != null)
        {
            onGoalReached.Invoke();
        }
    }
    

}

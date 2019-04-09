using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour
{
    public UnityEvent onGoalReached;
    private AudioSource audioSource;
    private static readonly int ReachedGoal = Animator.StringToHash("reachedGoal");

    private void OnTriggerEnter(Collider other)
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger(ReachedGoal);
    }

    IEnumerator NotifyCallbacks()
    {
        yield return new WaitForSeconds(1.0f);
        if (onGoalReached != null)
        {
            onGoalReached.Invoke();
        }
    }

    public void OnGoalReached()
    {
        StartCoroutine(NotifyCallbacks());
    }
    

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public float titleDuration = 1.5f;
    private float accumTime;
    private bool isTitleTimeElapsed;

    public void OnLevelReset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnLevelCompleted()
    {
        
    }

    private void Update()
    {
        if(!isTitleTimeElapsed)
        {
            if (accumTime >= titleDuration)
            {
                isTitleTimeElapsed = true;
                TitleTimeHasElapsed();
            }
        }

    }

    private void TitleTimeHasElapsed()
    {
        Debug.Log("title time elapsed");
    }

}

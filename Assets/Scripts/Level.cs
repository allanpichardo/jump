using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public Text titleText;
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

            accumTime += Time.deltaTime;
        }

    }

    private void TitleTimeHasElapsed()
    {
        titleText.enabled = false;
    }

}

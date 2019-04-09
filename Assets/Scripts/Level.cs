using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public int nextScene = 1;
    public Text titleText;
    public float titleDuration = 1.5f;
    private float accumTime;
    private bool isTitleTimeElapsed;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnLevelReset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnLevelCompleted()
    {
        if (audioSource)
        {
            StartCoroutine(FadeOut(audioSource, 1.0f));
        }
        SceneManager.LoadScene(nextScene);
    }
    
    IEnumerator FadeOut (AudioSource audioSource, float FadeTime) {
        float startVolume = audioSource.volume;
 
        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
 
            yield return null;
        }
 
        audioSource.Stop ();
        audioSource.volume = startVolume;
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

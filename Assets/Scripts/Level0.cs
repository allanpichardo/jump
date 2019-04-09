using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level0 : MonoBehaviour
{
    public Text titleText;
    public float waitTime = 4.0f;

    private bool isFirstWasd = true;
    private bool isFirstSpace = true;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isFirstWasd && (Input.GetKeyDown(KeyCode.W)|| Input.GetKeyDown(KeyCode.A)|| Input.GetKeyDown(KeyCode.S)|| Input.GetKeyDown(KeyCode.D)))
        {
            StopAllCoroutines();
            StartCoroutine(ShowGuidance("Press SPACE to run"));
            isFirstWasd = false;
        }

        if(isFirstSpace && Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            StartCoroutine(ShowGuidance("The dog will turn right when it hits an obstacle"));
            isFirstSpace = false;
        }
    }

    IEnumerator ShowGuidance(string text)
    {
        titleText.text = text;
        titleText.enabled = true;
        yield return new WaitForSeconds(waitTime);
        titleText.text = "";
        titleText.enabled = false;
    }
}

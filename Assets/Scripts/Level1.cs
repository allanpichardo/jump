using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level1 : MonoBehaviour
{
    public Text titleText;
    public float waitTime = 4.0f;
    private bool isFirstTime = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && isFirstTime)
        {
            StartCoroutine(ShowGuidance("Click another tile to select the portal exit"));
            isFirstTime = false;
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

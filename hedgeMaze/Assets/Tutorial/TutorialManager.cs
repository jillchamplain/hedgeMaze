using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using DG.Tweening;

[Serializable]
public class TutorialScript
{
    public string title;
    public List<string> text;
    public float waitTimeBetweenText;
    public bool hasDisplayed;
}

public class TutorialManager : MonoBehaviour
{
    [HideInInspector] public static TutorialManager instance;
    [SerializeField] float textFadeTime;
    [SerializeField] TextMeshProUGUI tutorialTF;
    [SerializeField] List<TutorialScript> tutorialScripts = new List<TutorialScript>();
    bool displayingText = false;
    TutorialScript getScriptOfTitle(string title)
    {
        foreach(TutorialScript script in tutorialScripts)
        {
            if(script.title == title) return script;
        }

        Debug.Log("Returning null");
        return null;
    }
    private void Start()
    {
        if(instance == null)
            instance = this;
    }

    public void TutorialDisplay(string scriptTitle)
    {
        Debug.Log($"Tried to display {scriptTitle}");
        Debug.Log($"{scriptTitle} has been displayed: {getScriptOfTitle(scriptTitle).hasDisplayed}");
        if ((getScriptOfTitle(scriptTitle).hasDisplayed) || displayingText)
        {
            Debug.Log($"Tried to dsplay copy of {scriptTitle}");
            return;
        }
        tutorialTF.DOComplete();
        StartCoroutine(DisplayText(getScriptOfTitle(scriptTitle)));
    }

    IEnumerator DisplayText(TutorialScript script)
    {
        displayingText = true; 

        for(int i = 0; i < script.text.Count; i++)
        {
            tutorialTF.text = script.text[i];
            tutorialTF.DOFade(1f, textFadeTime);
            yield return new WaitForSeconds(textFadeTime);
            yield return new WaitForSeconds(script.waitTimeBetweenText);
            tutorialTF.DOFade(0f, textFadeTime);
            yield return new WaitForSeconds(textFadeTime);
        }
        displayingText = false;
        script.hasDisplayed = true;
        Debug.Log($"tutorial has been displayed {script.hasDisplayed}");

    }
}

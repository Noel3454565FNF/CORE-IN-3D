using System.Collections;
using TMPro;
using UnityEngine;

public class LineClone : MonoBehaviour
{
    public TMPro.TextMeshProUGUI textComponent;
    public string currText;
    public Color currColor = Color.white;
    public LineClone NextLine;
    public bool firstLine = false;
    public float TimeToWaitBetweenEachCharacter = 0.05f;

    private bool isBusy = false;
    private Coroutine activeCoroutine;

    public enum WhatIsMyPurpose
    {
        PassTheButter,
        ReactorSys,
        Mainframe,
    }

    private void Start()
    {
        if (textComponent == null)
        {
            textComponent = gameObject.GetComponent<TextMeshProUGUI>();
        }
        if (textComponent != null)
            textComponent.text = string.Empty;

        if (firstLine)
        {
            EntryPoint("WELCOME TO THE UNDERGROUND :D", Color.white);
        }
    }

    public void EntryPoint(string text, Color textColor)
    {
        if (isBusy)
            return;

        // Pass the current text and color to the next line if available
        NextLine?.EntryPoint(currText, currColor);

        // Start text rendering
        currText = text;
        currColor = textColor;

        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);

        activeCoroutine = StartCoroutine(RegisterText(text, textColor));
    }

    private IEnumerator RegisterText(string text, Color textColor)
    {
        isBusy = true;
        textComponent.text = string.Empty;
        textComponent.color = textColor;

        if (firstLine)
        {
            string tempText = string.Empty;
            foreach (char c in text)
            {
                tempText += c;
                textComponent.text = tempText;
                yield return new WaitForSeconds(TimeToWaitBetweenEachCharacter);
            }
        }
        else
        {
            textComponent.text = text;
        }

        isBusy = false;
    }
}

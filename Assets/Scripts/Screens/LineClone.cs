using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using TMPro;
using UnityEngine;

public class LineClone : MonoBehaviour
{
    public TMPro.TextMeshProUGUI textComponent;
    public TMPro.TMP_InputField textComponentALT;
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

    public enum TextType
    {
        TMPUGUI,
        TMPInputField
    }
    public TextType tt = new TextType();

    private void Start()
    {
        StartCoroutine(Starrt());
    }

    IEnumerator Starrt()
    {
        yield return new WaitForSeconds(2f);
        if (textComponent == null)
        {
            textComponent = gameObject.GetComponent<TextMeshProUGUI>();
            if (textComponent == null)
            {
                textComponentALT = gameObject.GetComponent<TMP_InputField>();
            }
        }
        if (textComponent != null)
            textComponent.text = string.Empty;

        if (firstLine)
        {
            EntryPoint("", Color.white);
        }

    }

    public void EntryPoint(string text, Color textColor)
    {
        if (isBusy)
        {
            return;
        }

        if (tt == TextType.TMPUGUI && textComponent == null)
        {
            return;
        }

        if (tt == TextType.TMPInputField && textComponent == null)
        {
            return;
        }

        // Pass the current text and color to the next line if available
        NextLine?.EntryPoint(currText, currColor);

        // Start text rendering
        currText = text;
        currColor = textColor;

        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);

        if (tt == TextType.TMPUGUI)
        {
            activeCoroutine = StartCoroutine(RegisterText(text, textColor));
        }
        else if(tt == TextType.TMPInputField)
        {
            activeCoroutine = StartCoroutine(RegisterTextALT(text, textColor));
        }
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

    private IEnumerator RegisterTextALT(string text, Color textColor)
    {
        isBusy = true;
        textComponentALT.text = string.Empty;
        textComponentALT.textComponent.color = textColor;

        if (firstLine)
        {
            string tempText = string.Empty;
            foreach (char c in text)
            {
                tempText += c;
                textComponentALT.text = tempText;
                yield return new WaitForSeconds(TimeToWaitBetweenEachCharacter);
            }
        }
        else
        {
            textComponentALT.text = text;
        }

        isBusy = false;
    }

}

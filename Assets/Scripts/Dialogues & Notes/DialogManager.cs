using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using UnityEditor.PackageManager;

public class DialogManager : MonoBehaviour
{
    [Header("GameObject")]
    public GameObject dialogPanel;
    public TMP_Text dialogText;
    public Button nextButton;

    [Header("Attributes")]
    [SerializeField] private float wordSpeed;
    [SerializeField] private float delayBeforeClosed;
    private Queue<string> dialogLines;
    private bool isTyping = false;
    private string currentLine;
    private Coroutine typingCoroutine;

    [Header("Events")]
    private Action onDialogFinished;

    void Start()
    {
        dialogLines = new Queue<string>();
        dialogPanel.SetActive(false);
    }

    public void StartDialog(string[] lines, Action callback, bool autoClose)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        dialogPanel.SetActive(true);
        dialogLines.Clear();
        onDialogFinished = callback;

        foreach (string line in lines)
        {
            dialogLines.Enqueue(line);
        }

        DisplayNextLine(autoClose);
    }

    public void DisplayNextLine(bool autoClose)
    {
        if (isTyping) return;
        
        if (dialogLines.Count == 0)
        {
            EndDialog();
            return;
        }

        currentLine = dialogLines.Dequeue();
        typingCoroutine = StartCoroutine(TypeLine(currentLine, autoClose));
    }

    IEnumerator TypeLine(string line, bool autoClose)
    {
        isTyping = true;
        dialogText.text = "";
        if(nextButton != null) nextButton.interactable = false;
        foreach (char c in line.ToCharArray())
        {
            dialogText.text += c;
            yield return new WaitForSeconds(wordSpeed);
        }
        isTyping = false;
        
        if (dialogLines.Count == 0 && autoClose)
        {
            if(nextButton != null) nextButton.interactable = true;
            yield return new WaitForSeconds(delayBeforeClosed); // Delay 2 detik
            EndDialog();
        }
        else
        {
            if(nextButton != null) nextButton.interactable = true;
        }
    }

    void EndDialog()
    {
        if(nextButton != null) nextButton.interactable = false;

        dialogPanel.SetActive(false);
        onDialogFinished?.Invoke();
    }
}

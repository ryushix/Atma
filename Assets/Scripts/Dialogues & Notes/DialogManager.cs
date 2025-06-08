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
    public GameObject dialogPanel;
    public TMP_Text dialogText;
    public GameObject charHead;
    public Button nextButton;

    public float wordSpeed;
    private Queue<string> dialogLines;
    private bool isTyping = false;
    private string currentLine;
    private Action onDialogFinished;

    public UnityEvent EndofDialog;

    void Start()
    {
        dialogLines = new Queue<string>();
        dialogPanel.SetActive(false);
        nextButton.onClick.AddListener(DisplayNextLine);
    }

    public void StartDialog(string[] lines, Action callback)
    {
        dialogPanel.SetActive(true);
        dialogLines.Clear();
        onDialogFinished = callback;

        foreach (string line in lines)
        {
            dialogLines.Enqueue(line);
        }

        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (isTyping) return;
        
        if (dialogLines.Count == 0)
        {
            EndDialog();
            EndofDialog.Invoke();
            return;
        }

        currentLine = dialogLines.Dequeue();
        StartCoroutine(TypeLine(currentLine));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogText.text = "";
        nextButton.interactable = false;
        foreach (char c in line.ToCharArray())
        {
            dialogText.text += c;
            yield return new WaitForSeconds(wordSpeed);
        }
        isTyping = false;
        nextButton.interactable = true;
    }

    void EndDialog()
    {
        dialogPanel.SetActive(false);
        onDialogFinished?.Invoke();
    }
}

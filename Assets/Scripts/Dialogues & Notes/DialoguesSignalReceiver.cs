using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

public class DialogSignalReceiver : MonoBehaviour
{
    public DialogManager dialogManager;
    public PlayableDirector director;
    public bool autoClose;
    public enum DialogMode { Manual, AutoClose };
    public DialogMode dialogMode;
    public string[] dialog;

    [Header("Events")]
    public UnityEvent onDialogComplete;

    void Start()
    {
        autoClose = (dialogMode == DialogMode.AutoClose);
    }

    void Update()
    {
        if (dialogManager.nextButton != null)
        {
            if (dialogManager.nextButton.interactable == true && Input.GetKeyDown(KeyCode.Space))
            {
                
                dialogManager.DisplayNextLine(autoClose);
            }
        }
    }

    public void TriggerDialog()
    {
        if (director != null)
        {
            director.Pause(); // Stop Timeline sementara
        }
        dialogManager.StartDialog(dialog, OnDialogFinished, autoClose);
    }

    public void OnDialogFinished()
    {
        onDialogComplete?.Invoke();
        if (director != null) director.Resume(); // Lanjutkan Timeline
    }
}

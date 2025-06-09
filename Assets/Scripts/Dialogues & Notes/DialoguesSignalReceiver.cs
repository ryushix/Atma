using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

public class DialogSignalReceiver : MonoBehaviour
{
    public DialogManager dialogManager;
    public PlayableDirector director;
    public enum DialogMode {Manual, AutoClose};
    public DialogMode dialogMode;
    public string[] dialog;

    [Header("Events")]
    public UnityEvent onDialogComplete;

    public void TriggerDialog()
    {
        if (director != null)
        {
            director.Pause(); // Stop Timeline sementara
        }
        bool autoClose = (dialogMode == DialogMode.AutoClose);
        dialogManager.StartDialog(dialog, OnDialogFinished, autoClose);
    }

    void OnDialogFinished()
    {
        onDialogComplete?.Invoke();
        if (director != null) director.Resume(); // Lanjutkan Timeline
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogSignalReceiver : MonoBehaviour
{
    public DialogManager dialogManager;
    public PlayableDirector director;
    public string[] dialog;

    public void TriggerDialog()
    {
        if (director != null)
        {
            director.Pause(); // Stop Timeline sementara
        }
        dialogManager.StartDialog(dialog, OnDialogFinished);
    }

    void OnDialogFinished()
    {
        if (director != null) director.Resume(); // Lanjutkan Timeline
    }
}

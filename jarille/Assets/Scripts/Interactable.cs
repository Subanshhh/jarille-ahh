using UnityEngine;

public class Interactable : MonoBehaviour
{
    public DialogueLine[] dialogueLines;

    public void Interact()
    {
        // 🚫 Don't restart dialogue if it's already active
        if (DialogueManager.Instance.isDialogueActive) return;

        DialogueManager.Instance.StartDialogue(dialogueLines);
    }
}
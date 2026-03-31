using UnityEngine;

public class Interactable : MonoBehaviour
{
    public DialogueLine[] dialogueLines;

    public void Interact()
    {
        
        if (DialogueManager.Instance.isDialogueActive) return;

        DialogueManager.Instance.StartDialogue(dialogueLines);
    }
}
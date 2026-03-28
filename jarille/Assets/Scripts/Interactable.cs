using UnityEngine;

public class Interactable : MonoBehaviour
{
    public DialogueLine[] dialogueLines;

    public void Interact()
    {
        DialogueManager.Instance.StartDialogue(dialogueLines);
    }
}
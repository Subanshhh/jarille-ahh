using UnityEngine;

public class Interactable : MonoBehaviour
{
    [TextArea]
    public string[] dialogueLines;
    public SavePoint savePoint;

    public void Interact()
    {
        if (savePoint != null)
        {
            savePoint.Save();
            DialogueManager.Instance.StartDialogue(new string[] {
            "Game saved.",
            "Jarille is happy yayy."
        });
        }
        else
        {
            DialogueManager.Instance.StartDialogue(dialogueLines);
        }
    }

}
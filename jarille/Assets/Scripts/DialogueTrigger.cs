using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueLine[] dialogueLines;

    private bool triggered = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Something entered trigger: " + collision.name);

        if (triggered) return;

        if (collision.CompareTag("Player"))
        {
            Debug.Log("PLAYER ENTERED TRIGGER");
            triggered = true;

            DialogueManager.Instance.StartDialogue(dialogueLines, OnDialogueEnd);
        }
    }

    void OnDialogueEnd()
    {
        Destroy(gameObject);
    }
}
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueLine[] dialogueLines;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Start dialogue
            DialogueManager.Instance.StartDialogue(dialogueLines);

            // Destroy trigger immediately
            Destroy(gameObject);
        }
    }
}
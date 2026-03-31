using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueLine[] dialogueLines;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            DialogueManager.Instance.StartDialogue(dialogueLines);

            
            Destroy(gameObject);
        }
    }
}
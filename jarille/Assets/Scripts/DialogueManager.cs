using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject dialoguePanel;
    public TMP_Text dialogueText;

    private Queue<string> lines = new Queue<string>();
    private bool isTyping = false;

    void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(string[] dialogueLines)
    {
        lines.Clear();

        foreach (string line in dialogueLines)
        {
            lines.Enqueue(line);
        }

        dialoguePanel.SetActive(true);
        ShowNextLine();
    }

    public void ShowNextLine()
    {
        if (isTyping) return;

        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        string line = lines.Dequeue();
        StartCoroutine(TypeLine(line));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.02f);
        }

        isTyping = false;
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    //public TMP_Text nameText;
    public Image portraitImage;

    private Queue<DialogueLine> lines = new Queue<DialogueLine>();
    private bool isTyping = false;
    private Action onDialogueEndCallback;

    void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(DialogueLine[] dialogueLines, Action onEnd = null)
    {
        lines.Clear();
        onDialogueEndCallback = onEnd;

        foreach (DialogueLine line in dialogueLines)
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

        DialogueLine line = lines.Dequeue();

        // Update UI
        //nameText.text = line.characterName;
        portraitImage.sprite = line.characterSprite;

        StartCoroutine(TypeLine(line.text));
    }

    IEnumerator TypeLine(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.02f);
        }

        isTyping = false;
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);

        onDialogueEndCallback?.Invoke();
    }
}
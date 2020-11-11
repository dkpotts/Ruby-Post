using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

public class DialogManager : MonoBehaviour
{
    public float displayTime = 4.0f;
    public GameObject dialogBox;
    public GameObject dialogImage;

    //private string characterDialog = "Greetings Adventurer!";
    private Queue<string> sentences;
    private string characterName;

    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();

        dialogBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartDialog(CharacterDialog dialog)
    {
        if (!dialog.convoStarted)
        {
            dialog.convoStarted = true;
            sentences.Clear();
            nameText.text = dialog.characterName;

            foreach (string sentence in dialog.sentences)
            {
                sentences.Enqueue(sentence);
            }

            dialogImage.GetComponent<UnityEngine.UI.Image>().sprite = dialog.dialogSprite;

            DisplayNextSentence(dialog);
        }
        else
        {
            DisplayNextSentence(dialog);
        }
    }

    public void DisplayNextSentence(CharacterDialog dialog)
    {
        if (!dialog.convoPaused)
        {
            if (sentences.Count == 0)
            {
                EndDialog();
                dialog.convoStarted = false;
                return;
            }
            dialogBox.SetActive(true);
            string sentence = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialog()
    {
        dialogBox.SetActive(false);
    }
}

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

    private string characterName;

    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
 
    // Start is called before the first frame update
    void Start()
    {
        dialogBox.SetActive(false);
        //subscribing the ondialogupdate function here to the onRobotsFixed Action from the EventSystem
        GameEvents.current.onRobotsFixed += onDialogUpdate;
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
            nameText.text = dialog.characterName;
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
            if (dialog.sentencesIndex == dialog.sentences.Length)
            {
                EndDialog();
                dialog.convoStarted = false;
                return;
            }

            string sentence = dialog.sentences[dialog.sentencesIndex];
            dialogBox.SetActive(true);

            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
            dialog.sentencesIndex += 1;
        }
        else
        {
            dialogBox.SetActive(false);
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

    private void onDialogUpdate()
    {
        Debug.Log("you fixed the robots!");
    }
}

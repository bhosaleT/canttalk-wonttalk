using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour {

    private Queue<string> sentences;

    // Config
    //

    [SerializeField]
    TextMeshProUGUI EnemyName;

    [SerializeField]
    TextMeshProUGUI dialogueText;

    [SerializeField]
    GameObject dialogueUI;

    [SerializeField]
    MapManager mapManager;

    void Start() {
        sentences = new Queue<string>();
    }

    void EndDialogue() {
        mapManager.GoToGameScene();
    }

    IEnumerator TypeSentence(string sentence) {
        dialogueText.text = "";

        foreach(char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return null;
        }
    }

    // Public Methods
    public void StartDialogue(Dialogue dialogue) {

        // Clear previous sentences.
        sentences.Clear();

        // Turn on the gameUI.
        dialogueUI.SetActive(true);
        EnemyName.text = dialogue.name;

        // add all the sentences to Queues.
        foreach(string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence() {

        // Check if there are any sentences left.
        if(sentences.Count == 0) {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();

        StartCoroutine(TypeSentence(sentence));
    }
}
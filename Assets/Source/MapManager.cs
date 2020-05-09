using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour {

    [System.Serializable]
    public class ButtonsData {
        [SerializeField]
        public string buttonName;

        [SerializeField]
        public Button enemyButton;
    }

    [System.Serializable]
    public class DialogsData {
        [SerializeField]
        public int characterName;

        [SerializeField]
        public Dialogue dialogue;
    }

    // Config
    //

    [SerializeField]
    List<ButtonsData> buttonsDataList;

    [SerializeField]
    List<DialogsData> dialogsDataList;

    [SerializeField]
    LevelLoader levelLoader;

    [SerializeField]
    DialogueManager dialogueManager;

    [SerializeField]
    GameObject ThankYouGameObject;

    //
    // \Config

    static string currentCharacter;
    Dictionary<string, Button> buttonDataMap;
    Dictionary<int, Dialogue> dialogueDataMap;

    bool gameHasEnded = false;

    void Awake() {

        ThankYouGameObject.SetActive(false);
        gameHasEnded = false;

        buttonDataMap = new Dictionary<string, Button>();
        dialogueDataMap = new Dictionary<int, Dialogue>();

        foreach(ButtonsData data in buttonsDataList) {
            buttonDataMap.Add(data.buttonName, data.enemyButton);
            // data.enemyButton.enabled = false;
            data.enemyButton.interactable = false;
        }

        foreach(DialogsData data in dialogsDataList) {
            dialogueDataMap.Add(data.characterName, data.dialogue);
        }

        if(currentCharacter == null) {
            Debug.Log("Current character was null");
            currentCharacter = "The Dude";
        } else if(currentCharacter == "The Dude") {
            Debug.Log("Current character was the dude");
            currentCharacter = "The Twins";
        } else if(currentCharacter == "The Twins") {
            currentCharacter = "The Lady";
        } else if(currentCharacter == "The Lady") {
            ThankYouGameObject.SetActive(true);
            gameHasEnded = true;
            currentCharacter = null;
        }

        if(!gameHasEnded) {
            buttonDataMap[currentCharacter].interactable = true;
        }

    }

    // Public Methods

    public void TriggerDialogue(int _newEnemy) {
        BattleSystem.newEnemy = _newEnemy;

        Dialogue newDialogue = dialogueDataMap[_newEnemy];

        dialogueManager.StartDialogue(newDialogue);
    }

    public void GoToGameScene() {
        levelLoader.LoadMapScene("Game");
    }

}
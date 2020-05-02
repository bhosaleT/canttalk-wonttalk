using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour {

    [System.Serializable]
    public class EnemyData {

        [SerializeField]
        public string enemyName;

        [SerializeField]
        public GameObject enemyPrefab;

    }

    [System.Serializable]
    public class GameWinData {
        [SerializeField]
        public string enemyName;

        [SerializeField]
        public string winDialogue;
    }

    [System.Serializable]
    public class GameLoseData {
        [SerializeField]
        public string enemyName;

        [SerializeField]
        public string loseDialogue;
    }

    //Config
    //

    [SerializeField]
    GameObject playerPrefab;

    [SerializeField]
    List<EnemyData> enemyDataList;

    [SerializeField]
    Transform playerSpawnPoint;

    [SerializeField]
    Transform enemySpawnPoint;

    [SerializeField]
    TextMeshProUGUI dialogueText;

    [SerializeField]
    BattleHUD playerHUD;

    [SerializeField]
    BattleHUD enemyHUD;

    [SerializeField]
    List<string> enemyNames;

    [SerializeField]
    GameObject buttonsGameObject;

    [SerializeField]
    GameObject winCase;

    [SerializeField]
    GameObject lostCase;

    [SerializeField]
    GameObject gameOverGO;

    [SerializeField]
    TextMeshProUGUI winMessage;

    [SerializeField]
    TextMeshProUGUI lostMessage;

    [SerializeField]
    List<ParticleSystem> particleSystems;

    [SerializeField]
    List<GameWinData> gameWinList;

    [SerializeField]
    List<GameLoseData> gameLoseList;

    [SerializeField]
    ParticleSystem hurtEffectPlayer;

    [SerializeField]
    ParticleSystem hurtEffectEnemy;

    BattleState state;
    Dictionary<string, GameObject> enemyDataMap;
    Dictionary<string, string> gameWinMap;
    Dictionary<string, string> gameLoseMap;
    Unit player;
    Unit currentEnemy;

    public static int newEnemy;

    void Awake() {
        enemyDataMap = new Dictionary<string, GameObject>();
        gameWinMap = new Dictionary<string, string>();
        gameLoseMap = new Dictionary<string, string>();

        // Create all the data dictionaries.

        foreach(EnemyData data in enemyDataList) {
            enemyDataMap.Add(data.enemyName, data.enemyPrefab);
        }

        foreach(GameWinData data in gameWinList) {
            gameWinMap.Add(data.enemyName, data.winDialogue);
        }

        foreach(GameLoseData data in gameLoseList) {
            gameLoseMap.Add(data.enemyName, data.loseDialogue);
        }
    }

    void Start() {

        state = BattleState.START;
        // Placeholder remove this.
        int random = Random.Range(0, 3);
        StartCoroutine(SetupBattle(enemyNames[newEnemy]));
    }

    IEnumerator SetupBattle(string enemyName) {

        // Add player.
        player = Instantiate(playerPrefab, playerSpawnPoint).GetComponent<Unit>();

        // Add enemy.

        if(enemyDataMap.ContainsKey(enemyName)) {

            currentEnemy = Instantiate(enemyDataMap[enemyName], enemySpawnPoint).GetComponent<Unit>();

        } else {
            Debug.LogError("Enemy GameObject not found, MayDay!! MayDay!!");
        }

        playerHUD.SetupHUD(player);
        enemyHUD.SetupHUD(currentEnemy);

        buttonsGameObject.SetActive(false);

        dialogueText.gameObject.SetActive(true);

        string dialogue = "A wild " + currentEnemy.unitName + " approaches...";

        dialogueText.text = dialogue;

        yield return new WaitForSeconds(1f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator TypeSentence(string sentence, float howLongToWaitAfterDialogue = 1f) {
        dialogueText.text = "";

        foreach(char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void PlayerTurn() {
        StartCoroutine(HideDialogueAndShowButtons());
    }

    IEnumerator HideDialogueAndShowButtons() {
        string dialogue = "Ahh it's your turn! Here are your options:";

        dialogueText.text = dialogue;

        yield return new WaitForSeconds(1.5f);

        dialogueText.gameObject.SetActive(false);
        buttonsGameObject.SetActive(true);
    }

    IEnumerator ActuallyAttack(Unit.ActualAttackData data) {
        dialogueText.gameObject.SetActive(true);
        buttonsGameObject.SetActive(false);

        dialogueText.text = data.attackDialogue;

        yield return new WaitForSeconds(2.5f);

        string dialogue = "You did " + data.damageValue;

        dialogueText.text = dialogue;

        bool isDead = currentEnemy.TakeDamage(data.damageValue);
        enemyHUD.UpdateHUD();
        hurtEffectEnemy.Play();

        yield return new WaitForSeconds(1.5f);

        if(isDead) {
            // End the battle.
            state = BattleState.WON;
            EndBattle();
        } else {
            state = BattleState.ENEMYTURN;
            EnemyTurn();
        }
    }

    IEnumerator ActualEnemyTurn() {

        // stop particle systems from playing.
        foreach(ParticleSystem system in particleSystems) {
            system.Stop();
        }

        dialogueText.gameObject.SetActive(true);

        dialogueText.text = "It's " + currentEnemy.unitName + "'s turn";

        yield return new WaitForSeconds(2f);

        Unit.ActualAttackData data = currentEnemy.FetchAttackDataForEnemy();

        dialogueText.text = currentEnemy.unitName + " used " + data.attackDialogue;

        yield return new WaitForSeconds(1f);

        if(data.isHealer) {
            dialogueText.text = currentEnemy.unitName + " healed itself with " + data.damageValue;
        } else {
            dialogueText.text = currentEnemy.unitName + " did " + data.damageValue + " damage";
        }

        bool isDead = player.TakeDamage(data.damageValue, data.isHealer);

        hurtEffectPlayer.Play();

        playerHUD.UpdateHUD();

        yield return new WaitForSeconds(2f);

        if(isDead) {
            state = BattleState.LOST;
            EndBattle();
        } else {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }

    }

    void EnemyTurn() {
        StartCoroutine(ActualEnemyTurn());
    }

    void EndBattle() {
        gameOverGO.SetActive(true);

        if(state == BattleState.WON) {

            // Show win screen.

            string message = gameWinMap[currentEnemy.unitName];
            winMessage.text = message;
            winCase.SetActive(true);
        } else if(state == BattleState.LOST) {

            // Show lost screen.

            string message = gameLoseMap[currentEnemy.unitName];
            lostMessage.text = message;
            lostCase.SetActive(true);
        }
    }

    void ResetGame() {

    }

    // Public Methods

    public void Initialize() {}

    public void Attack(string attackID) {

        if(state != BattleState.PLAYERTURN) {
            return;
        }

        Unit.ActualAttackData data = currentEnemy.FetchAttackData(attackID);
        StartCoroutine(ActuallyAttack(data));
    }

    public void GoBackToMapScreen() {
        // Reset the game before you go.

    }

    public void Retry() {

    }

}
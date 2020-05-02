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

    BattleState state;
    Dictionary<string, GameObject> enemyDataMap;
    Dictionary<string, string> gameWinMap;
    Dictionary<string, string> gameLoseMap;
    Unit player;
    Unit currentEnemy;

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
        StartCoroutine(SetupBattle(enemyNames[random]));
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

        dialogueText.text = "A wild " + currentEnemy.unitName + " approaches...";

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn() {
        StartCoroutine(HideDialogueAndShowButtons());
    }

    IEnumerator HideDialogueAndShowButtons() {

        dialogueText.text = "Ahh it's your turn! Here are your options:";

        yield return new WaitForSeconds(1.5f);

        dialogueText.gameObject.SetActive(false);
        buttonsGameObject.SetActive(true);
    }

    IEnumerator ActuallyAttack(Unit.ActualAttackData data) {
        dialogueText.gameObject.SetActive(true);
        buttonsGameObject.SetActive(false);

        dialogueText.text = data.attackDialogue;

        yield return new WaitForSeconds(1f);

        dialogueText.text = "You did " + data.damageValue;

        bool isDead = currentEnemy.TakeDamage(data.damageValue);

        yield return new WaitForSeconds(1f);

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

        yield return new WaitForSeconds(2f);

        bool isDead = player.TakeDamage(data.damageValue, data.isHealer);

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
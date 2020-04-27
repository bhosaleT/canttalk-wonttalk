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
    TextMeshProUGUI playerName;

    [SerializeField]
    TextMeshProUGUI enemyName;

    [SerializeField]
    BattleHUD playerHUD;

    [SerializeField]
    BattleHUD enemyHUD;

    BattleState state;
    Dictionary<string, GameObject> enemyDataMap;
    Unit player;
    Unit currentEnemy;

    void Awake() {

        enemyDataMap = new Dictionary<string, GameObject>();

        foreach(EnemyData data in enemyDataList) {
            enemyDataMap.Add(data.enemyName, data.enemyPrefab);
        }
    }

    void Start() {

        state = BattleState.START;
        string enemyName = "something";
        SetupBattle(enemyName);
    }

    void SetupBattle(string enemyName) {

        // Add player.
        player = Instantiate(playerPrefab, playerSpawnPoint).GetComponent<Unit>();

        // Add enemy.

        if(enemyDataMap.ContainsKey(enemyName)) {

            currentEnemy = Instantiate(enemyDataMap[enemyName], enemySpawnPoint).GetComponent<Unit>();

        } else {
            Debug.LogError("Enemy not found");
        }

        playerHUD.SetupHUD(player);
        enemyHUD.SetupHUD(currentEnemy);
    }

    // Public Methods

    public void Initialize() {

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    [System.Serializable]
    public class ActualAttackData {
        [SerializeField]
        public string attackDialogue;

        [SerializeField]
        public int damageValue;
    }

    [System.Serializable]
    public class AttackData {
        [SerializeField]
        public string attackID;

        [SerializeField]
        public ActualAttackData attackData;
    }

    // Config
    //

    [SerializeField]
    public string unitName;

    [SerializeField]
    public int unitLevel;

    [SerializeField]
    public int damage;

    [SerializeField]
    public int maxHP;

    [SerializeField]
    public int currentHP;

    [SerializeField]
    List<AttackData> attackDataList;

    // 
    // \Config

    Dictionary<string, ActualAttackData> attackDataMap;

    // Public methods

    void Awake() {
        attackDataMap = new Dictionary<string, ActualAttackData>();

        foreach(AttackData data in attackDataList) {
            attackDataMap.Add(data.attackID, data.attackData);
        }
    }

    public bool TakeDamage(int damage) {
        currentHP -= damage;

        if(currentHP < 0) {
            return true;
        } else {
            return false;
        }
    }

    public ActualAttackData FetchAttackData(string attackID) {
        ActualAttackData data = attackDataMap[attackID];
        return data;
    }
}
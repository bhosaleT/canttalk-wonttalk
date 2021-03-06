﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    [System.Serializable]
    public class ActualAttackData {
        [SerializeField]
        public string attackDialogue;

        [SerializeField]
        public int damageValue;

        [SerializeField]
        public bool isHealer;
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

    [Header("For Enemy Only")]

    [SerializeField, Tooltip("Only for the enemy")]
    List<string> attackIDs;

    [SerializeField, Tooltip("Enemy attack data")]
    List<AttackData> enemyDataList;

    // 
    // \Config

    Dictionary<string, ActualAttackData> attackDataMap;
    Dictionary<string, ActualAttackData> enemyDataMap;

    // Public methods

    void Awake() {
        attackDataMap = new Dictionary<string, ActualAttackData>();
        enemyDataMap = new Dictionary<string, ActualAttackData>();

        foreach(AttackData data in attackDataList) {
            attackDataMap.Add(data.attackID, data.attackData);
        }

        foreach(AttackData data in enemyDataList) {
            enemyDataMap.Add(data.attackID, data.attackData);
        }
    }

    public bool TakeDamage(int damage, bool isHealer = false) {

        if(!isHealer) {
            currentHP -= damage;

        } else {
            if(currentHP != maxHP) {
                currentHP += damage;
            }
        }

        Debug.Log(unitName + currentHP);

        if(currentHP <= 0) {
            return true;
        } else {
            return false;
        }
    }

    // This is for the Player character.
    public ActualAttackData FetchAttackData(string attackID) {
        ActualAttackData data = attackDataMap[attackID];
        return data;
    }

    // This is for the Enemy.
    // Fetch one random action from the list.

    public ActualAttackData FetchAttackDataForEnemy() {
        Debug.Log("ActualAttackData was called " + attackIDs.Count);
        int attackDataIndex = Random.Range(0, attackIDs.Count);

        string attackID = attackIDs[attackDataIndex];

        ActualAttackData data = enemyDataMap[attackID];

        return data;
    }

    public void Reset() {
        currentHP = maxHP;
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

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

    // 
    // \Config

    // Public methods

    public bool TakeDamage(int damage) {
        currentHP -= damage;

        if(currentHP < 0) {
            return true;
        } else {
            return false;
        }
    }
}
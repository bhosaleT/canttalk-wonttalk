using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour {

    // Config
    //

    [SerializeField]
    TextMeshProUGUI unitName;

    [SerializeField]
    TextMeshProUGUI unitLevelText;

    [SerializeField]
    Slider hpSlider;

    Unit currentUnit;

    // Setup Hud.
    public void SetupHUD(Unit unit) {
        currentUnit = unit;

        unitName.text = currentUnit.unitName;
        unitLevelText.text = "Lvl " + currentUnit.unitLevel.ToString();

        hpSlider.maxValue = currentUnit.maxHP;
        hpSlider.value = currentUnit.currentHP;
    }

    // update hud

    public void UpdateHUD() {
        hpSlider.value = currentUnit.currentHP;
    }

}
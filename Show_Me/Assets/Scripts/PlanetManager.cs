using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlanetManager : MonoBehaviour {

    public float atmosLevel;
    public float temperature;
    public float radiation;
    public float lifeComplexity;

    [SerializeField] private TMP_Text atmosText;
    [SerializeField] private TMP_Text tempText;
    [SerializeField] private TMP_Text radText;
    [SerializeField] private TMP_Text lifeText;

    [SerializeField] private Material planetMat;
    [SerializeField] private Material oceanMat;
    [SerializeField] private Material atmosphereMat;

    public void Start() {
        UpdateValues();
    }

    public void UpdateValues() {
        atmosText.text = "O2/CO2: " + atmosLevel;
        tempText.text = "Temp: " + temperature;
        radText.text = "Rad: " + radiation;
        lifeText.text = "Life: " + lifeComplexity;

        planetMat.SetFloat("_LifeAmount", lifeComplexity/7);
        atmosphereMat.SetFloat("_TempBlend", temperature/10);
    }

}
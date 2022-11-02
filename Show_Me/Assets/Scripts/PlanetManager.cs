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

    private Material material;
    private Shader shader;

    public void Start() {
        material = GetComponent<Material>();
        shader = GetComponent<Shader>();
    }

    public void Update() {
        atmosText.text = "CO2: " + atmosLevel;
        tempText.text = "Temp: " + temperature;
        radText.text = "Rad: " + radiation;
        lifeText.text = "Life: " + lifeComplexity;
    }

}
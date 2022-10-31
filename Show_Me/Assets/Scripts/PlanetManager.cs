using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlanetManager : MonoBehaviour {

    public float oxygenLevel;
    public float carbonLevel;
    public float temperature;
    public float radiation;
    public float lifeComplexity;

    [SerializeField] private TMP_Text oxygenText;
    [SerializeField] private TMP_Text carbonText;
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
        oxygenText.text = "O2: " + oxygenLevel;
        carbonText.text = "CO2: " + carbonLevel;
        tempText.text = "Temp: " + temperature;
        radText.text = "Rad: " + radiation;
        lifeText.text = "Life: " + lifeComplexity;
    }

}
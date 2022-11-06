using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlanetManager : MonoBehaviour {

    public float atmosLevel;
    public float temperature;
    public float lifeComplexity;
    public float radiation;

    [SerializeField] private GameObject atmosSlider;
    [SerializeField] private GameObject tempSlider;
    [SerializeField] private GameObject lifeSlider;
    [SerializeField] private GameObject radSlider;

    [SerializeField] private Material planetMat;
    [SerializeField] private Material oceanMat;
    [SerializeField] private Material atmosphereMat;

    public void Start() {
        UpdateValues();
    }

    public void UpdateValues() {

        atmosSlider.transform.localScale = new Vector3(atmosLevel/2, 
                                                       atmosSlider.transform.localScale.y,
                                                       atmosSlider.transform.localScale.z);

        tempSlider.transform.localScale = new Vector3(temperature/2, 
                                                      tempSlider.transform.localScale.y,
                                                      tempSlider.transform.localScale.z);

        lifeSlider.transform.localScale = new Vector3(lifeComplexity/2, 
                                                      lifeSlider.transform.localScale.y,
                                                      lifeSlider.transform.localScale.z);

        radSlider.transform.localScale = new Vector3(radiation/2, 
                                                     radSlider.transform.localScale.y,
                                                     radSlider.transform.localScale.z);

        atmosphereMat.SetFloat("_TempBlend", temperature/10);
        planetMat.SetFloat("_LifeAmount", lifeComplexity/7);

    }

}
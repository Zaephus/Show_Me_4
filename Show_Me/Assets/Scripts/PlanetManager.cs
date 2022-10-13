using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour {

    private Material material;
    private Shader shader;

    public void Start() {
        material = GetComponent<Material>();
        shader = GetComponent<Shader>();
    }

    public void Update() {

    }

}
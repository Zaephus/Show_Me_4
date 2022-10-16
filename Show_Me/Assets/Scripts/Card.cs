using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour {

    public int index;

    [SerializeField] private TMP_Text indexText;

    [SerializeField] private Transform target;

    public void OnStart() {
        indexText.text = index.ToString();
    }

    public void OnUpdate() {

    }

}
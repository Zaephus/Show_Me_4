using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "DeckObject")]
public class DeckObject : ScriptableObject {
    public List<CardStats> cards = new List<CardStats>();
}
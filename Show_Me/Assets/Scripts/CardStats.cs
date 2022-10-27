using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "CardStats")]
public class CardStats : ScriptableObject {

    public string cardName;
    [TextArea(5,10)]
    public string cardDescription;

    public float oxygenModifier;
    public float carbonModifier;
    public float tempModifier;
    public float radModifier;
    public float lifeModifier;

}
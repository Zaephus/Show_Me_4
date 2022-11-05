using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.SceneTemplate;
using UnityEngine;

public enum mod
{
    Addition,
    ActiveMultiplication,
    passiveAddition,
    effect,
    Destroypassive

}
public enum special
{
    none,
    radiationFilter,
    AtmosphericStabilizer,
    StaticTemp,
    CO2Converter,
    ConverseThermostat,
    EnergyConversion,
    MutationVirus
}

[CreateAssetMenuAttribute(fileName = "CardStats")]
public class CardStats : ScriptableObject {

    public string cardName;
    [TextArea(5,10)]
    public string cardDescription;

    public Texture2D cardTexture;

    public float oxygenModifier;
    public float carbonModifier;
    public float tempModifier;
    public float radModifier;
    public float lifeModifier;
    public mod modification;
    public special special;
    public System.Func<(int, int, int, int, special), (int,int,int,int)> cardFunc = cardValues => SpecialCard(cardValues);

    public static (int,int,int,int) SpecialCard( (int,int,int,int, special) cardValues)
    {
        int atmos = cardValues.Item1;
        int temp = cardValues.Item2;
        int rad = cardValues.Item3;
        int life = cardValues.Item4;
        special sp = cardValues.Item5;

        switch (sp)
        {
            case special.radiationFilter:
                rad = 0;
                break;
            case special.none:
                break;
            case special.AtmosphericStabilizer:
                atmos = 0;
                break;
            case special.StaticTemp:
                temp = 0;
                break;
            case special.CO2Converter:
                atmos = math.abs(temp);
                break;
            case special.ConverseThermostat:
                temp = -temp;
                break;
            case special.EnergyConversion:
                if (rad > (10 - temp))
                {
                    rad = rad - (10 - temp);
                    temp = 10;
                }
                else
                {
                    temp = temp + rad;
                    rad = 0;
                }
                break;
            case special.MutationVirus:
                if (rad > (7 - life))
                {
                    rad = rad - (7 - life);
                    life = 7;
                }
                else
                {
                    life = life + rad;
                    rad = 0;
                }
                break;
            default:
                break;
        }

        return (atmos, temp, rad, life);
    }
}


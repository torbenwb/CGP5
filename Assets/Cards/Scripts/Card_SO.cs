using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Card_SO : ScriptableObject
{
    public string cardName;
    public string description;
    public Sprite cardImage;
    public int manaCost;

    public List<CardEffect> cardEffects;
}

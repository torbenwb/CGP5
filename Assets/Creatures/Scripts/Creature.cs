using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Creature : BoardItem
{
    [Header("UI Elements")]
    public Image creatureImage;
    public TextMeshProUGUI displayHealth;
    public TextMeshProUGUI displayStrength;

    Creature_SO creatureType;
    public Creature_SO CreatureType{get => creatureType;}

    [Header("Stats")]
    public int health;
    public int strength;

    #region Active Card Effects
    List<CardEffect> passiveEffects = new List<CardEffect>();
    List<CardEffect> onKillEffects = new List<CardEffect>();

    #endregion

    #region Interface
    public void LoadCreatureSO(Creature_SO creatureType)
    {
        this.creatureType = creatureType;
        this.health = creatureType.health;
        this.strength = creatureType.strength;

        displayHealth.text = this.health.ToString();
        displayStrength.text = this.strength.ToString();
        creatureImage.sprite = creatureType.creatureImage;
    }

    public void SetHealth(int amount){
        health = amount;
        displayHealth.text = health.ToString();
    }

    public void SetStrength(int amount){
        strength = amount;
        displayStrength.text = strength.ToString();
    }
    #endregion
}

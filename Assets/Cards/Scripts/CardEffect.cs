using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardEffect
{
    public enum Target
    {
        Player,
        Creature,
        AllCreatures
    }

    public enum Type
    {
        ChangeHealth,
        ChangeStrength,
        ChangeMana,
        DrawCard,
    }

    [SerializeField] public Target targetType;
    [SerializeField] public Type effectType;
    [SerializeField] public int strength;

    public CardEffect(Target targetType, Type effectType, int strength)
    {
        this.targetType = targetType;
        this.effectType = effectType;
        this.strength = strength;
    }

    public CardEffect(CardEffect copy)
    {
        this.targetType = copy.targetType;
        this.effectType = copy.effectType;
        this.strength = copy.strength;
    }

    public void ResolveEffect(Player player)
    {
        switch (effectType){
            case Type.ChangeHealth: player.SetHealth(player.health + strength); break;
            case Type.ChangeMana: player.SetMana(player.mana + strength); break;
            case Type.DrawCard: for(int i = 0; i < strength; i++) player.DrawCard(); break;
        }
    }

    public void ResolveEffect(Creature targetCreature)
    {
        switch (effectType){
            case Type.ChangeHealth: targetCreature.SetHealth(targetCreature.health + strength); break;
            case Type.ChangeStrength: targetCreature.SetStrength(targetCreature.strength + strength); break;
        }
    }

    public void ResolveEffect(Creature[] creatures)
    {
        foreach(Creature targetCreature in creatures)
        {
            switch (effectType){
            case Type.ChangeHealth: targetCreature.SetHealth(targetCreature.health + strength); break;
            case Type.ChangeStrength: targetCreature.SetStrength(targetCreature.strength + strength); break;
        }
        }
    }
}

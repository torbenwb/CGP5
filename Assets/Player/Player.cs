using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Board")]
    public Board hand;
    public GameObject cardPrefab;

    [Header("Cards")]
    public List<Card_SO> startingDeck = new List<Card_SO>();
    Stack<Card_SO> deck = new Stack<Card_SO>();
    List<Card_SO> discard = new List<Card_SO>();

    [Header("Stats")]
    public int health;
    public int mana;
    public int turnIndex = 0;
    public bool myTurn = false;

    #region MonoBehaviour
    void Start()
    {
        StartTurn();
    }
    #endregion
    
    #region Interface
    public void StartTurn()
    {
        turnIndex++;
        if (turnIndex == 1)
        {
            ShuffleStartingDeck();
            DrawCard();
            DrawCard();
            DrawCard();
        }
        else{
            DrawCard();
        }

        myTurn = true;
    }

    public void EndTurn()
    {
        FindObjectOfType<Enemy>().StartTurn();
        myTurn = false;
    }

    public void DrawCard()
    {
        Card_SO cardType = deck.Pop();
        hand.NewBoardItem(cardPrefab).GetComponent<Card>().LoadCardSO(cardType);
    }

    public void TryPlayCard(Card card, Creature target)
    {
        Card_SO cardType = card.CardType;
        if (mana < cardType.manaCost) return;

        mana -= cardType.manaCost;
        DiscardCard(card);

        StartCoroutine(ResolveCardEffects(cardType, target));
    }

    public void DiscardCard(Card card)
    {
        hand.RemoveBoardItem(card);
        discard.Add(card.CardType);
    }

    public void ShuffleStartingDeck()
    {
        List<Card_SO> copy = new List<Card_SO>(startingDeck);
        while(copy.Count > 0)
        {
            int randomIndex = Random.Range(0, copy.Count);
            Card_SO card = copy[randomIndex];
            copy.RemoveAt(randomIndex);
            deck.Push(card);
        }
    }

    public void SetHealth(int amount){
        health = amount;
    }

    public void SetMana(int amount){
        mana = amount;
    }
    #endregion

    #region Play Card
    IEnumerator ResolveCardEffects(Card_SO cardType, Creature targetCreature)
    {
        myTurn = false;

        foreach(CardEffect c in cardType.cardEffects)
        {
            switch(c.targetType)
            {
                case CardEffect.Target.Player:
                    c.ResolveEffect(this);
                    break;
                case CardEffect.Target.Creature:
                    c.ResolveEffect(targetCreature);
                    break;
                case CardEffect.Target.AllCreatures:
                    c.ResolveEffect(FindObjectsOfType<Creature>());
                    break;
            }
            
            yield return new WaitForSeconds(0.25f);
        }

        FindObjectOfType<Enemy>().PostCardPlayed();

        yield return new WaitForSeconds(1);
        myTurn = true;
    }
    #endregion
}

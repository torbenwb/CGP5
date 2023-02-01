using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : BoardItem
{
    Card_SO cardType;
    public Card_SO CardType { get => cardType; }

    [Header("UI Elements")]
    public TextMeshProUGUI nameDisplay;
    public TextMeshProUGUI descriptionDisplay;
    public TextMeshProUGUI manaCostDisplay;
    public Image cardImage;

    #region Interface
    public void LoadCardSO(Card_SO cardType)
    {
        this.cardType = cardType;
        nameDisplay.text = cardType.cardName;
        descriptionDisplay.text = cardType.description;
        manaCostDisplay.text = cardType.manaCost.ToString();
        cardImage.sprite = cardType.cardImage;
    }
    #endregion
}

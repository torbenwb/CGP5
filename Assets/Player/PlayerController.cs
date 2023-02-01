using UnityEngine;

/*  The PlayerController component is used to parse user input
    and do the following:
        *   Visually interact with the board including the player's
            hand and the enemy's creatures (for targeting only)
        *   Parse input and pass commands to the player to either:
            *   Try and play a specific card
            *   End current turn
    The PlayerController will only operate during the Player's turn.
*/

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Board))]
public class PlayerController : MonoBehaviour
{
    // Player to pass control input to
    Player player;
    // Enemy used to determine target creature
    Enemy enemy;
    // Player hand of cards
    Board hand;
    // Currently selected card
    Card selectedCard;
    // Currently selected creature
    Creature targetCreature;

    #region MonoBehaviour
    void Awake()
    {
        player = GetComponent<Player>();
        hand = GetComponent<Board>();
        enemy = FindObjectOfType<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.myTurn)
        {
            selectedCard = null;
            targetCreature = null;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Return)) player.EndTurn();

        hand.SetFocusTarget(hand.GetTarget(mousePosition));
        targetCreature = enemy.board.GetTarget(mousePosition) as Creature;
        enemy.board.SetFocusTarget(targetCreature);

        if (Input.GetMouseButtonDown(0)) selectedCard = hand.GetTarget(mousePosition) as Card;
        if (Input.GetMouseButtonUp(0)) TryPlaySelectedCard();

        if (selectedCard)
        {
            hand.DragTarget(selectedCard, mousePosition);
        }
    }
    #endregion

    #region Player
    // Attempt to play selected card on target creature.
    void TryPlaySelectedCard()
    {
        if (!selectedCard) return;

        player.TryPlayCard(selectedCard, targetCreature);

        selectedCard = null;
    }
    #endregion

    #region Utility
    // Get the world space position of the mouse with z locked to 0
    Vector3 mousePosition
    {
        get 
        {
            Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            p.z = 0f;
            return p;
        }
    }
    #endregion

}

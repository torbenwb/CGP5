using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Board))]
public class Enemy : MonoBehaviour
{
    public Board board;
    

    [Header("Creatures")]
    public GameObject creaturePrefab;
    public Creature_SO creatureType;
    List<Creature> creatures = new List<Creature>();

    #region MonoBehaviour
    private void Awake()
    {
        board = GetComponent<Board>();
    }

    void Start()
    {
        SpawnCreature(creatureType);
        SpawnCreature(creatureType);
        SpawnCreature(creatureType);
    }
    #endregion

    #region Actions
    public void StartTurn()
    {
        StartCoroutine(TurnSequence());
    }

    public void PostCardPlayed()
    {
        List<Creature> deadCreatures = new List<Creature>();

        foreach(Creature c in creatures) if (c.health <= 0) deadCreatures.Add(c);

        foreach(Creature c in deadCreatures)
        {
            DestroyCreature(c);
        }
    }

    void SpawnCreature(Creature_SO creatureType)
    {
        Creature newCreature = board.NewBoardItem(creaturePrefab).GetComponent<Creature>();
        newCreature.LoadCreatureSO(creatureType);
        creatures.Add(newCreature);
    }

    void DestroyCreature(Creature creature)
    {
        board.RemoveBoardItem(creature);
        creatures.Remove(creature);
        Destroy(creature.gameObject);
    }
    #endregion

    #region Turn Sequence
    IEnumerator TurnSequence()
    {
        Player player = FindObjectOfType<Player>();

        foreach(Creature creature in creatures)
        {
            creature.offset = Vector3.down;
            player.SetHealth(player.health - creature.strength);

            yield return new WaitForSeconds(0.25f);
            creature.offset = Vector3.zero;
        }
        yield return null;

        player.StartTurn();
    }
    #endregion
}

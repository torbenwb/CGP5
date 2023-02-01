using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  The Board Component is used to control the layout, position, 
    and rotation of a set of GameObjects containing a BoardItem
    component.

    Boards can use two types of layout: Horizontal or Radial. 
    Each BoardItem's position is regularly updated by the Board.

    A Board may have one focus target - a specific BoardItem receiving
    focus. The focus target will be repositioned accordingly.

    A Board may have one drag target - a specific BoardItem being dragged
    to a position outside of its layout position.
*/
[RequireComponent(typeof(BoxCollider2D))]
public class Board : MonoBehaviour
{
    public enum LayoutType{ Horizontal, Radial }
    // Area of this board - Used to detect mouse over events
    BoxCollider2D boardArea;
    // Board items managed by this board
    List<BoardItem> boardItems = new List<BoardItem>();
    // Item currently receiving focus - affects how item is positioned
    BoardItem focusTarget;
    // Item currently being dragged - affects item's offset
    BoardItem dragTarget;
    // Position to drag target to.
    Vector3 dragTargetPosition;

    [Header("Layout")]
    public LayoutType layoutType = LayoutType.Horizontal;
    [Tooltip("Width of each individual element.")]
    public float width;
    [Tooltip("Padding between each element.")]
    public float padding;
    [Tooltip("Radius used for radial layout.")]
    public float radius;
    [Tooltip("How much to offset position of focus target.")]
    public float focusOffset;
    [Tooltip("Where to spawn new Board Items in world space coordinates.")]
    public Vector3 spawnPosition;

    #region MonoBehaviour
    void Awake() => boardArea = GetComponent<BoxCollider2D>();

    void Update()
    {
        int targetIndex = boardItems.IndexOf(focusTarget);

        switch(layoutType){
            case LayoutType.Horizontal: HorizontalLayout(targetIndex); break;
            case LayoutType.Radial: RadialLayout(targetIndex); break;
        }

        if (dragTarget) dragTarget.position = dragTargetPosition;

        dragTarget = null;
        focusTarget = null;
    }

    #endregion

    #region Layout
    // Determine the horizontal layout position for each board item.
    // Offset adjacent and focus target positions.
    void HorizontalLayout(int targetIndex = -1)
    {
        int count = boardItems.Count;
        float totalWidth = (count * width) + (count - 1) * padding;
        Vector3 start = transform.position + Vector3.left * (totalWidth / 2);

        for(int i = 0; i < count; i++){
            Vector3 position = start + Vector3.right * (((float)i + 0.5f) * width + (i * padding));

            if (targetIndex != -1)
            {
                if (i == targetIndex - 1) position += Vector3.left * focusOffset;
                if (i == targetIndex + 1) position += Vector3.right * focusOffset;
                if (i == targetIndex) position += Vector3.up * focusOffset;
            }

            boardItems[i].position = position;
        }
    }
    // Determine the radial position and rotation for each board item.
    // Offset adjacent and focus target positions and rotation.
    void RadialLayout(int targetIndex = -1)
    {
        for(int i = 0; i < boardItems.Count; i++)
        {
            float angleOffset = 0f;
            float radiusOffset = 0f;

            if (targetIndex != -1)
            {
                if (i == targetIndex - 1) angleOffset = -focusOffset;
                else if (i == targetIndex + 1) angleOffset = focusOffset;
                else if (i == targetIndex) radiusOffset = focusOffset;
            }
            

            boardItems[i].position = RadialPosition(i, angleOffset, radiusOffset);
            boardItems[i].transform.rotation = RadialRotation(boardItems[i].transform.position);
        }
    }

    // Get radial layout position at index.
    // Offset along outer ring by angle offset.
    // Offset away from center by radius offset.
    Vector3 RadialPosition(int index, float angleOffset, float radiusOffset)
    {
        Vector3 origin = transform.position + Vector3.down * radius;
        float circ = Mathf.PI * 2f * radius;
        int count = boardItems.Count;
        float totalWidth = (count * width) + ((count - 1) * padding);
        float totalAngle = ((totalWidth / circ) * 360f);
        float angle = totalAngle / count;
        float startAngle = -(totalAngle / 2f);
        Vector3 startDirection = Quaternion.AngleAxis(startAngle, Vector3.forward) * Vector3.up;
        startDirection = Quaternion.AngleAxis(((float) index + 0.5f) * angle + angleOffset, Vector3.forward) * startDirection;
        return origin + startDirection * (radius + radiusOffset);
    }

    // Get board item's rotation based on relative position to 
    // circle's center.
    Quaternion RadialRotation(Vector3 position)
    {
        Vector3 origin = transform.position + Vector3.down * radius;
        return Quaternion.LookRotation(Vector3.forward, (position - origin));
    }
    #endregion

    #region General Public Interface
    public bool InBoardArea(Vector3 position) => (boardArea.OverlapPoint(position));
    public void SetFocusTarget(BoardItem focusTarget) => this.focusTarget = focusTarget;

    // Drag target to target position.
    public void DragTarget(BoardItem dragTarget, Vector3 position)
    {
        this.dragTarget = dragTarget;
        dragTargetPosition = position;
    }

    // Return nearest board item to target position.
    public BoardItem GetTarget(Vector3 position)
    {
        if (!InBoardArea(position)) return null;
        
        BoardItem target = null;
        float minDistance = 1000f;

        foreach(BoardItem b in boardItems)
        {
            float distance = (position - b.transform.position).magnitude;
            if (distance <= minDistance)
            {
                minDistance = distance;
                target = b;
            }
        }
        return target;
    }

    // Instantiate a new board item from given prefab
    public GameObject NewBoardItem(GameObject prefab)
    {
        if (!prefab) return null;

        GameObject newBoardItem = Instantiate(prefab, spawnPosition, Quaternion.identity);
        newBoardItem.transform.parent = transform;

        BoardItem boardItem = newBoardItem.GetComponent<BoardItem>();
        if (!boardItem) boardItem = newBoardItem.AddComponent<BoardItem>();

        boardItems.Add(boardItem);
        return newBoardItem;
    }

    // Destroy and remove specific board item.
    public void RemoveBoardItem(BoardItem boardItem)
    {
        if (!boardItems.Contains(boardItem)) return;
        boardItems.Remove(boardItem);
        Destroy(boardItem.gameObject);
    }

    #endregion
}

using UnityEngine;

/*  The BoardItem component regularly updates a GameObject's
    position to move towards a target position defined by 
    position and offset.

    Move speed is defined by moveSpeed and the distance to 
    the target position. GameObject will move faster when 
    farther away from target position.
*/
public class BoardItem : MonoBehaviour
{
    [Tooltip("How fast this item moves to its target position.")]
    public float moveSpeed = 15f;
    [Tooltip("Target position. Usually set by Board.")]
    public Vector3 position;
    [Tooltip("Offset from target position. Usually set by Board.")]
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = position + offset;
        float distance = (transform.position - (targetPosition)).magnitude;
        float moveDistance = moveSpeed * distance * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveDistance);
    }
}

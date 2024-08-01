using UnityEngine;

public class LadderHandler : MonoBehaviour
{
    [SerializeField] private GameObject topChild;
    [SerializeField] private GameObject bottomChild;

    private void OnValidate()
    {
        // Position the children at the top and bottom of the ladder
        PositionChildren();
    }

    private void PositionChildren()
    {
        // Get the ladder's collider (assumes the ladder has a BoxCollider2D)
        BoxCollider2D ladderCollider = GetComponent<BoxCollider2D>();

        if (ladderCollider != null)
        {
            // Calculate the top and bottom y positions
            float topY = ladderCollider.bounds.max.y;
            float bottomY = ladderCollider.bounds.min.y;

            // Set the positions of the children
            topChild.transform.position = new Vector3(transform.position.x, topY-0.1f, transform.position.z);
            bottomChild.transform.position = new Vector3(transform.position.x, bottomY+0.1f, transform.position.z);

            // Mark the scene as dirty so changes are saved
            #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(topChild);
            UnityEditor.EditorUtility.SetDirty(bottomChild);
            #endif
        }
        else
        {
            Debug.LogWarning("Ladder object does not have a BoxCollider2D.");
        }
    }
}

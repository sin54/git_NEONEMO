using UnityEngine;

public class Interactor : MonoBehaviour
{
    private IInteractable currentTarget;
    [SerializeField] private LayerMask interactLayer;
    public Transform interactorSource;
    [SerializeField] private float interactionRange;

    private void Update()
    {
        Vector2 origin = interactorSource.position;
        Vector2 direction = interactorSource.up;

        Debug.DrawRay(origin, direction * interactionRange, Color.red);

        RaycastHit2D hitInfo = Physics2D.Raycast(origin, direction, interactionRange,interactLayer);
        if (hitInfo.collider == null)
        {
            if (currentTarget != null)
            {
                currentTarget.HoverOut();
                currentTarget = null;
            }

        }
        else
        {
            if (currentTarget == null)
            {
                currentTarget = hitInfo.collider.GetComponent<IInteractable>();
                currentTarget.HoverIn();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            currentTarget.Interact();
        }
    }
    private void OnDrawGizmos()
    {
    }
}

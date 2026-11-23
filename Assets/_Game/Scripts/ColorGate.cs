using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ColorGate : MonoBehaviour
{
    [SerializeField] private CourierPlayer player;
    [SerializeField] private CourierColor requiredColor;
    [SerializeField] private Transform gatePanel;
    [SerializeField] private float openingDistance = 2.1f;
    [SerializeField] private float openingSpeed = 5f;

    private BoxCollider gateCollider;
    private Collider playerCollider;
    private Bounds doorwayBounds;
    private Vector3 closedPosition;
    private bool isOpen;

    private void Awake()
    {
        gateCollider = GetComponent<BoxCollider>();
        doorwayBounds = gateCollider.bounds;
        doorwayBounds.Expand(0.15f);
        if (player != null) playerCollider = player.GetComponent<Collider>();
        if (gatePanel != null) closedPosition = gatePanel.localPosition;
    }

    private void Update()
    {
        if (player == null) return;

        bool matchingColor = player.CurrentColor == requiredColor;
        bool playerInDoorway = playerCollider != null &&
            doorwayBounds.Intersects(playerCollider.bounds);

        isOpen = matchingColor || (isOpen && playerInDoorway);
        gateCollider.enabled = !isOpen;

        if (gatePanel != null)
        {
            Vector3 target = closedPosition;
            if (isOpen) target.y -= openingDistance;
            gatePanel.localPosition = Vector3.MoveTowards(
                gatePanel.localPosition, target, openingSpeed * Time.deltaTime);
        }
    }
}

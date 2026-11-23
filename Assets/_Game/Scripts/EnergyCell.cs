using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EnergyCell : MonoBehaviour
{
    [SerializeField] private GameSession session;
    [SerializeField] private Transform visual;
    [SerializeField] private float rotationSpeed = 45f;
    [SerializeField] private float bobHeight = 0.12f;

    private bool collected;
    private Vector3 startPosition;

    private void Awake()
    {
        GetComponent<BoxCollider>().isTrigger = true;
        if (visual != null) startPosition = visual.localPosition;
    }

    private void Update()
    {
        if (visual == null || collected) return;
        visual.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
        visual.localPosition = startPosition +
            Vector3.up * Mathf.Sin(Time.time * 2f) * bobHeight;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected || session == null || session.IsComplete || session.IsPaused) return;
        if (other.GetComponentInParent<CourierPlayer>() == null) return;

        collected = true;
        session.CollectCell();
        gameObject.SetActive(false);
    }
}

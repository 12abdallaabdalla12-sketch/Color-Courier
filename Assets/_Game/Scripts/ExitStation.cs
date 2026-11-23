using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(MeshRenderer))]
public class ExitStation : MonoBehaviour
{
    [SerializeField] private GameSession session;
    [SerializeField] private TMP_Text exitLabel;
    [SerializeField] private Material lockedMaterial;
    [SerializeField] private Material unlockedMaterial;

    private MeshRenderer padRenderer;
    private bool isUnlocked;

    private void Awake()
    {
        padRenderer = GetComponent<MeshRenderer>();
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void Start()
    {
        RefreshState();
    }

    private void Update()
    {
        if (session != null && session.ExitUnlocked != isUnlocked)
            RefreshState();
    }

    private void RefreshState()
    {
        isUnlocked = session != null && session.ExitUnlocked;

        Material padMaterial = isUnlocked ? unlockedMaterial : lockedMaterial;

        if (padMaterial != null)
            padRenderer.sharedMaterial = padMaterial;

        if (exitLabel != null)
            exitLabel.text = isUnlocked ? "EXIT\nOPEN" : "EXIT\nLOCKED";
    }

    private void OnTriggerStay(Collider other)
    {
        if (session == null || !session.ExitUnlocked || session.IsComplete)
            return;

        if (other.GetComponentInParent<CourierPlayer>() != null)
            session.CompleteLevel();
    }
}

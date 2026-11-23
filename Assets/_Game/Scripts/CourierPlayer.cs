using UnityEngine;
using UnityEngine.InputSystem;

public enum CourierColor
{
    Red,
    Blue,
    Yellow
}

[RequireComponent(typeof(Rigidbody), typeof(MeshRenderer))]
public class CourierPlayer : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material blueMaterial;
    [SerializeField] private Material yellowMaterial;

    public CourierColor CurrentColor { get; private set; }

    private Rigidbody rb;
    private MeshRenderer bodyRenderer;
    private Vector3 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bodyRenderer = GetComponent<MeshRenderer>();
        SetColor(CourierColor.Red);
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        moveInput = Vector3.zero;

        if (keyboard == null)
            return;

        if (keyboard.wKey.isPressed) moveInput.z += 1f;
        if (keyboard.sKey.isPressed) moveInput.z -= 1f;
        if (keyboard.aKey.isPressed) moveInput.x -= 1f;
        if (keyboard.dKey.isPressed) moveInput.x += 1f;

        moveInput = Vector3.ClampMagnitude(moveInput, 1f);

        if (keyboard.digit1Key.wasPressedThisFrame)
            SetColor(CourierColor.Red);
        else if (keyboard.digit2Key.wasPressedThisFrame)
            SetColor(CourierColor.Blue);
        else if (keyboard.digit3Key.wasPressedThisFrame)
            SetColor(CourierColor.Yellow);
    }

    private void FixedUpdate()
    {
        Vector3 velocity = moveInput * moveSpeed;
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;
    }

    private void OnDisable()
    {
        moveInput = Vector3.zero;

        if (rb != null)
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
    }

    private void SetColor(CourierColor color)
    {
        CurrentColor = color;

        switch (color)
        {
            case CourierColor.Red:
                bodyRenderer.sharedMaterial = redMaterial;
                break;
            case CourierColor.Blue:
                bodyRenderer.sharedMaterial = blueMaterial;
                break;
            case CourierColor.Yellow:
                bodyRenderer.sharedMaterial = yellowMaterial;
                break;
        }
    }
}

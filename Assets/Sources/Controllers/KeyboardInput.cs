using UnityEngine;

[RequireComponent(typeof(PhysicsMovement))]
public class KeyboardInput : MonoBehaviour
{
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";

    private PhysicsMovement _movement;

    private void Awake()
    {
        _movement = GetComponent<PhysicsMovement>();
    }

    private void LateUpdate()
    {
        float horizontal = Input.GetAxis(Horizontal);
        float vertical = Input.GetAxis(Vertical);

        _movement.MoveTo(Vector2.up * vertical + Vector2.right * horizontal);
    }
}

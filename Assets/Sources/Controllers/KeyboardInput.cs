using UnityEngine;

[RequireComponent(typeof(Movement))]
public class KeyboardInput : MonoBehaviour
{
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";

    private Movement _movement;

    private void Awake()
    {
        _movement = GetComponent<Movement>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis(Horizontal);
        float vertical = Input.GetAxis(Vertical);

        _movement.MoveTo(Vector2.up * vertical + Vector2.right * horizontal);
    }
}

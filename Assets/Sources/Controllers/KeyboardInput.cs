using UnityEngine;

[RequireComponent(typeof(NavMeshMovement))]
public class KeyboardInput : MonoBehaviour
{
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";

    private Vector2 _destination;

    private NavMeshMovement _movement;

    private void Awake()
    {
        _movement = GetComponent<NavMeshMovement>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis(Horizontal);
        float vertical = Input.GetAxis(Vertical);

        _destination = Vector2.up * vertical + Vector2.right * horizontal;
    }

    private void LateUpdate()
    {
        _movement.MoveTo(_destination);
    }
}

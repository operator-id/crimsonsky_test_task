using UnityEngine;

namespace Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerCharacter character;
        [SerializeField] private float moveSpeed = 1F;
        [Space]
        [SerializeField] private Camera raycastCamera;
        [SerializeField] private LayerMask validMoveLayer;
        [SerializeField] private LayerMask impassableLayer;

        private Vector2 _moveInput;
        private bool _queuedMoveOrder;

        private void Update()
        {
            HandleInput();
        }

        private void FixedUpdate()
        {
            if (_queuedMoveOrder)
            {
                var increment = _moveInput * (moveSpeed * Time.fixedDeltaTime);
                character.MoveWithIncrement(increment);
            }

            character.DoFixedUpdate();
        }

        private void HandleInput()
        {
            if (TryGetMoveDestinationInput(out var destination))
            {
                character.SetDestination(destination, moveSpeed);
                return;
            }

            _queuedMoveOrder = TryGetMoveDirectionInput(out _moveInput);
        }

        private bool TryGetMoveDestinationInput(out Vector3 position)
        {
            position = default;
            var didClick = Input.GetMouseButtonUp(0);
            if (!didClick)
            {
                return false;
            }

            var point = raycastCamera.ScreenToWorldPoint(Input.mousePosition);

            var hit = Physics2D.OverlapPoint(point, validMoveLayer | impassableLayer);

            if (hit == null || 1 << hit.gameObject.layer == impassableLayer)
            {
                return false;
            }

            position = point;

            return true;
        }

        private bool TryGetMoveDirectionInput(out Vector2 input)
        {
            input = Vector2.zero;
            if (Input.GetKey(KeyCode.W))
            {
                input.y += 1;
            }

            if (Input.GetKey(KeyCode.A))
            {
                input.x -= 1;
            }

            if (Input.GetKey(KeyCode.S))
            {
                input.y -= 1;
            }

            if (Input.GetKey(KeyCode.D))
            {
                input.x += 1;
            }

            return input != default;
        }
    }
}
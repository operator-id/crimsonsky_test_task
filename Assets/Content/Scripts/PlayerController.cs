using UnityEngine;

namespace Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerCharacter character;
        [SerializeField] private float moveSpeed = 1F;
        [Space] [SerializeField] private Camera raycastCamera;
        [SerializeField] private LayerMask impassableLayer;
        [SerializeField] private LayerMask validMoveLayer;

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

            var origin = raycastCamera.ScreenToWorldPoint(Input.mousePosition);
            var results = new Collider2D[1];
            var hitCount = Physics2D.OverlapPoint(origin, new ContactFilter2D
            {
                layerMask = validMoveLayer | impassableLayer
            }, results);

            if (hitCount == 0)
            {
                return false;
            }

            if (1 << results[0].gameObject.layer == validMoveLayer)
            {
                position = origin;
                return true;
            }

            return false;
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
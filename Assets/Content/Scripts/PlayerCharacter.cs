using UnityEngine;

namespace Scripts
{
    public class PlayerCharacter : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rigidBody;
        [SerializeField] private CircleCollider2D circleCollider;
        [SerializeField] private LayerMask impassableLayer;

        private bool _movesToTarget;
        private Vector2 _destination;
        private float _moveSpeed;
        private Vector2 _moveDirection;


        public void SetDestination(Vector2 destination, float moveSpeed)
        {
            _movesToTarget = true;
            _moveDirection = (destination - rigidBody.position).normalized;
            _moveSpeed = moveSpeed;
            _destination = destination;
        }

        public void MoveWithIncrement(Vector2 increment)
        {
            _movesToTarget = false;
            Move(increment);
        }

        public void DoFixedUpdate()
        {
            MoveToDestination();
        }

        private void MoveToDestination()
        {
            if (!_movesToTarget)
            {
                return;
            }

            if (Vector2.Distance(rigidBody.position, _destination) < 0.1F)
            {
                _movesToTarget = false;
                return;
            }

            var moveIncrement = _moveDirection * (_moveSpeed * Time.fixedDeltaTime);

            var hasCollided = Physics2D.OverlapCircle(rigidBody.position + moveIncrement, circleCollider.radius,
                impassableLayer) != null;
            if (hasCollided)
            {
                _movesToTarget = false;
                return;
            }

            Move(moveIncrement);
        }

        private void Move(Vector2 increment)
        {
            rigidBody.MovePosition(rigidBody.position + increment);
        }
    }
}
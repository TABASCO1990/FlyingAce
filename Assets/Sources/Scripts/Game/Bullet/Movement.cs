using System.Collections;
using UnityEngine;

namespace Game.Bullet
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private Transform _transformToMove;
        [SerializeField] private float _movementSpeed = 25;

        private Health _health;
        private bool _targetReached;

        public void Init(Health health)
        {
            _health = health;
        }

        public void StartMoving(Vector3 targetPosition)
        {
            StartCoroutine(StartStraightMove(targetPosition));
        }
        
        private IEnumerator StartStraightMove(Vector3 targetPosition)
        {
            _targetReached = false;

            _transformToMove.LookAt(targetPosition);

            while (_targetReached == false)
            {
                var position = _transformToMove.position;

                position = Vector3.MoveTowards(
                    position, 
                    targetPosition, 
                    _movementSpeed * Time.deltaTime);

                if (position == targetPosition)
                {
                    _targetReached = true;
                    continue;
                }

                _transformToMove.position = position;

                yield return new WaitForEndOfFrame();
            }

            _health.Die();
        }
    }
}

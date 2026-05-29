using UnityEngine;
using Random = UnityEngine.Random;

namespace Riftcore.Gameplay.Pickups.Core
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class PickupDropMotion : MonoBehaviour
    {
        [SerializeField] private Vector2 _upForceRange = new(2.5f, 4f);
        [SerializeField] private Vector2 _sideForceRange = new(0.5f, 1.5f);
        [SerializeField] private LayerMask _groundMask;

        private Rigidbody _rigidbody;

        public bool IsDropping { get; private set; }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Drop()
        {
            /*IsDropping = true;

            _rigidbody.isKinematic = false;
            _rigidbody.useGravity = true;

            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;

            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            float sideForce = Random.Range(_sideForceRange.x, _sideForceRange.y);
            float upForce = Random.Range(_upForceRange.x, _upForceRange.y);

            Vector3 force = new Vector3(
                randomDirection.x * sideForce,
                upForce,
                randomDirection.y * sideForce
            );

            _rigidbody.AddForce(force, ForceMode.Impulse);*/
        }

        public void Stop()
        {
            IsDropping = false;

            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;
        }

        /*private void OnCollisionEnter(Collision collision)
        {
            if (!IsDropping)
                return;

            if (((1 << collision.gameObject.layer) & _groundMask) == 0)
                return;

            Stop();
        }*/
    }
}
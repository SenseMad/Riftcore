using Riftcore.Gameplay.Players.Core;
using Riftcore.Gameplay.Players.Input;
using UnityEngine;

namespace Riftcore.Gameplay.Players.Movement
{
    public sealed class PlayerMovementController : MonoBehaviour
    {
        [Header("Speed")]
        [SerializeField, Min(0)] private float _speedWalking = 5.0f;
        [SerializeField, Min(0)] private float _speedRunning = 10.0f;

        [Header("Boost")]
        [SerializeField, Min(0)] private float _acceleration = 9.0f;
        [SerializeField, Min(0)] private float _deceleration = 11.0f;

        [Header("Gravity")]
        [SerializeField, Min(0)] private float _gravity = 35.0f;
        //[SerializeField, Min(0)] private float _jumpHeight = 1.5f;
        [SerializeField, Min(0)] private float _groundedGravity = 2.0f;

        [Header("Jump Helpers")]
        [SerializeField, Min(0)] private float _coyoteTime = 0.12f;
        [SerializeField, Min(0)] private float _jumpBufferTime = 0.12f;

        [Header("Rotation")]
        [SerializeField, Min(0)] private float _rotationSpeed = 5;

        private Player _player;
        private PlayerStateController _playerStateController;
        
        private Vector3 _velocity;
        private Vector2 _localMoveInput;

        private bool _isGrounded;
        private bool _isJumping;

        private float _lastGroundedTime;
        private float _lastJumpPressedTime = -999f;

        private void Awake()
        {
            _player = GetComponentInParent<Player>();
            
            _playerStateController = _player.GetComponentInChildren<PlayerStateController>();
        }

        public void Tick(PlayerInputData inputData)
        {
            if (_player == null || _player.CharacterController == null || _player.PlayerCameraController == null)
                return;

            UpdateGrounded();

            if (inputData.JumpPressed)
                _lastJumpPressedTime = Time.time;

            TryJump();
            Move(inputData, _playerStateController.CurrentState);
        }

        private void Move(PlayerInputData inputData, PlayerState playerState)
        {
            Vector2 frameInput = Vector2.ClampMagnitude(inputData.Move, 1f);

            Vector3 moveDirection = GetMoveDirection(frameInput);
            float targetSpeed = GetTargetSpeed(frameInput, playerState);
            Vector3 desiredHorizontalVelocity = moveDirection * targetSpeed;

            Rotate(moveDirection, playerState);
            UpdateHorizontalVelocity(desiredHorizontalVelocity, moveDirection);
            UpdateVerticalVelocity();
            ApplyMovement();
        }
        
        private void UpdateGrounded()
        {
            _isGrounded = _player.CharacterController.isGrounded;

            if (_isGrounded)
            {
                _lastGroundedTime = Time.time;
                _isJumping = false;
            }
        }
        
        private void TryJump()
        {
            bool hasJumpBuffer = Time.time - _lastJumpPressedTime <= _jumpBufferTime;
            bool hasGroundGrace = Time.time - _lastGroundedTime <= _coyoteTime;

            if (!hasJumpBuffer || !hasGroundGrace)
                return;

            _isJumping = true;
            _isGrounded = false;
            _lastJumpPressedTime = -999f;
            _lastGroundedTime = -999f;

            _velocity.y = Mathf.Sqrt(_player.GameStatistics.MovementStatistics.JumpHeight * 2f * _gravity);
        }
        
        private Vector3 GetMoveDirection(Vector2 input)
        {
            Vector3 camForward = _player.PlayerCameraController.transform.forward;
            camForward.y = 0f;
            camForward.Normalize();

            Vector3 camRight = _player.PlayerCameraController.transform.right;
            camRight.y = 0f;
            camRight.Normalize();

            Vector3 moveDirection = camRight * input.x + camForward * input.y;

            if (moveDirection.sqrMagnitude > 1f)
                moveDirection.Normalize();

            return moveDirection;
        }
        
        private float GetTargetSpeed(Vector2 moveInput, PlayerState playerState)
        {
            if (moveInput.sqrMagnitude <= 0.0001f)
                return 0f;

            float baseSpeed;

            if (playerState.IsSprinting)
                baseSpeed = _speedRunning;
            else
                baseSpeed = _speedWalking;

            return baseSpeed * _player.GameStatistics.MovementStatistics.MultiplierMoveSpeed;
        }
        
        private void Rotate(Vector3 moveDirection, PlayerState playerState)
        {
            if (moveDirection.sqrMagnitude <= 0.0001f)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection.normalized);
            _player.transform.rotation = Quaternion.Slerp(_player.transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
        }
        
        private void UpdateHorizontalVelocity(Vector3 desiredHorizontalVelocity, Vector3 moveDirection)
        {
            float lerpSpeed = moveDirection.sqrMagnitude > 0.0001f ? _acceleration : _deceleration;

            Vector3 currentHorizontalVelocity = new(_velocity.x, 0f, _velocity.z);
            Vector3 newHorizontalVelocity = Vector3.Lerp(currentHorizontalVelocity, desiredHorizontalVelocity, Time.deltaTime * lerpSpeed);

            _velocity.x = newHorizontalVelocity.x;
            _velocity.z = newHorizontalVelocity.z;
        }
        
        private void UpdateVerticalVelocity()
        {
            if (_isGrounded && _velocity.y <= 0f)
            {
                _velocity.y = -_groundedGravity;
                return;
            }

            _velocity.y -= _gravity * Time.deltaTime;
        }

        private void ApplyMovement()
        {
            if (!_player.CharacterController.enabled)
                return;

            _player.CharacterController.Move(_velocity * Time.deltaTime);
        }
    }
}
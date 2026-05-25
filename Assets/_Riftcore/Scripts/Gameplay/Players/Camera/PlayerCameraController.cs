using Riftcore.Gameplay.Players.Core;
using Riftcore.Gameplay.Players.Input;
using Riftcore.Infrastructure.Logging;
using Unity.Cinemachine;
using UnityEngine;

namespace Riftcore.Gameplay.Players.PlayerCamera
{
    public sealed class PlayerCameraController : MonoBehaviour, IPlayerLateTickable
    {
        [Header("Settings")]
        [SerializeField, Min(0)] private float _sensitivity = 1f;
        [SerializeField] private Vector2 _pitchLimits = new(-89f, 89f);
        
        private CinemachineCamera _cinemachineCamera;
        private PlayerControlState _playerControlState;
        private PlayerInputHandler _playerInputHandler;
        private PlayerTickRunner _playerTickRunner;
        
        private float _yaw;
        private float _targetPitch;
        private float _currentPitch;
        
        public Camera Camera { get; private set; }

        private void Awake()
        {
            Camera = Camera.main;
            
            _playerTickRunner = GetComponentInParent<PlayerTickRunner>();
            if (_playerTickRunner == null)
            {
                GameLog.Error($"{nameof(PlayerCameraController)}: PlayerTickRunner not found.");
                return;
            }
            
            _playerControlState = _playerTickRunner.GetComponentInChildren<PlayerControlState>();
            _playerInputHandler = _playerTickRunner.GetComponentInChildren<PlayerInputHandler>();
            
            if (_playerControlState == null)
                GameLog.Error($"{nameof(PlayerCameraController)}: PlayerControlState not found.");
            
            if (_playerInputHandler == null)
                GameLog.Error($"{nameof(PlayerCameraController)}: PlayerInputHandler not found.");
        }

        private void Start()
        {
            Vector3 euler = transform.rotation.eulerAngles;

            _yaw = euler.y;
            _targetPitch = NormalizeAngle(euler.x);
            _currentPitch = _targetPitch;
        }

        private void OnEnable()
        {
            _playerTickRunner?.Register(this);
        }

        private void OnDisable()
        {
            _playerTickRunner?.Unregister(this);
        }

        public void LateTick()
        {
            RotateCamera();
        }

        public void Initialize(CinemachineCamera cinemachineCamera)
        {
            _cinemachineCamera = cinemachineCamera;
            _cinemachineCamera.Target.TrackingTarget = transform;
        }
        
        public void SetYawFromRotation(Quaternion rotation)
        {
            Vector3 euler = rotation.eulerAngles;

            _yaw = euler.y;
            _targetPitch = NormalizeAngle(euler.x);
            _currentPitch = _targetPitch;

            transform.rotation = Quaternion.Euler(_currentPitch, _yaw, 0f);
        }

        public void ForceCameraWarp(Vector3 positionDelta)
        {
            if (_cinemachineCamera == null)
                return;

            _cinemachineCamera.OnTargetObjectWarped(transform, positionDelta);
        }
        
        private void RotateCamera()
        {
            if (_playerControlState != null && !_playerControlState.InputEnabled)
                return;
            
            if (_playerInputHandler == null)
                return;

            Vector2 look = _playerInputHandler.GetInput().Look;
            if (look.sqrMagnitude > 0.0001f)
            {
                _yaw += look.x * _sensitivity;
                _targetPitch -= look.y * _sensitivity;
            }
            
            _targetPitch = Mathf.Clamp(_targetPitch, _pitchLimits.x, _pitchLimits.y);
            _currentPitch = Mathf.Lerp(_currentPitch, _targetPitch, Time.deltaTime * 15f);
            
            transform.rotation = Quaternion.Euler(_currentPitch, _yaw, 0f);
        }
        
        private static float NormalizeAngle(float angle)
        {
            if (angle > 180f)
                angle -= 360f;

            return angle;
        }
    }
}
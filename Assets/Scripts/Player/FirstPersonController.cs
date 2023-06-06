using InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]
    public class FirstPersonController : MonoBehaviour
    {
        private const float Threshold = 0.01f;
        private const float TerminalVelocity = 53.0f;
        
        #region User Variables

        [Header("Player")] [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 4.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 6.0f;

        [Tooltip("Rotation speed of the character")]
        public float RotationSpeed = 1.0f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        [Space(10)] [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Tooltip("What to multiply the character height with when crouching")]
        public float CrouchHeightMultiplier;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.1f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")] public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.5f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 90.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -90.0f;

        #endregion
        
        // cinemachine
        private float _cinemachineTargetPitch;
        private CharacterController _controller;

        private float _defaultHeight;
        private float _fallTimeoutDelta;
        private StarterAssetsInputs _input;
        private bool _isCrouching;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private GameObject _mainCamera;
        private PlayerInput _playerInput;
        private float _rotationVelocity;

        // player
        private float _speed;
        private float _verticalVelocity;
        private Vector3 _slideVelocity;
        
        private bool IsCurrentDeviceMouse => _playerInput.currentControlScheme == "KeyboardMouse";

        private float Height
        {
            // Get and set the y scale of the PlayerCapsule object
            get => transform.localScale.y;
            set => transform.localScale = new Vector3(transform.localScale.x, value, transform.localScale.z);
        }

        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null) _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
            _playerInput = GetComponent<PlayerInput>();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
            _defaultHeight = Height;
        }

        private void Update()
        {
            JumpAndGravity();
            Crouch();
            GroundedCheck();
            Move();
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void OnDrawGizmosSelected()
        {
            var transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            var transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            var position = transform.position;
            Gizmos.DrawSphere(new Vector3(position.x, position.y - GroundedOffset, position.z), GroundedRadius);
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            var position = transform.position;
            var spherePosition = new Vector3(position.x, position.y - GroundedOffset, position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);
        }

        private void CameraRotation()
        {
            // if there isn't an input
            if (!(_input.look.sqrMagnitude >= Threshold)) return;

            //Don't multiply mouse input by Time.deltaTime
            var deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetPitch += _input.look.y * RotationSpeed * deltaTimeMultiplier;
            _rotationVelocity = _input.look.x * RotationSpeed * deltaTimeMultiplier;

            // clamp our pitch rotation
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Update Cinemachine camera target pitch
            CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

            // rotate the player left and right
            transform.Rotate(Vector3.up * _rotationVelocity);
        }

        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            var targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            var velocity = _controller.velocity;
            var currentHorizontalSpeed = new Vector3(velocity.x, 0.0f, velocity.z).magnitude;

            const float speedOffset = 0.1f;
            var inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            // normalise input direction
            var inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.move != Vector2.zero)
                // move
            {
                var playerTransform = transform;
                inputDirection = playerTransform.right * _input.move.x + playerTransform.forward * _input.move.y;
            }

            // move the player
            _controller.Move(inputDirection.normalized * (_speed * Time.deltaTime * (_isCrouching ? .4f : 1)) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime + 
                             _slideVelocity * Time.deltaTime);
        }

        private void Crouch()
        {
            if (_input.crouch && !_isCrouching)
            {
                _isCrouching = true;
                Height = _defaultHeight * CrouchHeightMultiplier;
                _verticalVelocity -= 40f;

                var forward = transform.forward;
                forward.y = 0;
                _slideVelocity = forward * 20f;
            }
            else if (!_input.crouch && _isCrouching)
            {
                _isCrouching = false;
                //TODO: CHeck if there's enough room to stand up
                Height = _defaultHeight;
                _slideVelocity = Vector3.zero;
            }
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f) _verticalVelocity = -2f;

                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f) _jumpTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f) _fallTimeoutDelta -= Time.deltaTime;

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < TerminalVelocity) _verticalVelocity += Gravity * Time.deltaTime;
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
    }
}
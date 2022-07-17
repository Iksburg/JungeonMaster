using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")] 
        private float _moveSpeed;

        public float walkSpeed;
        public float sprintSpeed;
    
        public float groundDrag;
    
        [Header("Crouching")]
        public float crouchSpeed;
        public float crouchYScale;
        private float _startYScale;
    
        [Header("Jumping")]
        public float jumpForce;
        public float jumpCooldown;
        public float airMultiplier;
        private bool _readyToJump;

        [Header("KeyBinds")] 
        public KeyCode jumpKey = KeyCode.Space;
        public KeyCode sprintKey = KeyCode.LeftShift;
        public KeyCode crouchKey = KeyCode.LeftControl;
    
        [Header("Ground Check")] 
        public float playerHeight;
        public LayerMask whatIsGround;
        private Vector3 _correctedVector;
        private bool _grounded;

        [Header("Slope Handling")] 
        public float maxSlopeAngle;
        private RaycastHit _slopeHit;
        private bool _exitingSlope;
    
        public Transform orientation;

        private float _horizontalInput;
        private float _verticalInput;

        private Vector3 _moveDirection;

        private Rigidbody _rb;

        public MovementState state;
        public enum MovementState
        {
            Crouching,
            Walking,
            Sprinting,
            Air
        }
    
        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.freezeRotation = true;

            _readyToJump = true;

            _startYScale = transform.localScale.y;
        }
        
        private void Update()
        {
            //ground check
            _correctedVector = transform.position + new Vector3(0, 0.01f, 0);
            _grounded = Physics.Raycast(_correctedVector, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        
            MyInput();
            SpeedControl();
            StateHandler();
        
            //handle drag
            if (_grounded)
                _rb.drag = groundDrag;
            else
            {
                _rb.drag = 0;
            }
        }
    
        private void FixedUpdate()
        {
            MovePlayer();
        }
    
        // ReSharper disable Unity.PerformanceAnalysis
        private void MyInput()
        {
            _horizontalInput = Input.GetAxisRaw("Horizontal");
            _verticalInput = Input.GetAxisRaw("Vertical");
        
            // when jump
            if (Input.GetKey(jumpKey) && _readyToJump && _grounded)
            {
                _readyToJump = false;
            
                Jump();
            
                Invoke(nameof(ResetJump), jumpCooldown);
            }
        
            // when crouch
            if (Input.GetKeyDown(crouchKey))
            {
                transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
                _rb.AddForce(Vector3.down * 5, ForceMode.Impulse);
            }
        
            // when stop crouching
            if (Input.GetKeyUp(crouchKey))
            {
                transform.localScale = new Vector3(transform.localScale.x, _startYScale, transform.localScale.z);
            }
        }

        private void StateHandler()
        {
            // Crouching
            if (Input.GetKey(crouchKey))
            {
                state = MovementState.Crouching;
                _moveSpeed = crouchSpeed;
            }
            else switch (_grounded)
            {
                // Sprinting
                case true when Input.GetKey(sprintKey):
                    state = MovementState.Sprinting;
                    _moveSpeed = sprintSpeed;
                    break;
                // Walking
                case true:
                    state = MovementState.Walking;
                    _moveSpeed = walkSpeed;
                    break;
                // In air
                default:
                    state = MovementState.Air;
                    break;
            }
        }
    
        private void MovePlayer()
        {
            // calculate movement direction
            _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;
        
            // on slope
            if (OnSlope() && !_exitingSlope)
            {
                _rb.AddForce(GetSlopeMoveDirection() * (_moveSpeed * 20f), ForceMode.Force);
                if (_rb.velocity.y > 0)
                    _rb.AddForce(Vector3.down * 70f, ForceMode.Force);
            }
        
            //on ground
            else if (_grounded)
            {
                _rb.AddForce(_moveDirection.normalized * (_moveSpeed * 10f), ForceMode.Force);
            }
        
            // in air
            else {
                _rb.AddForce(_moveDirection.normalized * (_moveSpeed * 10f * airMultiplier), ForceMode.Force);
            }
            
            // use gravity in jump
            if (Input.GetKey(jumpKey))
                _rb.useGravity = true;
            else
                _rb.useGravity = !OnSlope();
        }

        private void SpeedControl()
        {
            // limiting speed on slope
            if (OnSlope() && !_exitingSlope)
            {
                if (_rb.velocity.magnitude > _moveSpeed)
                    _rb.velocity = _rb.velocity.normalized * _moveSpeed;
            }
        
            // limiting speed on ground or in air
            else
            {
                Vector3 flatVel = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        
                // limit velocity if it needed
                if (flatVel.magnitude > _moveSpeed)
                {
                    Vector3 limitedVel = flatVel.normalized * _moveSpeed;
                    _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
                }
            }
        }

        private void Jump()
        {
            _exitingSlope = true;
        
            // reset y velocity
            _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        
            _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        private void ResetJump()
        {
            _readyToJump = true;
            _exitingSlope = false;
        }

        private bool OnSlope()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, playerHeight * 0.5f + 0.3f))
            {
                float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
                return angle < maxSlopeAngle && angle != 0;
            }

            return false;
        }

        private Vector3 GetSlopeMoveDirection()
        {
            return Vector3.ProjectOnPlane(_moveDirection, _slopeHit.normal).normalized;
        }
    }
}

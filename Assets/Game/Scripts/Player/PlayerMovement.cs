using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _walkSpeed;
    [SerializeField]
    private float _sprintSpeed;
    [SerializeField]
    private float _rotationSmoothTime = 0.1f;
    [SerializeField]
    private float _walkingSprintTransition;
    [SerializeField]
    private float _jumpForce;
    [SerializeField]
    private Transform _groundDetector;
    [SerializeField]
    private float _detectorRadius;
    [SerializeField]
    private LayerMask _groundLayer;
    [SerializeField]
    private Vector3 _upperStepOffset;
    [SerializeField]
    private float _stepCheckerDistance;
    [SerializeField]
    private float _stepForce;
    [SerializeField]
    private Transform _climbDtector;
    [SerializeField]
    private float _climbCheckDistance;
    [SerializeField]
    private LayerMask _climbableLayer;
    [SerializeField]
    private Vector3 _climbOffset;
    [SerializeField]
    private float _climbSpeed;

    [SerializeField]
    private InputManager _input;

    private float _rotationSmoothVelocity;
    private float _speed;
    private bool _isGrounded;
    private PlayerStance _playerStance;
    private Rigidbody _rb;

    private void Awake()
    {
        _speed = _walkSpeed;
        _rb = GetComponent<Rigidbody>();
        _playerStance = PlayerStance.Stand;
    }

    private void Start()
    {
        _input.OnMoveInput += Move;
        _input.OnSprintInput += Sprint;
        _input.OnJumpInput += Jump;
        _input.OnClimbInput += StartClimb;
        _input.OnCancelClimb += CancelClimb;
    }

    private void OnDestroy()
    {
        _input.OnMoveInput -= Move;
        _input.OnSprintInput -= Sprint;
        _input.OnJumpInput -= Jump;
        _input.OnClimbInput -= StartClimb;
        _input.OnCancelClimb -= CancelClimb;
    }

    private void Update()
    {
        CheckIsGrounded();
        CheckStep();
    }

    private void Move(Vector2 axisDirection)
    {
        Vector3 movementDirection = Vector3.zero;
        bool isPlayerStanding = _playerStance == PlayerStance.Stand;
        bool isPlayerClimbing = _playerStance == PlayerStance.Climb;

        if (isPlayerStanding)
        {
            if (axisDirection.magnitude >= 0.1)
            {
                float rotationAngle = Mathf.Atan2(axisDirection.x, axisDirection.y) * Mathf.Rad2Deg;
                float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle, ref _rotationSmoothVelocity, _rotationSmoothTime);
                transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
                movementDirection = new Vector3(axisDirection.x, 0, axisDirection.y);
                _rb.AddForce(movementDirection * _speed * Time.deltaTime);
            }
        }
        else if (isPlayerClimbing)
        {
            Vector3 horizontal = axisDirection.x * transform.right;
            Vector3 vertical = axisDirection.y * transform.up;
            movementDirection = horizontal + vertical;
            _rb.AddForce(movementDirection * _speed * Time.deltaTime);
        }
    }

    private void Sprint(bool isSprint)
    {
        if (isSprint)
        {
            if(_speed < _sprintSpeed)
            {
                _speed += _walkingSprintTransition * Time.deltaTime;
            }
            
        }
        else
        {
            if (_speed > _walkSpeed)
            {
                _speed -= _walkingSprintTransition * Time.deltaTime;
            }
        }
    }

    private void Jump()
    {
        if (_isGrounded)
        {
            Vector3 jumpDirection = Vector3.up;
            _rb.AddForce(jumpDirection * _jumpForce * Time.deltaTime);
        }
    }

    private void CheckIsGrounded()
    {
        _isGrounded = Physics.CheckSphere(_groundDetector.position, _detectorRadius, _groundLayer);
    }

    private void CheckStep()
    {
        bool isHitLowerStep = Physics.Raycast(_groundDetector.position, transform.forward, _stepCheckerDistance);
        bool isHitUpperStep = Physics.Raycast(_groundDetector.position + _upperStepOffset, transform.forward, _stepCheckerDistance);
        if(isHitLowerStep && !isHitUpperStep)
        {
            _rb.AddForce(0, _stepForce * Time.deltaTime, 0);
        }
    }

    private void StartClimb()
    {
        bool isInFrontOfClimbingWall = Physics.Raycast(_climbDtector.position, transform.forward, out RaycastHit hit, _climbCheckDistance, _climbableLayer);
        bool isNotClimbing = _playerStance != PlayerStance.Climb;
        if(isInFrontOfClimbingWall && _isGrounded && isNotClimbing)
        {
            Vector3 offset = (transform.forward * _climbOffset.z) - (Vector3.up * _climbOffset.y);
            transform.position = hit.point - offset;
            _playerStance = PlayerStance.Climb;
            _rb.useGravity = false;
            _speed = _climbSpeed;
        }
    }

    private void CancelClimb()
    {
        if(_playerStance == PlayerStance.Climb)
        {
            _playerStance = PlayerStance.Stand;
            _rb.useGravity = true;
            _speed = _walkSpeed;
            transform.position -= transform.forward;
        }
    }
}

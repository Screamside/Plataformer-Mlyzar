using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{

    private PlayerInput _playerInput;


    [Header("Inputs")]
    [ReadOnly][SerializeField] private Vector2 _movementInputVector2;
    [ReadOnly][SerializeField] private bool _isJumpPressed;
    [ReadOnly][SerializeField] private bool _isRunningPressed;

    [Header("Movement")]
    [SerializeField] private float _maxSpeed = 5.0f;


    private Rigidbody _rigidbody;
    private Vector3 _desiredVelocity;


    private CharacterBaseState _currentCharacterState;
    private CharacterStateIdle _characterStateIdle = new CharacterStateIdle();
    private CharacterStateWalk _characterStateWalk = new CharacterStateWalk();
    private CharacterStateRun _characterStateRun = new CharacterStateRun();

    private void Awake()
    {
        _currentCharacterState = _characterStateIdle;
        _currentCharacterState.EnterState(this);

        _rigidbody = GetComponent<Rigidbody>();

        InitializeInputs();
    }

    private void InitializeInputs()
    {
        _playerInput = new PlayerInput();

        _playerInput.CharacterControls.Movement.performed += ctx => _movementInputVector2 = ctx.ReadValue<Vector2>();
        _playerInput.CharacterControls.Movement.canceled += ctx => _movementInputVector2 = ctx.ReadValue<Vector2>();

        _playerInput.CharacterControls.Run.started += ctx => _isRunningPressed = ctx.ReadValueAsButton();
        _playerInput.CharacterControls.Run.canceled += ctx => _isRunningPressed = ctx.ReadValueAsButton();

        _playerInput.CharacterControls.Jump.started += ctx => _isJumpPressed = ctx.ReadValueAsButton();
        _playerInput.CharacterControls.Jump.canceled += ctx => _isJumpPressed = ctx.ReadValueAsButton();
    }
    
    private void Update()
    {

        _desiredVelocity = new Vector3(_movementInputVector2.x, 9.8f, _movementInputVector2.y) * _maxSpeed;

    }

    private void FixedUpdate()
    {
        var velocity = _rigidbody.velocity;

        velocity.x = Mathf.MoveTowards(velocity.x, _desiredVelocity.x, _maxSpeed);
        velocity.z = Mathf.MoveTowards(velocity.z, _desiredVelocity.z, _maxSpeed);

        _rigidbody.velocity = velocity;
    }

    void SwitchState(CharacterBaseState state)
    {
        _currentCharacterState = state;
        state.EnterState(this);
    }


    private void OnEnable()
    {
        _playerInput.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        _playerInput.CharacterControls.Disable();
    }
}

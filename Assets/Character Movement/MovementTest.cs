using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTest : MonoBehaviour
{

    public PlayerInput input;

    [SerializeField]private Vector2 _moveDirection;
    [SerializeField] private bool _isJumpPressed;
    [SerializeField] private bool _isRunPressed;

    private Vector3 _desiredDirection;
    private bool _desiredJump;

    [SerializeField][Range(1f,100f)] private float _maxSpeed;
    [SerializeField][Range(1f, 100f)] private float _maxAcceleration;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private bool _snappyControls;


    private Rigidbody _rigidbody;
    private bool _onGround;


    void Awake()
    {

        _rigidbody = GetComponent<Rigidbody>();

        input = new PlayerInput();

        input.CharacterControls.Movement.started += ctx =>  _moveDirection = ctx.ReadValue<Vector2>();
        input.CharacterControls.Movement.performed += ctx => _moveDirection = ctx.ReadValue<Vector2>();
        input.CharacterControls.Movement.canceled += ctx =>  _moveDirection = ctx.ReadValue<Vector2>();

        input.CharacterControls.Jump.started += ctx => _isJumpPressed = ctx.ReadValueAsButton();
        input.CharacterControls.Jump.canceled += ctx => _isJumpPressed = ctx.ReadValueAsButton();

        input.CharacterControls.Run.started += ctx => _isRunPressed = ctx.ReadValueAsButton();
        input.CharacterControls.Run.canceled += ctx => _isRunPressed = ctx.ReadValueAsButton();



    }

    void OnEnable() { input.CharacterControls.Enable();}

    private void OnDisable() {input.CharacterControls.Disable();}
    
    // Update is called once per frame
    void Update()
    {


        _desiredJump |= _isJumpPressed;
        
    }

    void FixedUpdate()
    {
        var currentVelocity = _rigidbody.velocity;


        if (_snappyControls)
        {


            _desiredDirection = new Vector3(_moveDirection.x, -9.8f, _moveDirection.y);


            currentVelocity = (_desiredDirection * _maxSpeed);

            Debug.Log(currentVelocity.magnitude);

        }
        else
        {
            _desiredDirection = new Vector3(_moveDirection.x, -9.8f, _moveDirection.y);

            Vector3 desiredVelocity = _desiredDirection * _maxSpeed;

            float maxSpeedChange = _maxAcceleration * Time.fixedDeltaTime;

            currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, desiredVelocity.x, maxSpeedChange);
            currentVelocity.z = Mathf.MoveTowards(currentVelocity.z, desiredVelocity.z, maxSpeedChange);
        }

        currentVelocity.y = _rigidbody.velocity.y;

        if (_desiredJump)
        {
            _desiredJump = false;
            if (_onGround)
            {
                currentVelocity.y += Mathf.Sqrt(-2f * Physics.gravity.y * _jumpHeight);
            }
            
        }


        _rigidbody.velocity = currentVelocity;

    }

    private void OnCollisionEnter(Collision collision)
    {
        _onGround = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        _onGround = false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTest : MonoBehaviour
{

    public PlayerInput input;

    [SerializeField]private Vector2 _moveDirection;
    private Vector3 _desiredDirection;

    [SerializeField][Range(1f,100f)] private float _maxSpeed;
    [SerializeField][Range(1f, 100f)] private float _maxAcceleration;
    [SerializeField] private bool _snappyControls;


    private Rigidbody _rigidbody;

    void Awake()
    {

        _rigidbody = GetComponent<Rigidbody>();

        input = new PlayerInput();

        input.CharacterControls.Movement.started += ctx =>  _moveDirection = ctx.ReadValue<Vector2>();
        input.CharacterControls.Movement.performed += ctx => _moveDirection = ctx.ReadValue<Vector2>();
        input.CharacterControls.Movement.canceled += ctx =>  _moveDirection = ctx.ReadValue<Vector2>();

    }

    void OnEnable() { input.CharacterControls.Enable();}

    private void OnDisable() {input.CharacterControls.Disable();}
    
    // Update is called once per frame
    void Update()
    {
        
        
        
    }

    void FixedUpdate()
    {
        var currentVelocity = _rigidbody.velocity;

        if (_snappyControls)
        {



            _desiredDirection = new Vector3(_moveDirection.x, 0, _moveDirection.y);
            
            
            currentVelocity = (_desiredDirection * _maxSpeed);

            Debug.Log(currentVelocity.magnitude);



        }
        else
        {
            _desiredDirection = new Vector3(_moveDirection.x, currentVelocity.y, _moveDirection.y);

            Vector3 desiredVelocity = _desiredDirection * _maxSpeed;

            float maxSpeedChange = _maxAcceleration * Time.fixedDeltaTime;

            currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, desiredVelocity.x, maxSpeedChange);
            currentVelocity.z = Mathf.MoveTowards(currentVelocity.z, desiredVelocity.z, maxSpeedChange);
        }



        
        
        _rigidbody.velocity = currentVelocity;

    }
}

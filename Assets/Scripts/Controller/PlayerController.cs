using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    // Properties
    public bool IsGrounded
    {
        get => _isGrounded;
        private set
        {
            if (value != _isGrounded)
            {
                _isGrounded = value;
                _isGroundedChanged = true;
                if (value) _inAirJumpCount = 0;
            }
        }
    }
    
    public bool IsWalking 
    {
        get => _isWalking;
        private set
        {
            if (value != _isWalking)
            {
                _isWalking = value;
                _isWalkingChanged = true;
            }
        }
    }

    public bool IsFlipped
    {
        get => _spriteRenderer.flipX;
        private set
        {
            if (value != _spriteRenderer.flipX) _spriteRenderer.flipX = value;
        }
    }
    
    
    // Serialized fields
    [SerializeField] private float _moveSpeed = 50.0f;
    [SerializeField] private float _acceleration = 2.0f;
    [SerializeField] private float _friction = 2.0f;
    [SerializeField] private float _jumpForce = 50f;
    [SerializeField] private uint _maxInAirJumpCount = 1;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private string _groundTag = "Ground";
    
    // Components
    private SpriteRenderer _spriteRenderer;
    private CapsuleCollider2D _collider;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    
    // Private fields
    private float _horizontalAxis;
    private uint _inAirJumpCount;
    private bool _isJumpPressed;
    private float _lastPositionY;
    private bool _isGrounded;
    private bool _isGroundedChanged;
    private bool _isWalking;
    private bool _isWalkingChanged;
    
    // Constants
    private static readonly int IsWalkingParam = Animator.StringToHash("isWalking");
    private static readonly int IsGroundedParam = Animator.StringToHash("isGrounded");

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CapsuleCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        _inAirJumpCount = 0;
    }

    private void Update()
    {
        PlayerInput();
        AnimationControl();
        FlipSprite();
    }

    private void FixedUpdate()
    {
        GroundCheck();
        Jump();
        Movement();
    }
    
    private void PlayerInput()
    {
        _horizontalAxis = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            _isJumpPressed = true;
        }
    }

    private void Jump()
    {
        // Jump
        bool canJump = (IsGrounded || _inAirJumpCount > 0) && _isJumpPressed;
        _isJumpPressed = false;

        if (!canJump) return;
        
        
        _rigidbody.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
        if (IsGrounded) ResetInAirJump();
        else _inAirJumpCount--;
        
    }

    private void ResetInAirJump()
    {
        _inAirJumpCount = _maxInAirJumpCount;
    }
    
    private void Movement()
    {
        // Horizontal movements
        var moveInputForce = new Vector2(_horizontalAxis * _acceleration * 1000.0f * Time.deltaTime, 0);
        
        if (IsGrounded) IsWalking = moveInputForce.x != 0;
        else IsWalking = false;
        
        _rigidbody.AddForce(moveInputForce);

        var currentVel = _rigidbody.velocity;
        
        var speedAbs = Mathf.Abs(currentVel.x);
        if (speedAbs > _moveSpeed)
        {
            _rigidbody.velocity = new Vector2(currentVel.x / speedAbs * _moveSpeed, currentVel.y);
        }
        
        if (moveInputForce.x != 0) return;
        
        // Add friction when player input stops
        currentVel = _rigidbody.velocity;
        speedAbs = Mathf.Abs(currentVel.x);
        var frictionAmount = _friction * 1000.0f * Time.deltaTime;
        
        // Stop the player if the speed is too low
        if (speedAbs < 0.1f || (speedAbs - frictionAmount < 0 && IsGrounded))
        {
            _rigidbody.velocity = new Vector2(0, currentVel.y);
            return;
        }
        
        // Apply friction when grounded
        if (!IsGrounded) return;
        if (currentVel.x < 0) frictionAmount *= -1;
        _rigidbody.AddForce(new Vector2(-frictionAmount, 0));
    }

    private void GroundCheck()
    {
        var position = transform.position;
        
        // Only check if the player has moved in the Y axis
        if (Math.Abs(position.y - _lastPositionY) < 0.01f) return;
        _lastPositionY = position.y;
        
        var origin = new Vector2(position.x, position.y - _collider.size.y / 2.0f);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _groundLayer);

        IsGrounded = hit && hit.transform.CompareTag(_groundTag);
    }

    private void AnimationControl()
    {
        // Update animator parameters only when bool values change
        if (_isWalkingChanged)
        {
            _animator.SetBool(IsWalkingParam, IsWalking);
            _isWalkingChanged = false;
        }
        if (_isGroundedChanged)
        {
            _animator.SetBool(IsGroundedParam, IsGrounded);
            _isGroundedChanged = false;
        }
    }

    private void FlipSprite()
    {
        if (_horizontalAxis == 0) return;
        IsFlipped = _horizontalAxis < 0;
    }
}
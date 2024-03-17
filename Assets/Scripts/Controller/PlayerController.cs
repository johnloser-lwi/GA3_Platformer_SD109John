using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 50.0f;
    [SerializeField] private float _acceleration = 2.0f;
    [SerializeField] private float _friction = 2.0f;
    [SerializeField] private float _jumpForce = 50f;
    [SerializeField] private LayerMask _groundLayer;
    
    private SpriteRenderer _spriteRenderer;
    private CapsuleCollider2D _collider;
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    private bool _isGrounded;
    private bool _isGroundedChanged = false;
    private bool IsGrounded
    {
        get => _isGrounded;
        set
        {
            if (value != _isGrounded)
            {
                _isGrounded = value;
                _isGroundedChanged = true;
            }
        }
    }

    private bool _isWalking;
    private bool _isWalkingChanged = false;
    private bool IsWalking 
    {
        get => _isWalking;
        set
        {
            if (value != _isWalking)
            {
                _isWalking = value;
                _isWalkingChanged = true;
            }
        }
    }
    private float _horizontalAxis;
    private bool _canDoubleJump = false;
    private bool _isJumpPressed = false;
    private static readonly int IsWalkingParam = Animator.StringToHash("isWalking");
    private static readonly int IsGroundedParam = Animator.StringToHash("isGrounded");

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CapsuleCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
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
        bool canJump = IsGrounded || _canDoubleJump;

        if (_isJumpPressed && canJump)
        {
            _rigidbody.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
            if (!IsGrounded) _canDoubleJump = false;
        }
        _isJumpPressed = false;
    }
    
    private void Movement()
    {
        // Horizontal movements
        var moveInputForce = new Vector2(_horizontalAxis * _acceleration * Time.deltaTime, 0);
        
        if (IsGrounded) IsWalking = moveInputForce.x != 0;
        else IsWalking = false;
        
        _rigidbody.AddForce(moveInputForce);

        var currentVel = _rigidbody.velocity;
        
        var speedAbs = currentVel.x > 0 ? currentVel.x : -currentVel.x;
        if (speedAbs > _moveSpeed)
        {
            _rigidbody.velocity = new Vector2(currentVel.x / speedAbs * _moveSpeed, currentVel.y);
        }
        
        if (moveInputForce.x != 0) return;
        
        // Add friction when player input stops
        currentVel = _rigidbody.velocity;
        if (speedAbs < 1.0f)
        {
            _rigidbody.velocity = new Vector2(0, currentVel.y);
            return;
        }
        
        if (IsGrounded)
        {
            var frictionAmount = _friction * Time.deltaTime;
            if (currentVel.x < 0) frictionAmount *= -1;
            _rigidbody.AddForce(new Vector2(-frictionAmount, 0));
        }
    }

    private void GroundCheck()
    {
        var position = transform.position;
        var origin = new Vector2(position.x, position.y - _collider.size.y / 2.0f);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _groundLayer);

        IsGrounded = hit && hit.transform.CompareTag("Ground");

        if (IsGrounded) _canDoubleJump = true;
    }

    private void AnimationControl()
    {
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
        _spriteRenderer.flipX = _horizontalAxis < 0;
    }
}
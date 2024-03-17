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

    private float _horizontalAxis;
    private bool _isGrounded = false;
    private bool _canDoubleJump = false;
    private bool _isWalking = false;
    private bool _isJumpPressed = false;
    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    private static readonly int IsGrounded = Animator.StringToHash("isGrounded");

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
        bool canJump = _isGrounded || _canDoubleJump;

        if (_isJumpPressed && canJump)
        {
            _rigidbody.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
            if (!_isGrounded) _canDoubleJump = false;
        }
        _isJumpPressed = false;
    }
    
    private void Movement()
    {
        // Horizontal movements
        var moveInputForce = new Vector2(_horizontalAxis * _acceleration * Time.deltaTime, 0);
        
        if (_isGrounded) _isWalking = moveInputForce.x != 0;
        else _isWalking = false;
        
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
        
        if (_isGrounded)
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

        _isGrounded = hit && hit.transform.CompareTag("Ground");

        if (_isGrounded) _canDoubleJump = true;
    }

    private void AnimationControl()
    {
        _animator.SetBool(IsWalking, _isWalking);
        _animator.SetBool(IsGrounded, _isGrounded);
    }

    private void FlipSprite()
    {
        if (_horizontalAxis == 0) return;
        _spriteRenderer.flipX = _horizontalAxis < 0;
    }
}
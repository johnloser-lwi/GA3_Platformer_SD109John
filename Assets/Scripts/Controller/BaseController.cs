using System;
using UnityEngine;

namespace Controller
{
    // Base class for all controllers, shares common functionalities for movement and ground check.
    // This class is meant to be inherited by other controllers, such as PlayerController and EnemyController,
    // cannot be used on its own
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(CapsuleCollider2D))]
    public abstract class BaseController : MonoBehaviour, ICharacterController
    {
        // Properties
        // Getter and Setter can be used to add additional logic when the value is changed
        public bool IsGrounded
        {
            get => _isGrounded;
            private set
            {
                if (value != _isGrounded)
                {
                    _isGrounded = value;
                    _isGroundedChanged = true;
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

        // This property is to make sure we don't actually flip the sprite if it's already flipped
        public bool IsFlipped
        {
            get => _spriteRenderer.flipX;
            private set
            {
                if (value != _spriteRenderer.flipX) _spriteRenderer.flipX = value;
            }
        }
    
    
        // Serialized fields
        [Header("Movement Settings")]
        [SerializeField][Tooltip("Max speed the player can walk.")] 
        protected float _moveSpeed = 50.0f;
        [SerializeField][Tooltip("How fast the player accelerates to max speed when walking.")] 
        protected float _acceleration = 2.0f;
        [SerializeField][Tooltip("How fast the player decelerates to 0 when not walking.")] 
        protected float _friction = 2.0f;
        [SerializeField] protected float _upwardVelocityCap = 5.0f;
        
        [Space(10)]
        [Header("Ground Check")]
        [SerializeField][Tooltip("If true, the controller will check if it's grounded.")] 
        protected bool _needGroundCheck = true;
        [SerializeField][Tooltip("Layer mask to check if the controller is grounded. (Can be ignored if needGroundCheck is false)")] 
        protected LayerMask _groundLayer;

        [SerializeField] protected int _groundCheckInterval = 3;
    
        // Components
        protected SpriteRenderer _spriteRenderer;
        protected CapsuleCollider2D _collider;
        protected Rigidbody2D _rigidbody;
        protected Animator _animator;
    
        // protected fields
        protected float _horizontalAxis;
        protected bool _isWalking;
        protected bool _isWalkingChanged;
        protected bool _isGrounded;
        protected bool _isGroundedChanged;
        protected int _groundCheckCount = 0;

        protected virtual void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<CapsuleCollider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        protected virtual void Update()
        {
            FlipSprite();
        }

        protected virtual void FixedUpdate()
        {
            // Anything related to physics should be done in FixedUpdate
            Movement();
            UpwardVelocityCap();
            GroundCheck();
        }

        protected virtual void UpwardVelocityCap()
        {
            if (_rigidbody.velocity.y > _upwardVelocityCap)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _upwardVelocityCap);
            }
        }
    
        protected virtual void Movement()
        {
            // Horizontal movements
            var moveInputForce = new Vector2(_horizontalAxis * _acceleration * 1000.0f * Time.deltaTime, 0);
        
            // Check if the character is walking base on the input
            if (IsGrounded) IsWalking = moveInputForce.x != 0;
            else IsWalking = false;
        
            // Apply force to the rigidbody
            _rigidbody.AddForce(moveInputForce);

            // Limit the speed of the character to the max speed
            var currentVel = _rigidbody.velocity;
            var speedAbs = Mathf.Abs(currentVel.x);
            if (speedAbs > _moveSpeed)
            {
                _rigidbody.velocity = new Vector2(currentVel.x / speedAbs * _moveSpeed, currentVel.y);
            }
            
            // Add friction when movement input stops, skip if there's an active movement input
            if (moveInputForce.x != 0) return;
            
            currentVel = _rigidbody.velocity;
            speedAbs = Mathf.Abs(currentVel.x);
            
            // When calculating float values with vectors, it's more efficient to calculate the float value first
            // and then multiply it with the vector, instead of multiplying the vector with multiple float values
            var frictionAmount = Vector2.right * (_friction * 1000.0f * Time.deltaTime);
            var canBeStoppedByFriction = speedAbs - frictionAmount.x < 0 && IsGrounded;
            var isTooSlow = speedAbs < 0.1f;
            
            // Stop the player if the speed is too low or if the friction can stop the player
            if (isTooSlow || canBeStoppedByFriction)
            {
                _rigidbody.velocity = Vector2.up * currentVel.y; 
                return;
            }
        
            // Apply friction when grounded
            if (!IsGrounded) return;
            if (currentVel.x < 0) frictionAmount *= -1;
            _rigidbody.AddForce(-frictionAmount);
        }

        protected virtual void GroundCheck()
        {
            // If the controller doesn't need to check if it's grounded, then it's always grounded
            // Skipping the raycast will save some performance
            if (!_needGroundCheck)
            {
                IsGrounded = true;
                return;
            }
            
            _groundCheckCount++;
            if (_groundCheckCount % _groundCheckInterval != 0) return;

            // caching the position of the controller can save some performance too
            var position = transform.position;
        
            // Raycast origin should be at the bottom of the collider
            // The collider's pivot is at the center, so we need to subtract half of the collider's height
            var origin = new Vector2(position.x, position.y);
            
            // By defining the layer mask in the raycast, we can save some performance by not checking every collider
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, _collider.size.y / 2.0f + 0.2f, _groundLayer);
            IsGrounded = hit;
        }

        protected virtual void FlipSprite()
        {
            if (_horizontalAxis == 0) return;
            IsFlipped = _horizontalAxis < 0;
        }

        public BaseController GetController()
        {
            return this;
        }
    }
}
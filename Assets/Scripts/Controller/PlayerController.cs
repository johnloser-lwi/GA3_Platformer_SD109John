using UnityEngine;

namespace Controller
{
    // Inheritance from BaseController
    public class PlayerController : BaseController
    {
        // Serialized fields
        [Space(10)]
        [Header("Jump Settings")]
        [SerializeField][Tooltip("Force applied to the player when jumping.")]
        private float _jumpForce = 50f;
        [SerializeField][Tooltip("Max jumps player can perform in air (does not include the first jump from the ground).")] 
        private uint _maxInAirJumpCount = 1;
    
        // Private fields
        private uint _inAirJumpCount;
        private bool _isJumpPressed;
    
        // Constants
        // Hashed string for animator parameters, this is to avoid using string directly to save performance
        private static readonly int IsWalkingParam = Animator.StringToHash("isWalking");
        private static readonly int IsGroundedParam = Animator.StringToHash("isGrounded");

        protected override void Start()
        {
            base.Start();
            
            // Initialize in air jump count, in case player can jump in air at spawn
            ClearInAirJump();
        }

        protected override void Update()
        {
            base.Update();
            PlayerInput();
            AnimationControl();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            Jump();
        }
    
        private void PlayerInput()
        {
            // Handle input and physics separately
            _horizontalAxis = Input.GetAxis("Horizontal");
        
            if (!Input.GetButtonDown("Jump")) return;
            _isJumpPressed = true;
        }

        private void Jump()
        {
            // Base on the input and other conditions, decide if the player can jump
            bool canJump = (IsGrounded || (_inAirJumpCount > 0 && _rigidbody.velocity.y <= 0)) && _isJumpPressed;
            _isJumpPressed = false;
            
            if (!canJump) return;
            
            _rigidbody.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
            
            // if the player is grounded, reset in air jump count so player can jump again in the air
            if (IsGrounded) ResetInAirJump();
            // else, decrease in air jump count
            else _inAirJumpCount--;
        
        }

        private void ResetInAirJump()
        {
            _inAirJumpCount = _maxInAirJumpCount;
        }

        public void ClearInAirJump()
        {
            _inAirJumpCount = 0;
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

        // Need some extra logic for ground check in player controller script
        protected override void GroundCheck()
        {
            base.GroundCheck();
            
            // If the player is grounded, reset in air jump count to 0 so when player fall from the edge of the ground,
            // they won't be able to jump. Player can only perform air jump when jumping from the ground
            if (IsGrounded) ClearInAirJump();
        }
    }
}
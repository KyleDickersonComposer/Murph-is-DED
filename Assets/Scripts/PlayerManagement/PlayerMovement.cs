using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PlayerManagement
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings: ")]
        [SerializeField]private float moveForce = 10.0f;
        
        [Header("Jump Settings: ")]
        
        [Min(1f)]
        [SerializeField]private float jumpHeight = 10.0f;

        [Min(0.1f)]
        [SerializeField]private float jumpTime = 1.0f;

        [Header("Ground Check Settings: ")]
        [SerializeField]private Transform groundCheck;

        [Min(0.05f)]
        [SerializeField]private float checkDistance = 1.0f;
        [SerializeField]private LayerMask groundMask;
        [Header("Player Graphic Settings: ")]
        
        [SerializeField]private Transform graphic;
        private float _gravity = -9.8f;
        private float _initialJumpVelocity = 0.0f;
        private float _jumpForce = 0.0f;

        private Rigidbody2D _playerRB;
        private float _moveInput = 0.0f;


        private void Jump()
        {
            var hit = Physics2D.Linecast(groundCheck.position , 
                                groundCheck.position - Vector3.up * checkDistance,
                                groundMask.value);
            
            if(hit.collider != null)
            {
                _playerRB.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);    
            }
            
        }

        private void UpdateGraphicsRotation()
        {
            if(_moveInput == 0)
            {
                return;
            }

            var rotation = (_moveInput == 1.0f) ? Quaternion.AngleAxis(0.0f, Vector3.up) : Quaternion.AngleAxis(180.0f, Vector3.up);
            graphic.rotation = rotation;
        }
        private void CalculateParameters()
        {
            float halfJumpTime = jumpTime/2.0f;
            _gravity = (-2.0f * jumpHeight)/(halfJumpTime * halfJumpTime);
            _initialJumpVelocity = 2.0f * jumpHeight/halfJumpTime;
            _jumpForce = _playerRB.mass * (_initialJumpVelocity/jumpTime);
        }

        private void Awake() 
        {
            _playerRB = GetComponent<Rigidbody2D>();
        }

        // Start is called before the first frame update
        void Start()
        {
            _playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
            _playerRB.interpolation = RigidbodyInterpolation2D.Interpolate;
            _playerRB.gravityScale = 0.0f;
        }

        // Update is called once per frame
        void Update()
        {
            CalculateParameters();
            _moveInput = Input.GetAxisRaw("Horizontal");
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            UpdateGraphicsRotation();
        }

        private void FixedUpdate() 
        {
            _playerRB.AddForce(Vector2.right * _moveInput * moveForce, ForceMode2D.Impulse);
            _playerRB.AddForce(Vector2.up * _playerRB.mass * _gravity);
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position - Vector3.up * checkDistance);
        }
    }
}
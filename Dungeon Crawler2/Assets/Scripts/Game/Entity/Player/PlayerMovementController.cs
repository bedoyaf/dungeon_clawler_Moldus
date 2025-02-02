using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    //configuratable
    [SerializeField]
    private float speed;

    //helpers
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    //input
    private Vector2 movementInput;
    private Vector2 smoothedMovementInput;
    private Vector2 movementInputSmoothVelocity;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.freezeRotation = true;
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        _animator.SetBool("running", movementInput.magnitude > 0);


    }

    void FixedUpdate()
    {
        // smoothedMovementInput = Vector2.SmoothDamp(smoothedMovementInput, movementInput, ref movementInputSmoothVelocity, 0.1f);
        //smoothedMovementInput = Vector2.SmoothDamp(smoothedMovementInput, movementInput, ref movementInputSmoothVelocity, smoothingTime);
        //  Vector2 targetPosition = _rigidbody.position + movementInput * speed * Time.fixedDeltaTime;
        //_rigidbody.linearVelocity = smoothedMovementInput * speed;
        //_rigidbody.linearVelocity = smoothedMovementInput * speed;
        // _rigidbody.MovePosition(targetPosition);

        if (movementInput.magnitude > 0)
        {
            Vector2 targetPosition = _rigidbody.position + movementInput * speed * Time.deltaTime;
            _rigidbody.MovePosition(targetPosition);
        }
        else
        {
            // Stop the character by setting velocity to zero
            _rigidbody.linearVelocity = Vector2.zero;
        }
    }

   // private void OnMove(InputValue inputValue)
   // {
   //     movementInput = inputValue.Get<Vector2>();
 //   }
}

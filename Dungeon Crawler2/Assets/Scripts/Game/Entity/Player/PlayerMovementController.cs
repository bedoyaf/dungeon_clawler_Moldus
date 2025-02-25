using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    //configuratable
    [SerializeField] public float speed = 10f;

    //helpers
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private AudioSource _audioSource;

    //input
    private Vector2 movementInput;
    private Vector2 smoothedMovementInput;
    private Vector2 movementInputSmoothVelocity;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.freezeRotation = true;
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        _animator.SetBool("running", movementInput.magnitude > 0);


    }

    void FixedUpdate()
    {
        if (movementInput.magnitude > 0)
        {
            //_audioSource.Stop();
            //_audioSource.Play();
            Vector2 targetPosition = _rigidbody.position + movementInput * speed * Time.deltaTime;
            _rigidbody.MovePosition(targetPosition);
        }
        else
        {
            // Stop the character by setting velocity to zero
            _rigidbody.linearVelocity = Vector2.zero;
        }
    }

    public void changeSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}

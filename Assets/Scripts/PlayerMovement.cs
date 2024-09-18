using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _jumpForce;
    [SerializeField] Transform _groundPos;
    private float _horizontal;
    private float _vertical;

    private Vector3 _moveDirection;

    private CharacterController _controller;
    private Rigidbody _rb;

    float _distance = 0.2f;
    bool _isGrounded;
    bool _isJumping = false;

    float _timeElapsed = 0f;

    void Start()
    {
        this._rb = GetComponent<Rigidbody>();
        this._controller = GetComponent<CharacterController>();
    }


    void Update()
    {
        //this.HandleMovement_1();
        //this.HandleMovement_2();
        this.GroundCheck();
        this.HandleMovement_3();
    }

    private void HandleMovement_1()
    {
        this._horizontal = Input.GetAxis("Horizontal");
        this._vertical = Input.GetAxis("Vertical");

        this._moveDirection = new(this._horizontal, 0f, this._vertical);

        this.transform.Translate(this._moveDirection);
    }

    private void HandleMovement_2()
    {
        this._horizontal = Input.GetAxis("Horizontal");
        this._vertical = Input.GetAxis("Vertical");

        this._moveDirection = new(this._horizontal, 0f, this._vertical);

        this._rb.AddForce(this._moveDirection.normalized * this._moveSpeed * Time.deltaTime, ForceMode.Force);
    }

    private void HandleMovement_3()
    {
        this._horizontal = Input.GetAxis("Horizontal");
        this._vertical = Input.GetAxis("Vertical");
        this._moveDirection = new(this._horizontal, 0f, this._vertical);

        this._controller.Move(this._moveDirection.normalized * this._moveSpeed * Time.deltaTime);

        if(Input.GetKey(KeyCode.Space) && !this._isJumping && this._isGrounded)
        {
            this._isJumping = true;
            this._controller.Move(Vector3.up.normalized * this._jumpForce * Time.deltaTime);
        }

        if(this._isJumping)
        {
            this._timeElapsed += Time.deltaTime;

            if(this._timeElapsed > 0.6f)
            {
                this.ApplyGravity();
                this._isJumping = false;
            }
        }

        if(!this._isGrounded && !this._isJumping)
        {
            this.ApplyGravity();
            this._isJumping = false;
        }
    }


    private void ApplyGravity()
    {
        this._controller.Move(new Vector3(0f, Physics.gravity.y, 0f) * Time.deltaTime);
    }

    private void GroundCheck()
    { 
        if(Physics.Raycast(this._groundPos.position, Vector3.down, this._distance))
        {
            Debug.DrawRay(this._groundPos.position, Vector3.down, Color.blue, this._distance);
            Debug.Log("Grounded");
            this._isGrounded = true;
        }

        else
        {
            Debug.Log("Not Grounded");
            this._isGrounded = false;
        }
    }

    private void Jump()
    {
        /*
         jumpVector.y = jump * jumpforce


        else
        jumpVector.y += physics.gravity.y * time.deltaTime
        controller.move(jumpVector * time.deltaTime)
         */
    }
}

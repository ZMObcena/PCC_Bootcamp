using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public bool CanMove { get; private set; } = true;

    [Header("Functional Options")]
    [SerializeField] private bool _canHeadBob = true;
    [SerializeField] private bool _canInteract = true;

    [Header("Movement Parameters")]
    [SerializeField] private float _walkSpeed = 7.0f;
    [SerializeField] private float _gravity = 30.0f;

    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] private float _lookSpeedX = 1.0f;
    [SerializeField, Range(1, 10)] private float _lookSpeedY = 1.0f;
    [SerializeField, Range(1, 180)] private float _upperLookLimit = 80.0f;
    [SerializeField, Range(1, 180)] private float _lowerLookLimit = 80.0f;

    [Header("HeadBob Parameters")]
    [SerializeField] private float _walkBobSpeed = 14f;
    [SerializeField] private float _walkBobAmount = 0.05f;
    private float _defaultYPos = 0f;
    private float _timer;

    [Header("Interaction Parameters")]
    [SerializeField] private Vector3 _interactionRayPoint = default;
    [SerializeField] private float _interactionDistance = default;
    [SerializeField] private LayerMask _interactionLayer = default;
    private Interactable _currentInteractable;

    private Camera _playerCamera;
    private CharacterController _characterController;

    private Vector3 _moveDirection;
    private Vector2 _currentInput;

    private float _rotationX = 0f;

    private void Awake()
    {
        this._playerCamera = GetComponentInChildren<Camera>();
        this._characterController = GetComponent<CharacterController>();
        this._defaultYPos = this._playerCamera.transform.localPosition.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if(CanMove)
        {
            this.HandleMovementInput();
            this.HandleMouseLook();

            if(this._canHeadBob)
            {
                this.HandleHeadBob();
            }

            if(this._canInteract)
            {
                this.HandleInteractionCheck();
                this.HandleInteractionInput();
            }

            this.ApplyFinalMovements();
        }
    }

    private void HandleMovementInput()
    {
        this._currentInput = new Vector2(this._walkSpeed * Input.GetAxisRaw("Vertical"), this._walkSpeed * Input.GetAxisRaw("Horizontal"));

        float moveDirectionY = this._moveDirection.y;
        this._moveDirection = (this.transform.TransformDirection(Vector3.forward) * this._currentInput.x) + (this.transform.TransformDirection(Vector3.right) * this._currentInput.y);
    }

    private void HandleMouseLook()
    {
        this._rotationX -= Input.GetAxis("Mouse Y") * this._lookSpeedY;
        this._rotationX = Mathf.Clamp(this._rotationX, -this._upperLookLimit, this._lowerLookLimit);
        this._playerCamera.transform.localRotation = Quaternion.Euler(this._rotationX, 0, 0);
        this.transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * this._lookSpeedX, 0);
    }

    private void HandleHeadBob()
    {
        if (!this._characterController.isGrounded) return;

        if(Mathf.Abs(this._moveDirection.x) > 0.1f || Mathf.Abs(this._moveDirection.z) > 0.1f)
        {
            this._timer += Time.deltaTime * this._walkBobSpeed;
            this._playerCamera.transform.localPosition = new Vector3(
                this._playerCamera.transform.localPosition.x,
                this._defaultYPos + Mathf.Sin(this._timer) * this._walkBobAmount,
                this._playerCamera.transform.localPosition.z);
        }
    }

    private void HandleInteractionCheck()
    {
        if(Physics.Raycast(this._playerCamera.ViewportPointToRay(this._interactionRayPoint), out RaycastHit hit, this._interactionDistance))
        {
            if (hit.collider.gameObject.layer == 7 && (this._currentInteractable == null || hit.collider.gameObject.GetInstanceID() != this._currentInteractable.GetInstanceID())) 
            {
                hit.collider.TryGetComponent(out this._currentInteractable);

                if(this._currentInteractable)
                {
                    this._currentInteractable.OnFocus();
                }
            }
        }
        else if(this._currentInteractable)
        {
            this._currentInteractable.OnLoseFocus();
            this._currentInteractable = null;
        }
    }

    private void HandleInteractionInput()
    {
        if(Input.GetKeyDown(KeyCode.E) && this._currentInteractable != null && Physics.Raycast(this._playerCamera.ViewportPointToRay(this._interactionRayPoint), out RaycastHit hit, this._interactionDistance, this._interactionLayer))
        {
            this._currentInteractable.OnInteract();
        }
    }

    private void ApplyFinalMovements()
    {
        if(!this._characterController.isGrounded)
        {
            this._moveDirection.y -= this._gravity * Time.deltaTime;
        }

        this._characterController.Move(this._moveDirection * Time.deltaTime);
    }
}

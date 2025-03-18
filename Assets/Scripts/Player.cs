using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : KitchenObjectHolder
{
    public static Player Instance { get; private set; }

    [SerializeField] private float moveSpeed = 7;
    [SerializeField] private float rotateSpeed = 10;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private Rigidbody rb;

    private bool isWalking = false;
    private BaseCounter selectedCounter;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnOperateAction += GameInput_OnOperateAction;
    }

    private void Update()
    {
        HandleInteraction();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    public bool IsWalking
    {
        get { return isWalking; }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        selectedCounter?.Interact(this);
    }

    private void GameInput_OnOperateAction(object sender, System.EventArgs e)
    {
        Debug.Log("Player detected Operate action!");
        selectedCounter?.InteractOperate(this);
    }

    private void HandleMovement()
    {
        Vector3 direction = gameInput.GetMovementDirectionNormalized();

        isWalking = direction != Vector3.zero;

        if (direction != Vector3.zero)
        {
            rb.MovePosition(rb.position + direction * Time.fixedDeltaTime * moveSpeed);

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
        }
    }

    private void HandleInteraction()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitinfo, 2f, counterLayerMask))
        {
            if (hitinfo.transform.TryGetComponent<BaseCounter>(out BaseCounter counter))
            {
                SetSelectedCounter(counter);
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    public void SetSelectedCounter(BaseCounter counter)
    {
        if (counter != selectedCounter)
        {
            selectedCounter?.CancelSelect();
            counter?.SelectCounter();
            this.selectedCounter = counter;
        }
    }


}

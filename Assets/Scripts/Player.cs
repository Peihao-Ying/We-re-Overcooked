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
    [SerializeField] private LayerMask cookingStationLayerMask;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CookingStation cookingStation;

    private bool isWalking = false;
    private BaseCounter selectedCounter;
    private bool stationSelected;

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
        if (stationSelected)
        {
            cookingStation.Interact(this);
        }
    }

    private void GameInput_OnOperateAction(object sender, System.EventArgs e)
    {
        Debug.Log("Player detected Operate action!");
        selectedCounter?.InteractOperate(this);

        if (stationSelected)
        {
            cookingStation.CookAllIngredients();
        }
        TryGiveFoodToSlime();
    }

    private void GameInput_OnGiveFoodAction(object sender, System.EventArgs e)
    {
        TryGiveFoodToSlime();
    }

    private void TryGiveFoodToSlime()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2f);

        foreach (Collider collider in hitColliders)
        {
            SlimeOrder slime = collider.GetComponent<SlimeOrder>();
            if (slime != null)
            {
                Debug.Log("Slime detected! Trying to submit food...");
                slime.InteractOperate(this);
                return;
            }
        }
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
        else if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitinfo2, 2f, cookingStationLayerMask))
        {
            if (hitinfo2.transform.TryGetComponent<CookingStation>(out CookingStation station))
            {
                stationSelected = true;
            }
            else
            {
                stationSelected = false;
            }
        }
        else
        {
            SetSelectedCounter(null);
            stationSelected = false;
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

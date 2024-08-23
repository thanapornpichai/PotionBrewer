using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IIngredientObjectParent
{
    public static Player Instance{ get; private set; }

    public event EventHandler<OnSelectedTableChangedEventArgs> OnSelectedTableChanged;
    public class OnSelectedTableChangedEventArgs : EventArgs
    {
        public BaseTable selectedTable;
    }

    [SerializeField]
    private float moveSpeed = 7f;
    [SerializeField]
    private GameInput gameInput;
    [SerializeField]
    private LayerMask tablesLayerMask;
    [SerializeField]
    private Transform ingredientObjectHoldPoint;

    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseTable selectedTable;
    private IngredientObject ingredientObject;
    

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (selectedTable != null)
        {
            selectedTable.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if(selectedTable != null)
        {
            selectedTable.Interact(this);
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        if(moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        float interactDistance = 2f;
        if(Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, tablesLayerMask))
        {
            if(raycastHit.transform.TryGetComponent(out BaseTable baseTable))
            {
                if(baseTable != selectedTable)
                {
                    SetSelectedTable(baseTable);
                }
            }
            else
            {
                SetSelectedTable(null);
            }
        }
        else
        {
            SetSelectedTable(null);
        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x !=0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    moveDir = moveDirZ;
                }
                else
                {
                    //cannot move in any direction
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    private void SetSelectedTable(BaseTable selectedTable)
    {
        this.selectedTable = selectedTable;

        OnSelectedTableChanged?.Invoke(this, new OnSelectedTableChangedEventArgs
        {
            selectedTable = selectedTable
        });
    }

    public Transform GetIngredientFollowTransform()
    {
        return ingredientObjectHoldPoint;
    }

    public void SetIngredientObject(IngredientObject ingredientObject)
    {
        this.ingredientObject = ingredientObject;
    }

    public IngredientObject GetIngredientObject()
    {
        return ingredientObject;
    }

    public void ClearIngredientObject()
    {
        ingredientObject = null;
    }

    public bool HasIngredientObject()
    {
        return ingredientObject != null;
    }
}

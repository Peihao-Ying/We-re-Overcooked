using System.Collections;
using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float jumpForce = 5f;
    public float rotationSpeed = 5f;
    public float changeDirectionTime = 3f;
    public LayerMask groundLayer; 

    private Rigidbody rb;
    private Vector3 moveDirection;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(ChangeDirectionRoutine());
    }

    void Update()
    {
        Vector3 nextPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;

        // Check if the next position has an obstacle
        if (!Physics.Raycast(transform.position, moveDirection, 1f)) // 1f distance check
        {
            transform.position = nextPosition;
        }

        // Rotate the slime towards movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    IEnumerator ChangeDirectionRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(changeDirectionTime);
            PickRandomDirection();
        }
    }

    void PickRandomDirection()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        moveDirection = new Vector3(randomX, 0, randomZ).normalized;
    }
}

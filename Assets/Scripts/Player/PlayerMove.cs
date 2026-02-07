using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows;



public class PlayerMove : MonoBehaviour
{
    public float jumpForce = 5.0f;
    public float moveSpeed = 5.0f;

    private bool isGrounded;
    private Rigidbody2D rb;
    private Collider2D col;
    private BodyManager bm;
    public LayerMask groundLayer; // ���⿡ �� ���̾� ����

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bm = GetComponent<BodyManager>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        // �� ����ִ��� Ȯ���ϱ� ���� raycast
        float rayLength = col.bounds.extents.y - col.offset.y + 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, rayLength, groundLayer);
        isGrounded = (hit.collider != null);
        // ����׿� ��, ���߿� ����
        Debug.DrawRay(transform.position, Vector2.down * rayLength, isGrounded ? Color.green : Color.red);

        if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                Debug.Log("Jump Key Pressed, jumping");
                Jump();
            }
            else
            {
                Debug.Log("Jump Key Pressed, denied");
            }
        }
    }

    void FixedUpdate()
    {

        float h = UnityEngine.Input.GetAxisRaw("Horizontal");
        float speedMultiplier = bm.GetMovementSpeedMultiplier(); // �ӵ� ���� ��������
        float appliedSpeed = moveSpeed * speedMultiplier;

        rb.linearVelocity = new Vector2(h * appliedSpeed, rb.linearVelocityY);
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}

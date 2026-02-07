using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows;



public class PlayerMove : MonoBehaviour, IAnimatableCharacter
{
    public float jumpForce = 5.0f;
    public float moveSpeed = 5.0f;

    private bool isGrounded;
    private bool wasGrounded;

    private Rigidbody2D rb;
    private Collider2D col;
    private BodyManager bm;
    public LayerMask groundLayer;

    // --- IAnimatableCharacter implement ---
    public bool isFalling => !isGrounded && rb.linearVelocity.y < -0.1f;
    public float moveDirection => UnityEngine.Input.GetAxisRaw("Horizontal");
    public event Action OnJump;
    public event Action OnLand;
    public event Action<BodyPartType> OnPartAttached;
    public event Action<BodyPartType> OnPartDetached;
    // ----------------------------------------

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bm = GetComponent<BodyManager>();
        col = GetComponent<Collider2D>();
    }

    void Start()
    {
        // connect BodyManager event with interface event
        if (bm != null)
        {
            // BodyManager event -> OnPartDetached event activated
            bm.OnBodyPartBroken += (type) => OnPartDetached?.Invoke(type);
            bm.OnBodyPartRestored += (type) => OnPartAttached?.Invoke(type);
        }
    }

    void Update()
    {
        CheckGround();
        
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
                Debug.Log(isGrounded);
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
        OnJump?.Invoke();
    }

    private void CheckGround()
    {
        float rayLength = col.bounds.extents.y - col.offset.y + 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, rayLength, groundLayer);
        isGrounded = (hit.collider != null);

        bool currentGrounded = (hit.collider != null);

        // land on ground!
        if (!wasGrounded && currentGrounded)
        {
            OnLand?.Invoke();
            Debug.Log("Landed!");
        }

        Debug.DrawRay(transform.position, Vector2.down * rayLength, isGrounded ? Color.green : Color.red);

    }
}

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Windows;



public class PlayerMove : MonoBehaviour, IAnimatableCharacter
{
    public bool isFalling { get; private set; }
    public float moveDirection { get; private set; }
    
    public event Action OnJump;
    public event Action OnLand;
    public event Action<CharacterPartType> OnSwipeDown;
    public event Action<CharacterPartType> OnSwipeUp;

    public event Action<CharacterPartType> OnPartAttached;
    public event Action<CharacterPartType> OnPartDetached;
    
    public float jumpForce = 5.0f;
    public float moveSpeed = 5.0f;

    [SerializeField] private bool isGrounded;
    private bool isFloating = false;
    [SerializeField] private bool isNearDoor;
    private Rigidbody2D rb;
    private Collider2D col;
    private BodyManager bm;
    public LayerMask groundLayer;


    private Chest nearbyChest;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bm = GetComponent<BodyManager>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        isFalling = rb.linearVelocityY < 0f;
        
        float rayLength = col.bounds.extents.y + 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(col.bounds.center, Vector3.down, rayLength, groundLayer);
        //Debug.Log(hit.collider.gameObject.name);
        
        bool currentGrounded = (hit.collider != null);
        
        Debug.DrawRay(col.bounds.center, Vector2.down * rayLength, currentGrounded ? Color.green : Color.red);
        
        if (!isGrounded && currentGrounded && rb.linearVelocity.y <= 0.1f)
        {
            Debug.Log("Landed!");
            OnLand?.Invoke();
        
            isFalling = false;
        }
        
        isGrounded = currentGrounded;
        
        if (Math.Abs(rb.linearVelocity.y) > 0.1f && !isGrounded)
        {
            if (!isFloating) 
            {
                Debug.Log("Jump Start (Physics Based)!");
                OnJump?.Invoke(); 
                
                isFloating = true; 
            }
        }
        else 
        {
            isFloating = false;
        }
        
        
        
        if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                Jump();
            }
        }

        if (nearbyChest != null && UnityEngine.Input.GetKeyDown(KeyCode.E))
        {
            nearbyChest.UnlockChest(); // 상자 열기
            nearbyChest = null; // 열었으니 더 이상 상호작용 안 함)
        }


        if (isNearDoor) if(UnityEngine.Input.GetKeyDown(KeyCode.E)) GameSystem.instance.MoveTo(this.gameObject, new Vector3(0, 0,0), GameSystem.instance.isRest);
        

    }
    
    
    public void DetachPart(CharacterPartType part)
    {
        OnPartDetached?.Invoke(part);
    }

    void FixedUpdate()
    {

        float h = UnityEngine.Input.GetAxisRaw("Horizontal");
        moveDirection = h;
        float speedMultiplier = bm.GetMovementSpeedMultiplier();
        float appliedSpeed = moveSpeed * speedMultiplier;

        rb.linearVelocity = new Vector2(h * appliedSpeed, rb.linearVelocityY);
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Chest"))
        { 
            nearbyChest = other.GetComponent<Chest>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Chest"))
        {
            Chest exitChest = other.GetComponent<Chest>();
            if (exitChest == nearbyChest)
            {
                nearbyChest = null; 
            }
        }

        Debug.Log("OnTriggerExit2D");
        if(other.CompareTag("Door")) isNearDoor = false;
    }

    public void SwipeUp()
    {
        OnSwipeUp?.Invoke(CharacterPartType.FrontArm);
    }
    public void SwipeDown()
    {
        OnSwipeDown?.Invoke(CharacterPartType.FrontArm);
    }

}

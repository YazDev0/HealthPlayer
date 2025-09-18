using UnityEngine;

public class PlayerJumpDetector : MonoBehaviour
{
    [Header("إعدادات اللاعب")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    
    [Header("حالة اللاعب - للبطات")]
    public bool isGrounded = true;
    public bool justJumped = false; // إشارة واضحة للبطات
    
    private Rigidbody rb;
    private float jumpCooldown = 0f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        // الحركة
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        
        Vector3 movement = new Vector3(moveX, 0, moveZ) * moveSpeed;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
        
        // القفز
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            isGrounded = false;
            justJumped = true; // إشارة للبطات
            jumpCooldown = 0.5f; // نصف ثانية
            
            Debug.Log("اللاعب قفز! البطات يجب أن تقفز الآن");
        }
        
        // تقليل وقت الإشارة
        if (jumpCooldown > 0)
        {
            jumpCooldown -= Time.deltaTime;
        }
        else
        {
            justJumped = false;
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Ground") || 
            collision.gameObject.name.Contains("Plane") ||
            collision.gameObject.name.Contains("Floor") ||
            collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}


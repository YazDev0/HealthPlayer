using UnityEngine;

public class DuckController : MonoBehaviour
{
    [Header("إعدادات البطة")]
    public Transform player;
    public float followSpeed = 3f;
    public float jumpForce = 10f;
    public float followDistance = 3f;
    
    [Header("التحكم")]
    public bool isActive = true; // متغير للتحكم في تشغيل/إيقاف البطة
    public KeyCode stopKey = KeyCode.T; // زر الإيقاف (T)
    
    [Header("حالة البطة")]
    public bool isGrounded = true;
    
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        // التحكم في تشغيل/إيقاف البطة
        if (Input.GetKeyDown(stopKey))
        {
            ToggleDuck();
        }
        
        // تشغيل البطة فقط إذا كانت مفعلة
        if (isActive && player != null)
        {
            FollowPlayer();
            JumpWithPlayer();
        }
        else if (!isActive)
        {
            // إيقاف الحركة تماماً
            StopMovement();
        }
    }
    
    void ToggleDuck()
    {
        isActive = !isActive;
        
        if (isActive)
        {
            Debug.Log("البطة تعمل الآن! 🦆");
        }
        else
        {
            Debug.Log("البطة متوقفة! ⏸️");
            StopMovement();
        }
    }
    
    void StopMovement()
    {
        // إيقاف الحركة تماماً
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
        }
    }
    
    void FollowPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        
        if (distance > followDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            Vector3 movement = new Vector3(direction.x, 0, direction.z) * followSpeed;
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }
    
    void JumpWithPlayer()
    {
        // القفز عند ضغط Space
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            isGrounded = false;
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


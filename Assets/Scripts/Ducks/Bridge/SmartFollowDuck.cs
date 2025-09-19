using UnityEngine;

public class SmartFollowDuck : MonoBehaviour
{
    [Header("إعدادات المتابعة")]
    public Transform player;
    public float followSpeed = 3f;
    public float jumpForce = 10f;
    public float followDistance = 3f;
    
    [Header("إعدادات الجسر")]
    public Transform bridgePosition; // الموقع في الجسر
    public float bridgeSpeed = 2f;
    public Material bridgeMaterial;
    public Material normalMaterial;
    
    [Header("حالة البطة")]
    public bool isFollowing = true; // وضع المتابعة
    public bool isBridgeMode = false; // وضع الجسر
    public bool isInBridgePosition = false;
    public bool isGrounded = true;
    
    private Rigidbody rb;
    private Renderer duckRenderer;
    private Vector3 originalScale;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        duckRenderer = GetComponent<Renderer>();
        originalScale = transform.localScale;
        
        // حفظ المادة الأصلية
        if (normalMaterial == null && duckRenderer != null)
        {
            normalMaterial = duckRenderer.material;
        }
    }
    
    void Update()
    {
        if (isBridgeMode)
        {
            // وضع الجسر - اذهب للموقع المحدد
            MoveToBridgePosition();
        }
        else if (isFollowing && player != null)
        {
            // وضع المتابعة العادي
            FollowPlayer();
            JumpWithPlayer();
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
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            isGrounded = false;
        }
    }
    
    void MoveToBridgePosition()
    {
        if (bridgePosition == null) return;
        
        float distance = Vector3.Distance(transform.position, bridgePosition.position);
        
        if (distance > 0.1f)
        {
            // الحركة نحو موقع الجسر
            Vector3 direction = (bridgePosition.position - transform.position).normalized;
            rb.velocity = direction * bridgeSpeed;
            isInBridgePosition = false;
        }
        else
        {
            // وصلت لموقع الجسر
            if (!isInBridgePosition)
            {
                ActivateAsBridge();
            }
        }
    }
    
    public void ActivateAsBridge()
    {
        isInBridgePosition = true;
        
        // إيقاف الحركة
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        
        // تكبير الحجم قليلاً
        transform.localScale = originalScale * 1.2f;
        
        // تغيير المادة
        if (duckRenderer != null && bridgeMaterial != null)
        {
            duckRenderer.material = bridgeMaterial;
        }
        
        Debug.Log($"البطة {gameObject.name} وصلت لموقع الجسر! 🦆🌉");
    }
    
    public void SetBridgeMode(Transform bridgePos)
    {
        // تفعيل وضع الجسر
        isBridgeMode = true;
        isFollowing = false;
        bridgePosition = bridgePos;
        isInBridgePosition = false;
        
        Debug.Log($"البطة {gameObject.name} تتجه للجسر!");
    }
    
    public void SetFollowMode()
    {
        // العودة لوضع المتابعة
        isBridgeMode = false;
        isFollowing = true;
        isInBridgePosition = false;
        bridgePosition = null;
        
        // إعادة الإعدادات الأصلية
        rb.isKinematic = false;
        transform.localScale = originalScale;
        
        if (duckRenderer != null && normalMaterial != null)
        {
            duckRenderer.material = normalMaterial;
        }
        
        Debug.Log($"البطة {gameObject.name} عادت للمتابعة!");
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


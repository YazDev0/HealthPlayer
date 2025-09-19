using UnityEngine;

public class NoCollisionBridgeDuck : MonoBehaviour
{
    [Header("إعدادات البطة")]
    public Transform player;
    public int duckNumber = 0;
    public float followSpeed = 3f;
    public float bridgeSpeed = 5f;
    public float followDistance = 3f;
    
    [Header("حالة البطة")]
    public bool goToBridge = false;
    public bool isInBridgePosition = false;
    public bool isGrounded = true;
    
    [Header("تجنب التصادم")]
    public float avoidanceRadius = 1.5f; // مسافة تجنب البطات الأخرى
    public float avoidanceForce = 3f; // قوة التجنب
    
    private Rigidbody rb;
    private NoCollisionBridgeManager bridgeManager;
    private Vector3 originalScale;
    private Collider[] otherDucks; // البطات الأخرى
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalScale = transform.localScale;
        
        bridgeManager = FindObjectOfType<NoCollisionBridgeManager>();
        
        if (bridgeManager != null)
        {
            bridgeManager.RegisterDuck(this);
        }
        
        // إعداد طبقة خاصة للبطات لتجنب التصادم
        SetupDuckLayer();
    }
    
    void SetupDuckLayer()
    {
        // وضع البطة في طبقة "Duck" إذا كانت موجودة
        int duckLayer = LayerMask.NameToLayer("Duck");
        if (duckLayer != -1)
        {
            gameObject.layer = duckLayer;
        }
    }
    
    void Update()
    {
        if (goToBridge)
        {
            GoToBridgePositionSmart();
        }
        else
        {
            FollowPlayer();
            JumpWithPlayer();
        }
    }
    
    void FollowPlayer()
    {
        if (player == null) return;
        
        float distance = Vector3.Distance(transform.position, player.position);
        
        if (distance > followDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            Vector3 movement = new Vector3(direction.x, 0, direction.z) * followSpeed;
            
            // تطبيق تجنب البطات الأخرى
            Vector3 avoidance = CalculateAvoidance();
            movement += avoidance;
            
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
            rb.velocity = new Vector3(rb.velocity.x, 10f, rb.velocity.z);
            isGrounded = false;
        }
    }
    
    void GoToBridgePositionSmart()
    {
        if (bridgeManager == null) return;
        
        Vector3 targetPosition = bridgeManager.GetBridgePosition(duckNumber);
        float distance = Vector3.Distance(transform.position, targetPosition);
        
        if (distance > 0.3f)
        {
            // حساب الاتجاه للهدف
            Vector3 direction = (targetPosition - transform.position).normalized;
            Vector3 movement = direction * bridgeSpeed;
            
            // تطبيق تجنب البطات الأخرى
            Vector3 avoidance = CalculateAvoidance();
            movement += avoidance;
            
            // تجنب البطات التي وصلت لمواقعها
            Vector3 staticAvoidance = AvoidStaticDucks();
            movement += staticAvoidance;
            
            rb.velocity = movement;
            isInBridgePosition = false;
        }
        else
        {
            if (!isInBridgePosition)
            {
                BecomePartOfBridge();
            }
        }
    }
    
    Vector3 CalculateAvoidance()
    {
        Vector3 avoidanceVector = Vector3.zero;
        
        // البحث عن البطات القريبة
        Collider[] nearbyDucks = Physics.OverlapSphere(transform.position, avoidanceRadius);
        
        foreach (Collider other in nearbyDucks)
        {
            if (other != GetComponent<Collider>() && other.CompareTag("Duck"))
            {
                // حساب اتجاه التجنب
                Vector3 avoidDirection = transform.position - other.transform.position;
                avoidDirection.y = 0; // تجاهل المحور العمودي
                
                if (avoidDirection.magnitude > 0)
                {
                    avoidDirection = avoidDirection.normalized;
                    avoidanceVector += avoidDirection * avoidanceForce;
                }
            }
        }
        
        return avoidanceVector;
    }
    
    Vector3 AvoidStaticDucks()
    {
        Vector3 avoidanceVector = Vector3.zero;
        
        // تجنب البطات التي وصلت لمواقعها
        if (bridgeManager != null)
        {
            foreach (NoCollisionBridgeDuck duck in bridgeManager.GetAllDucks())
            {
                if (duck != this && duck.isInBridgePosition)
                {
                    float distance = Vector3.Distance(transform.position, duck.transform.position);
                    if (distance < avoidanceRadius * 2f)
                    {
                        Vector3 avoidDirection = transform.position - duck.transform.position;
                        avoidDirection.y = 0;
                        
                        if (avoidDirection.magnitude > 0)
                        {
                            avoidDirection = avoidDirection.normalized;
                            avoidanceVector += avoidDirection * avoidanceForce * 2f;
                        }
                    }
                }
            }
        }
        
        return avoidanceVector;
    }
    
    void BecomePartOfBridge()
    {
        isInBridgePosition = true;
        
        // توقف عن الحركة
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        
        // كبر الحجم
        transform.localScale = originalScale * 1.2f;
        
        // غير اللون
        GetComponent<Renderer>().material.color = Color.yellow;
        
        // تأكد من أن اللاعب يمكنه الوقوف عليها
        GetComponent<Collider>().isTrigger = false;
        
        Debug.Log($"البطة {duckNumber} وصلت للجسر! 🦆");
    }
    
    public void StartGoingToBridge()
    {
        goToBridge = true;
        isInBridgePosition = false;
        
        // تمكين المرور عبر البطات الأخرى مؤقتاً
        GetComponent<Collider>().isTrigger = true;
        
        Debug.Log($"البطة {duckNumber} تتجه للجسر!");
    }
    
    public void StopGoingToBridge()
    {
        goToBridge = false;
        isInBridgePosition = false;
        
        // ارجع للحالة العادية
        rb.isKinematic = false;
        transform.localScale = originalScale;
        GetComponent<Renderer>().material.color = Color.white;
        GetComponent<Collider>().isTrigger = false;
        
        Debug.Log($"البطة {duckNumber} عادت للمتابعة!");
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Ground") || 
            collision.gameObject.name.Contains("Plane") ||
            collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    
    void OnDrawGizmosSelected()
    {
        // رسم دائرة التجنب
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, avoidanceRadius);
    }
}


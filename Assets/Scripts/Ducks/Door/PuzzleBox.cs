using UnityEngine;

public class PuzzleBox : MonoBehaviour
{
    [Header("إعدادات الصندوق")]
    public float pushForce = 5f; // قوة الدفع
    public bool canBePushed = true; // هل يمكن دفع الصندوق
    
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // إعدادات الـ Rigidbody
        if (rb != null)
        {
            rb.mass = 2f; // وزن مناسب
            rb.drag = 5f; // مقاومة لمنع الانزلاق الزائد
        }
        
        // إضافة Tag للصندوق
        if (!gameObject.CompareTag("Box"))
        {
            gameObject.tag = "Box";
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        // إذا اصطدم اللاعب بالصندوق
        if (collision.gameObject.CompareTag("Player") && canBePushed)
        {
            PushBox(collision);
        }
    }
    
    void OnCollisionStay(Collision collision)
    {
        // استمرار الدفع أثناء التلامس
        if (collision.gameObject.CompareTag("Player") && canBePushed)
        {
            PushBox(collision);
        }
    }
    
    void PushBox(Collision collision)
    {
        if (rb == null) return;
        
        // حساب اتجاه الدفع
        Vector3 pushDirection = transform.position - collision.transform.position;
        pushDirection.y = 0; // منع الدفع العمودي
        pushDirection = pushDirection.normalized;
        
        // تطبيق القوة
        rb.AddForce(pushDirection * pushForce, ForceMode.Force);
    }
    
    // دالة لتمكين/تعطيل الدفع
    public void SetPushable(bool pushable)
    {
        canBePushed = pushable;
        
        if (rb != null)
        {
            rb.isKinematic = !pushable; // إذا لا يمكن دفعه، اجعله kinematic
        }
    }
}


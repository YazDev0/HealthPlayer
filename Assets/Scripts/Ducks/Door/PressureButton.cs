using UnityEngine;

public class PressureButton : MonoBehaviour
{
    [Header("إعدادات الزر")]
    public Door connectedDoor; // الباب المرتبط بهذا الزر
    public float pressDepth = 0.2f; // عمق الضغط
    public Color pressedColor = Color.red; // لون الزر عند الضغط
    public Color normalColor = Color.green; // لون الزر العادي
    
    [Header("حالة الزر")]
    public bool isPressed = false;
    public int objectsOnButton = 0; // عدد الأشياء على الزر
    
    private Vector3 originalPosition;
    private Vector3 pressedPosition;
    private Renderer buttonRenderer;
    
    void Start()
    {
        // حفظ الموقع الأصلي
        originalPosition = transform.position;
        pressedPosition = originalPosition - Vector3.up * pressDepth;
        
        // الحصول على الـ Renderer للألوان
        buttonRenderer = GetComponent<Renderer>();
        if (buttonRenderer != null)
        {
            buttonRenderer.material.color = normalColor;
        }
        
        // البحث عن الباب إذا لم يكن محدد
        if (connectedDoor == null)
        {
            connectedDoor = FindObjectOfType<Door>();
        }
    }
    
    void Update()
    {
        // تحديث حالة الزر
        UpdateButtonState();
    }
    
    void UpdateButtonState()
    {
        bool shouldBePressed = objectsOnButton > 0;
        
        if (shouldBePressed != isPressed)
        {
            isPressed = shouldBePressed;
            
            if (isPressed)
            {
                PressButton();
            }
            else
            {
                ReleaseButton();
            }
        }
    }
    
    void PressButton()
    {
        // تحريك الزر للأسفل
        transform.position = pressedPosition;
        
        // تغيير اللون
        if (buttonRenderer != null)
        {
            buttonRenderer.material.color = pressedColor;
        }
        
        // فتح الباب
        if (connectedDoor != null)
        {
            connectedDoor.OpenDoor();
        }
        
        Debug.Log("الزر مضغوط! الباب مفتوح! 🔓");
    }
    
    void ReleaseButton()
    {
        // إرجاع الزر لموقعه الأصلي
        transform.position = originalPosition;
        
        // إرجاع اللون الأصلي
        if (buttonRenderer != null)
        {
            buttonRenderer.material.color = normalColor;
        }
        
        // إغلاق الباب
        if (connectedDoor != null)
        {
            connectedDoor.CloseDoor();
        }
        
        Debug.Log("الزر محرر! الباب مقفل! 🔒");
    }
    
    void OnTriggerEnter(Collider other)
    {
        // عندما يدخل شيء على الزر
        if (other.CompareTag("Player") || other.CompareTag("Box") || other.name.Contains("Duck"))
        {
            objectsOnButton++;
            Debug.Log($"{other.name} دخل على الزر. العدد: {objectsOnButton}");
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        // عندما يخرج شيء من الزر
        if (other.CompareTag("Player") || other.CompareTag("Box") || other.name.Contains("Duck"))
        {
            objectsOnButton--;
            if (objectsOnButton < 0) objectsOnButton = 0; // تأكد من عدم النزول تحت الصفر
            Debug.Log($"{other.name} خرج من الزر. العدد: {objectsOnButton}");
        }
    }
}


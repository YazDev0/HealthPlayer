using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("إعدادات الباب")]
    public Vector3 openOffset = Vector3.up * 3f; // اتجاه فتح الباب
    public float moveSpeed = 2f; // سرعة حركة الباب
    public AudioClip openSound; // صوت فتح الباب
    public AudioClip closeSound; // صوت إغلاق الباب
    
    [Header("حالة الباب")]
    public bool isOpen = false;
    public bool isMoving = false;
    
    private Vector3 closedPosition;
    private Vector3 openPosition;
    private AudioSource audioSource;
    
    void Start()
    {
        // حفظ المواقع
        closedPosition = transform.position;
        openPosition = closedPosition + openOffset;
        
        // إعداد الصوت
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    
    void Update()
    {
        // تحريك الباب تدريجياً
        MoveDoor();
    }
    
    void MoveDoor()
    {
        if (!isMoving) return;
        
        Vector3 targetPosition = isOpen ? openPosition : closedPosition;
        
        // تحريك الباب نحو الهدف
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        
        // التحقق من الوصول للهدف
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            transform.position = targetPosition;
            isMoving = false;
            
            Debug.Log(isOpen ? "الباب فتح بالكامل! ✅" : "الباب أغلق بالكامل! ❌");
        }
    }
    
    public void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            isMoving = true;
            
            // تشغيل صوت الفتح
            if (audioSource != null && openSound != null)
            {
                audioSource.PlayOneShot(openSound);
            }
            
            Debug.Log("بدء فتح الباب... 🚪➡️");
        }
    }
    
    public void CloseDoor()
    {
        if (isOpen)
        {
            isOpen = false;
            isMoving = true;
            
            // تشغيل صوت الإغلاق
            if (audioSource != null && closeSound != null)
            {
                audioSource.PlayOneShot(closeSound);
            }
            
            Debug.Log("بدء إغلاق الباب... 🚪⬅️");
        }
    }
    
    // دالة لتبديل حالة الباب (للاختبار)
    public void ToggleDoor()
    {
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }
}


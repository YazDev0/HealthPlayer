using UnityEngine;

public class DuckManager : MonoBehaviour
{
    [Header("إعدادات المدير")]
    public KeyCode stopAllKey = KeyCode.P; // زر إيقاف جميع البطات (P)
    public bool allDucksActive = true;
    
    [Header("البطات")]
    public DuckController[] ducks; // اسحب جميع البطات هنا
    
    void Start()
    {
        // البحث عن جميع البطات تلقائياً إذا لم تكن محددة
        if (ducks == null || ducks.Length == 0)
        {
            ducks = FindObjectsOfType<DuckController>();
        }
        
        Debug.Log($"تم العثور على {ducks.Length} بطة");
    }
    
    void Update()
    {
        // إيقاف/تشغيل جميع البطات
        if (Input.GetKeyDown(stopAllKey))
        {
            ToggleAllDucks();
        }
    }
    
    void ToggleAllDucks()
    {
        allDucksActive = !allDucksActive;
        
        foreach (DuckController duck in ducks)
        {
            if (duck != null)
            {
                duck.isActive = allDucksActive;
            }
        }
        
        if (allDucksActive)
        {
            Debug.Log("جميع البطات تعمل الآن! 🦆🦆🦆");
        }
        else
        {
            Debug.Log("جميع البطات متوقفة! ⏸️⏸️⏸️");
        }
    }
    
    // دالة لإضافة بطة جديدة
    public void AddDuck(DuckController newDuck)
    {
        if (newDuck != null)
        {
            System.Array.Resize(ref ducks, ducks.Length + 1);
            ducks[ducks.Length - 1] = newDuck;
            newDuck.isActive = allDucksActive;
        }
    }
    
    // دالة لإيقاف بطة معينة
    public void StopDuck(int index)
    {
        if (index >= 0 && index < ducks.Length && ducks[index] != null)
        {
            ducks[index].isActive = false;
        }
    }
    
    // دالة لتشغيل بطة معينة
    public void StartDuck(int index)
    {
        if (index >= 0 && index < ducks.Length && ducks[index] != null)
        {
            ducks[index].isActive = true;
        }
    }
}


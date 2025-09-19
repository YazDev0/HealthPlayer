using UnityEngine;
using System.Collections.Generic;

public class NoCollisionBridgeManager : MonoBehaviour
{
    [Header("نقاط الجسر")]
    public Transform bridgeStart;
    public Transform bridgeEnd;
    
    [Header("إعدادات الجسر")]
    public int numberOfDucks = 5;
    public KeyCode sendToBridgeKey = KeyCode.B;
    
    [Header("تجنب التصادم")]
    public bool useDelayedStart = true; // بدء متدرج للبطات
    public float delayBetweenDucks = 0.2f; // التأخير بين كل بطة
    
    [Header("حالة الجسر")]
    public bool ducksAtBridge = false;
    
    private List<NoCollisionBridgeDuck> ducks = new List<NoCollisionBridgeDuck>();
    private Vector3[] bridgePositions;
    
    void Start()
    {
        CalculateBridgePositions();
        SetupDuckLayers();
        Debug.Log("مدير الجسر بدون تصادم جاهز!");
    }
    
    void SetupDuckLayers()
    {
        // إنشاء طبقة للبطات إذا لم تكن موجودة
        // يمكن عمل هذا يدوياً في Project Settings > Tags and Layers
        Debug.Log("تأكد من إنشاء طبقة 'Duck' في Project Settings > Tags and Layers");
    }
    
    void Update()
    {
        if (Input.GetKeyDown(sendToBridgeKey))
        {
            if (!ducksAtBridge)
            {
                if (useDelayedStart)
                {
                    StartCoroutine(SendDucksToBridgeDelayed());
                }
                else
                {
                    SendDucksToBridge();
                }
            }
            else
            {
                ReturnDucksToFollow();
            }
        }
    }
    
    void CalculateBridgePositions()
    {
        if (bridgeStart == null || bridgeEnd == null)
        {
            Debug.LogError("يجب تحديد نقطة البداية والنهاية!");
            return;
        }
        
        bridgePositions = new Vector3[numberOfDucks];
        
        for (int i = 0; i < numberOfDucks; i++)
        {
            float t = (float)i / (numberOfDucks - 1);
            bridgePositions[i] = Vector3.Lerp(bridgeStart.position, bridgeEnd.position, t);
        }
        
        CreateVisualMarkers();
        Debug.Log($"تم حساب {numberOfDucks} موقع للجسر");
    }
    
    void CreateVisualMarkers()
    {
        // حذف المؤشرات القديمة
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            if (child.name.StartsWith("BridgeMarker_"))
            {
                DestroyImmediate(child.gameObject);
            }
        }
        
        // إنشاء مؤشرات جديدة
        for (int i = 0; i < bridgePositions.Length; i++)
        {
            GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            marker.transform.position = bridgePositions[i];
            marker.transform.localScale = Vector3.one * 0.3f;
            marker.GetComponent<Renderer>().material.color = Color.cyan;
            marker.GetComponent<Collider>().enabled = false;
            marker.name = $"BridgeMarker_{i}";
            marker.transform.parent = transform;
        }
    }
    
    public void RegisterDuck(NoCollisionBridgeDuck duck)
    {
        if (!ducks.Contains(duck))
        {
            ducks.Add(duck);
            
            // إضافة Tag للبطة
            if (!duck.CompareTag("Duck"))
            {
                duck.tag = "Duck";
            }
            
            Debug.Log($"تم تسجيل البطة {duck.duckNumber}");
        }
    }
    
    public Vector3 GetBridgePosition(int duckNumber)
    {
        if (bridgePositions != null && duckNumber >= 0 && duckNumber < bridgePositions.Length)
        {
            return bridgePositions[duckNumber];
        }
        return Vector3.zero;
    }
    
    public List<NoCollisionBridgeDuck> GetAllDucks()
    {
        return ducks;
    }
    
    System.Collections.IEnumerator SendDucksToBridgeDelayed()
    {
        ducksAtBridge = true;
        Debug.Log("🌉 إرسال البطات للجسر بشكل متدرج!");
        
        // ترتيب البطات حسب المسافة من نقطة البداية
        SortDucksByDistanceToStart();
        
        for (int i = 0; i < ducks.Count && i < numberOfDucks; i++)
        {
            if (ducks[i] != null)
            {
                ducks[i].duckNumber = i;
                ducks[i].StartGoingToBridge();
                Debug.Log($"إرسال البطة {i}");
                
                // انتظار قبل إرسال البطة التالية
                yield return new WaitForSeconds(delayBetweenDucks);
            }
        }
    }
    
    void SendDucksToBridge()
    {
        ducksAtBridge = true;
        Debug.Log("🌉 إرسال البطات للجسر!");
        
        SortDucksByDistanceToStart();
        
        for (int i = 0; i < ducks.Count && i < numberOfDucks; i++)
        {
            if (ducks[i] != null)
            {
                ducks[i].duckNumber = i;
                ducks[i].StartGoingToBridge();
            }
        }
    }
    
    void SortDucksByDistanceToStart()
    {
        if (bridgeStart == null) return;
        
        // ترتيب البطات حسب المسافة من نقطة البداية
        ducks.Sort((duck1, duck2) => 
        {
            float dist1 = Vector3.Distance(duck1.transform.position, bridgeStart.position);
            float dist2 = Vector3.Distance(duck2.transform.position, bridgeStart.position);
            return dist1.CompareTo(dist2);
        });
        
        Debug.Log("تم ترتيب البطات حسب المسافة");
    }
    
    void ReturnDucksToFollow()
    {
        ducksAtBridge = false;
        Debug.Log("🦆 إرجاع البطات للمتابعة!");
        
        foreach (NoCollisionBridgeDuck duck in ducks)
        {
            if (duck != null)
            {
                duck.StopGoingToBridge();
            }
        }
    }
    
    void OnDrawGizmos()
    {
        if (bridgeStart != null && bridgeEnd != null)
        {
            Gizmos.color = ducksAtBridge ? Color.green : Color.yellow;
            Gizmos.DrawLine(bridgeStart.position, bridgeEnd.position);
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(bridgeStart.position, 0.5f);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(bridgeEnd.position, 0.5f);
        }
        
        if (bridgePositions != null)
        {
            for (int i = 0; i < bridgePositions.Length; i++)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(bridgePositions[i], 0.3f);
            }
        }
    }
}


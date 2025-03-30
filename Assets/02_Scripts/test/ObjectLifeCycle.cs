using UnityEngine;

public class ObjectLifeCycle : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("Awake(), object 생성, 1회");
    }

    void OnEnable()
    {
        Debug.Log("OnEnable(), 활성화 시마다 호출, 최초생성 시 호출됨");
    }

    void Start()
    {
        Debug.Log("Start(), 1회");
    }

    void FixedUpdate()
    {
        Debug.Log("FixedUpdate(), 머신능력과 관계없이 고정적으로 호출, 1초에 50회 정도, cpu부하발생, 물리연산");
    }
        
    void Update()
    {
        Debug.Log("Update(), 머신능력에 따라 프레임별로 호출, 물리연산을 제외한 프레임별 호출");
    }

    void LateUpdate()
    {
        Debug.Log("LateUpdate(), Update() 이후 호출");
    }

    private void OnDisable()
    {
        Debug.Log("OnDisable(), 비활성화 시마다 호출");
    }

    void OnDestroy()
    {
        Debug.Log("OnDestroy(), object 삭제, 1회");
    }
}

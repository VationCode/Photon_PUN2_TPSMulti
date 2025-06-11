using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] GunSO m_gunSO;
    void Start()
    {
        Instantiate(m_gunSO.m_GunPrefab,this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;



[CreateAssetMenu(fileName = "GunData",menuName = "ScriptableObject/Gun")]
public class GunSO : ScriptableObject
{
    public string m_GunName;
    public GameObject m_GunPrefab;
    public GameObject m_Bullet;
    public GameObject m_Particle;
    public int m_GunDamage;
    public int m_GunSpeed;
}

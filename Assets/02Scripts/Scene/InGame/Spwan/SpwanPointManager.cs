using UnityEngine;

public class SpwanPointManager : MonoBehaviour
{
    public SpwanPoint[] m_SpwanPoints;

    private void Awake()
    {
        m_SpwanPoints = GetComponentsInChildren<SpwanPoint>();
    }
}

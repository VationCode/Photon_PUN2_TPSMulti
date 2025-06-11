using UnityEngine;

public class DistoryObj : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Destroy(this.gameObject);
    }
}

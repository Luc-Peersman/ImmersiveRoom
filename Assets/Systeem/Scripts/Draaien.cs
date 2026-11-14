using UnityEngine;

/// <summary>
/// Laat een object rustig rond zijn eigen as draaien, zoals een figuurtje op een sokkel.
/// </summary>
public class Draaien : MonoBehaviour
{
    [Tooltip("Hoeveel graden per seconde het object draait.")]
    public float gradenPerSeconde = 30f;

    void Update()
    {
        transform.Rotate(0f, gradenPerSeconde * Time.deltaTime, 0f, Space.World);
    }
}

using UnityEngine;

/// <summary>
/// Laat een object in een cirkel rond de bezoeker vliegen. Het gaat zo van de ene wand naar de volgende,
/// precies op de goede plek: zo zie je dat de vier wanden samen één ruimte zijn.
/// </summary>
public class RondjeOmBezoeker : MonoBehaviour
{
    [Tooltip("Het midden van de kamer (Wanden (camera's)).")]
    public Transform midden;

    [Tooltip("Afstand tot de bezoeker in meters.")]
    public float straal = 6f;

    [Tooltip("Hoogte boven de vloer in meters (de camera's hangen op 2 m).")]
    public float hoogte = 2.5f;

    [Tooltip("Hoeveel het object op en neer golft, in meters.")]
    public float golf = 0.8f;

    [Tooltip("Graden per seconde rond de bezoeker. Negatief = de andere kant op.")]
    public float snelheid = 20f;

    [Tooltip("Waar het object begint, in graden (0 = wand 1, 90 = wand 2, ...).")]
    public float startHoek = 0f;

    float hoek, tijd;

    void Start() { hoek = startHoek; }

    void Update()
    {
        if (midden == null) return;
        tijd += Time.deltaTime;
        hoek += snelheid * Time.deltaTime;
        float r = hoek * Mathf.Deg2Rad;
        Vector3 plek = new Vector3(Mathf.Sin(r) * straal, 0, Mathf.Cos(r) * straal);
        transform.position = new Vector3(midden.position.x, 0, midden.position.z) + plek
                             + Vector3.up * (hoogte + golf * Mathf.Sin(tijd * 0.7f + startHoek));
        transform.Rotate(0.3f, 1f, 0.2f, Space.Self);
    }
}

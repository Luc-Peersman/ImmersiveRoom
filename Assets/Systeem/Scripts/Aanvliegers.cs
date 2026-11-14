using UnityEngine;

/// <summary>
/// Laat steeds nieuwe objecten uit de verte op de bezoeker af vliegen, uit een van de vier wanden.
/// Met 3D (Anaglyph3D) aan lijkt het alsof ze uit de wand komen. Zet 'Aanval' aan voor meer en snellere objecten.
/// </summary>
public class Aanvliegers : MonoBehaviour
{
    [Tooltip("Het midden van de kamer, waar de bezoeker staat (Wanden (camera's)).")]
    public Transform midden;

    [Tooltip("Materialen waaruit gekozen wordt. Lichte kleuren met textuur werken het best in 3D.")]
    public Material[] materialen;

    [Tooltip("Seconden tussen twee objecten (in rust).")]
    public float interval = 1.2f;

    [Tooltip("Snelheid in meter per seconde (in rust).")]
    public float snelheid = 3f;

    [Tooltip("Waar de objecten beginnen: zoveel meter van het midden.")]
    public float startAfstand = 28f;

    [Tooltip("Waar de objecten stoppen en verdwijnen: zoveel meter van het midden. Kleiner = dichter bij de bezoeker.")]
    public float stopAfstand = 1.6f;

    [Tooltip("Aan: veel meer en snellere objecten.")]
    public bool aanval = false;

    float volgende;

    /// <summary>Zet de aanval aan of uit. Kan vanuit een state (UnityEvent) worden aangeroepen.</summary>
    public void ZetAanval(bool aan) { aanval = aan; }

    void Update()
    {
        if (midden == null || materialen == null || materialen.Length == 0) return;
        if (Time.time < volgende) return;
        volgende = Time.time + (aanval ? interval * 0.3f : interval);
        Lanceer();
    }

    void Lanceer()
    {
        int wand = Random.Range(0, 4);
        Vector3 richting = Quaternion.Euler(0, 90 * wand, 0) * Vector3.forward;
        Vector3 zijwaarts = Vector3.Cross(Vector3.up, richting);

        var vormen = new[] { PrimitiveType.Sphere, PrimitiveType.Cube, PrimitiveType.Cylinder, PrimitiveType.Capsule };
        var vorm = vormen[Random.Range(0, vormen.Length)];
        var go = GameObject.CreatePrimitive(vorm);
        go.name = "Aanvlieger";
        Destroy(go.GetComponent<Collider>());
        go.GetComponent<Renderer>().sharedMaterial = materialen[Random.Range(0, materialen.Length)];
        go.transform.SetParent(transform, true);
        go.transform.localScale = Vector3.one * Random.Range(0.4f, 1.1f);
        if (vorm == PrimitiveType.Cylinder) go.transform.localScale = new Vector3(1.2f, 0.12f, 1.2f); // een ring/schijf

        Vector3 opzij = zijwaarts * Random.Range(-1.6f, 1.6f) + Vector3.up * Random.Range(-1.0f, 1.2f);
        go.transform.position = midden.position + richting * startAfstand + opzij;

        var vlieger = go.AddComponent<Aanvlieger>();
        vlieger.doel = midden.position + richting * stopAfstand + opzij * 0.35f;
        vlieger.snelheid = aanval ? snelheid * 2.5f : snelheid;
    }
}

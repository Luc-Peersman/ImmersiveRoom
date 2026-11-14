using UnityEngine;

/// <summary>
/// Laat objecten door een deuropening binnenkomen, vlak langs de bezoeker vliegen en door een andere deur weer weggaan.
/// Omdat ze echt door de 3D-ruimte bewegen, verdwijnen ze op de ene wand en vliegen ze op de goede plek verder op een andere wand.
/// </summary>
public class Doorvliegers : MonoBehaviour
{
    [Tooltip("Het midden van de kamer, waar de bezoeker staat (Wanden (camera's)).")]
    public Transform midden;

    [Tooltip("Materialen waaruit gekozen wordt.")]
    public Material[] materialen;

    [Tooltip("Afstand van het midden tot de deuropeningen (en de gang erachter), in meters.")]
    public float deurAfstand = 18f;

    [Tooltip("Hoe dicht de objecten langs de bezoeker komen, in meters.")]
    public Vector2 langsAfstand = new Vector2(1.6f, 3f);

    [Tooltip("Seconden tussen twee objecten.")]
    public float interval = 0.8f;

    [Tooltip("Snelheid in meter per seconde.")]
    public float snelheid = 9f;

    [Tooltip("Aan: er komen objecten. Uit: er komen geen nieuwe.")]
    public bool actief = false;

    float volgende;

    /// <summary>Zet het doorvliegen aan of uit. Kan vanuit een state (UnityEvent) worden aangeroepen.</summary>
    public void ZetActief(bool aan) { actief = aan; }

    void Update()
    {
        if (!actief || midden == null || materialen == null || materialen.Length == 0) return;
        if (Time.time < volgende) return;
        volgende = Time.time + interval * Random.Range(0.6f, 1.4f);
        Lanceer();
    }

    static Vector3 Richting(int wand) => Quaternion.Euler(0, 90 * wand, 0) * Vector3.forward;

    void Lanceer()
    {
        int van = Random.Range(0, 4);
        int naar = Random.value < 0.6f ? (van + 2) % 4 : (van + (Random.value < 0.5f ? 1 : 3)) % 4;
        Vector3 m = midden.position;
        Vector3 start = m + Richting(van) * deurAfstand + Vector3.up * Random.Range(-0.6f, 1.2f);
        Vector3 eind = m + Richting(naar) * deurAfstand + Vector3.up * Random.Range(-0.6f, 1.2f);

        // Het punt waar het object vlak langs de bezoeker gaat: links of rechts van zijn vliegrichting
        Vector3 vlieg = (eind - start); vlieg.y = 0; vlieg.Normalize();
        Vector3 opzij = Vector3.Cross(Vector3.up, vlieg) * (Random.value < 0.5f ? -1 : 1);
        Vector3 langs = m + opzij * Random.Range(langsAfstand.x, langsAfstand.y) + Vector3.up * Random.Range(-0.4f, 1.0f);
        // Kwadratische Bezier-kromme die op t = 0.5 precies door 'langs' gaat
        Vector3 controle = 2f * langs - 0.5f * (start + eind);

        var vormen = new[] { PrimitiveType.Sphere, PrimitiveType.Cube, PrimitiveType.Capsule, PrimitiveType.Cylinder };
        var vorm = vormen[Random.Range(0, vormen.Length)];
        var go = GameObject.CreatePrimitive(vorm);
        go.name = "Doorvlieger";
        Destroy(go.GetComponent<Collider>());
        go.GetComponent<Renderer>().sharedMaterial = materialen[Random.Range(0, materialen.Length)];
        go.transform.SetParent(transform, true);
        go.transform.localScale = vorm == PrimitiveType.Cylinder ? new Vector3(1.1f, 0.1f, 1.1f) : Vector3.one * Random.Range(0.4f, 0.9f);

        var baan = go.AddComponent<BezierBaan>();
        baan.start = start; baan.controle = controle; baan.eind = eind; baan.snelheid = snelheid;
    }
}

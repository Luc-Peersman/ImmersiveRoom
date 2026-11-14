using UnityEngine;

/// <summary>Beweegt een object over een kwadratische Bezier-kromme en ruimt het aan het eind op.</summary>
public class BezierBaan : MonoBehaviour
{
    public Vector3 start, controle, eind;
    public float snelheid = 9f;
    float t, lengte;
    Vector3 draaiAs;

    void Start()
    {
        draaiAs = Random.onUnitSphere;
        Vector3 vorige = start;
        for (int i = 1; i <= 20; i++) { var p = Punt(i / 20f); lengte += Vector3.Distance(vorige, p); vorige = p; }
        transform.position = start;
    }

    Vector3 Punt(float u) => (1 - u) * (1 - u) * start + 2 * (1 - u) * u * controle + u * u * eind;

    void Update()
    {
        t += snelheid * Time.deltaTime / Mathf.Max(0.1f, lengte);
        transform.position = Punt(Mathf.Min(t, 1f));
        transform.Rotate(draaiAs, 120f * Time.deltaTime, Space.World);
        if (t >= 1f) Destroy(gameObject);
    }
}

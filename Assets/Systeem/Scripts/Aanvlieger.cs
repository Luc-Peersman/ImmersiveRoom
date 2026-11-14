using UnityEngine;

/// <summary>
/// Eén object dat draaiend naar zijn doel vliegt, daar krimpt en verdwijnt. Wordt gemaakt door Aanvliegers.
/// </summary>
public class Aanvlieger : MonoBehaviour
{
    public Vector3 doel;
    public float snelheid = 3f;

    Vector3 draaiAs;
    float krimpTijd = -1f;
    Vector3 beginSchaal;

    void Start()
    {
        draaiAs = Random.onUnitSphere;
        beginSchaal = transform.localScale;
    }

    void Update()
    {
        transform.Rotate(draaiAs, 90f * Time.deltaTime, Space.World);

        if (krimpTijd < 0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, doel, snelheid * Time.deltaTime);
            if ((transform.position - doel).sqrMagnitude < 0.0001f) krimpTijd = 0f;
            return;
        }

        // Bij het doel: in 0,4 seconde krimpen en verdwijnen
        krimpTijd += Time.deltaTime;
        transform.localScale = Vector3.Lerp(beginSchaal, Vector3.zero, krimpTijd / 0.4f);
        if (krimpTijd >= 0.4f) Destroy(gameObject);
    }
}

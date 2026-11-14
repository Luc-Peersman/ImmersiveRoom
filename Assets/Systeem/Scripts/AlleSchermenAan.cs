using UnityEngine;

/// <summary>
/// Zet bij de start alle aangesloten schermen aan.
/// Scherm 1 (Display 1) is het bedienscherm, scherm 2 t/m 5 zijn de vier beamers (wand 1 t/m 4).
/// Werkt alleen in een build (.exe), niet in de Editor: daar kies je het scherm bovenin de Game-weergave.
/// </summary>
public class AlleSchermenAan : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Aantal aangesloten schermen: " + Display.displays.Length);

        // Display.displays[0] staat altijd aan, daarom beginnen we bij 1
        for (int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }
    }
}

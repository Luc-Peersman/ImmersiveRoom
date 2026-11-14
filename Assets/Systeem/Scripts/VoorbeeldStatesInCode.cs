using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// VOORBEELD voor wie liever programmeert: dezelfde state machine als ProductieStates, maar in code.
/// Dit script staat niet in de scene. Wil je het gebruiken: zet het op een leeg GameObject
/// en zet ProductieStates uit (anders reageren ze allebei op dezelfde toetsen).
///
/// Een state is een plaatje uit je storyboard. In elke state betekent een toets iets anders.
/// </summary>
public class VoorbeeldStatesInCode : MonoBehaviour
{
    public GameObject objecten;      // sleep hier 'Objecten' in
    public AudioSource geluid;       // sleep hier een geluidsbron in

    int state = 0;

    void Start()
    {
        objecten.SetActive(false);
    }

    void Update()
    {
        var toetsenbord = Keyboard.current;
        if (toetsenbord == null) return;

        switch (state)
        {
            case 0: // Wachten op de bezoeker
                if (toetsenbord.spaceKey.wasPressedThisFrame)
                {
                    objecten.SetActive(true);
                    state = 1;
                }
                break;

            case 1: // De kamer is te zien
                if (toetsenbord.aKey.wasPressedThisFrame)
                {
                    geluid.Play();          // de productie blijft in state 1
                }
                if (toetsenbord.spaceKey.wasPressedThisFrame)
                {
                    objecten.SetActive(false);
                    state = 0;              // terug naar het begin
                }
                break;
        }
    }
}

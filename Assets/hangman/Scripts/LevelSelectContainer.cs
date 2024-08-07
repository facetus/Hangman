using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectContainer : MonoBehaviour
{
    public static GameObject tsiggelia;
    public static GameObject rodesia;

    public static void DisplayTsigellia()
    {
        tsiggelia.SetActive(true);
    }

    public static void DisplayRodesia()
    {
        rodesia.SetActive(true);

    }
}

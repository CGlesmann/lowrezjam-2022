using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static float meltingFactor = 1f;

    private void Awake()
    {
        Application.targetFrameRate = 24;
        Screen.SetResolution(640, 640, FullScreenMode.Windowed);
    }
}

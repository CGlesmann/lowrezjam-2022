using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        Screen.SetResolution(640, 640, FullScreenMode.Windowed);
    }
}

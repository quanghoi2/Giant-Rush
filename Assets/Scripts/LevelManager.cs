using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance = null;
    public Transform transPlayer;

    private void Awake()
    {
        Instance = this;
    }
}

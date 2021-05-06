using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBlockLock : MonoBehaviour
{
    public TextMeshPro textMultiScore;
    [HideInInspector]
    public Material matActive;
    public MeshRenderer mr;
    [HideInInspector]
    public int ID;

    public void UpdateActive()
    {
        if (matActive == null)
        {
            return;
        }

        Material[] mats = mr.materials;
        mats[0] = matActive;
        mr.materials = mats;
    }
}

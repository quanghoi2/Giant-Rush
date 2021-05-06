using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreBlock : MonoBehaviour
{
    public TextMeshPro textMultiScore;
    [HideInInspector]
    public Material matActive;
    public MeshRenderer mr;
    [HideInInspector]
    public int ID;

    [HideInInspector]
    public bool isActive = false;

    public void UpdateActive()
    {
        if(matActive == null)
        {
            return;
        }

        Material[] mats = mr.materials;
        mats[0] = matActive;
        mr.materials = mats;
        isActive = true;
    }
}

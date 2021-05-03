using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorStation : MonoBehaviour
{
    public List<Material> listColor;
    public MeshRenderer mr;

    public CHAR_COLOR mCharColor;

    // Start is called before the first frame update
    void Start()
    {
        UpdateColor();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void UpdateColor()
    {
        Material[] mats = mr.materials;
        mats[0] = listColor[(int)mCharColor];
        mr.materials = mats;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public List<Material> listColor;
    public SkinnedMeshRenderer sm;

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
        Material[] mats = sm.materials;
        mats[0] = listColor[(int)mCharColor];
        sm.materials = mats;
    }
}

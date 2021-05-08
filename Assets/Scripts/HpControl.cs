using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpControl : MonoBehaviour
{
    public RectTransform transHP;

    private float originalWidth;

    private void Start()
    {
        originalWidth = transHP.sizeDelta.x;
        Debug.Log("transHP.sizeDelta:" + transHP.sizeDelta);
    }

    public void UpdateHP(int hp)
    {
        Vector2 sd = transHP.sizeDelta;
        sd.x = originalWidth * hp / Define.MAX_HP;
        transHP.sizeDelta = sd;
    }
}

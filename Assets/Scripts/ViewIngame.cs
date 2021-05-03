using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewIngame : MonoBehaviour
{
    public GameObject mObjIndicator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(GameManager.Instance.State)
        {
            case STATE.READY:
                if(!mObjIndicator.activeSelf)
                {
                    mObjIndicator.SetActive(true);
                }
                break;
            case STATE.START_GAME:
                if (mObjIndicator.activeSelf)
                {
                    mObjIndicator.SetActive(false);
                }
                break;
        }
    }
}

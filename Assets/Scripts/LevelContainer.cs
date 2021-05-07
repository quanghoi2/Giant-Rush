using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelContainer : MonoBehaviour
{
    public static LevelContainer Instance = null;
    private STATE mState = STATE.NONE;

    const string PREFIX = "Level_";

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetState(STATE.PRE_LOAD);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }

    public STATE State
    {
        get { return mState; }
        set { mState = value; }
    }

    public void SetState(STATE state)
    {
        mState = state;
        switch(state)
        {
            case STATE.PRE_LOAD:
                foreach(Transform child in transform)
                {
                    Destroy(child.gameObject);
                }
                break;
            case STATE.LOAD:
                int level = 2;
                string nameLevel = PREFIX;
                if(level < 10)
                {
                    nameLevel += "0" + level;
                }
                else
                {
                    nameLevel += level;
                }
                GameObject prefabLevel = Resources.Load(nameLevel) as GameObject;
                Instantiate(prefabLevel, transform);
                break;
        }
    }

    private void UpdateState()
    {
        switch(mState)
        {
            case STATE.PRE_LOAD:
                if(transform.childCount == 0)
                {
                    SetState(STATE.LOAD);
                }
                break;

            case STATE.LOAD:
                if(transform.childCount > 0)
                {
                    SetState(STATE.READY);
                }
                break;
        }
    }
}

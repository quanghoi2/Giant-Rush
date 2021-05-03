using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    private STATE mState = STATE.READY;

    public STATE State
    {
        get { return mState; }
        set { mState = value; }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private STATE mState = STATE.READY;

    public PLAYER_STATE playerState;
    public BOSS_STATE bossState;

    public STATE State
    {
        get { return mState; }
        set { mState = value; }
    }
}

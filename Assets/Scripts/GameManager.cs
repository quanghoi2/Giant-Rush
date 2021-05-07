using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private STATE mState = STATE.LOAD;

    public PLAYER_STATE playerState;
    public PLAYER_STATE bossState;
    [HideInInspector]
    public int mMultiScore;
    public Player mPlayer;

    private int mBossHp = 4;
    private int mPlayerHp = 4;

    const int LAST_HIT = 1;

    private void Update()
    {
        UpdateState();
    }

    public STATE State
    {
        get { return mState; }
        set { mState = value; }
    }

    public bool AnimatorIsPlaying(Animator animator)
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1;
    }

    public bool AnimatorIsPlaying(Animator animator, string stateName)
    {
        return AnimatorIsPlaying(animator) && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    public int BossHP
    {
        get { return mBossHp; }
        set { mBossHp = value; }
    }

    public int PlayerHP
    {
        get { return mPlayerHp; }
        set { mPlayerHp = value; }
    }

    public void BossHitted()
    {
        BossHP -= 1;
        if (BossHP == 0)
        {
           bossState = PLAYER_STATE.KNOCK_OUT;
        }
        else
        {
            bossState = PLAYER_STATE.HITTED;
        }
    }

    public void PlayerHitted()
    {
        PlayerHP -= 1;
        if (PlayerHP == 0)
        {
            playerState = PLAYER_STATE.KNOCK_OUT;
        }
        else
        {
            playerState = PLAYER_STATE.HITTED;
        }
    }

    public bool IsPlayerLastHit()
    {
        return mPlayerHp == LAST_HIT;
    }

    public bool IsBossLastHit()
    {
        return mBossHp == LAST_HIT;
    }

    public void ReStartLevel()
    {
        CameraMgr.Instance.SetState(STATE.PRE_LOAD);
        LevelContainer.Instance.SetState(STATE.PRE_LOAD);
        mState = STATE.LOAD;
    }

    public void SetState(STATE state)
    {
        mState = state;
        switch(state)
        {
            case STATE.READY:
                if(!mPlayer.gameObject.activeSelf)
                {
                    mPlayer.gameObject.SetActive(true);
                }
                mPlayer.transform.position = Vector3.zero;
                mPlayer.SetState(PLAYER_STATE.READY);
                break;
        }
    }

    private void UpdateState()
    {
        switch(mState)
        {
            case STATE.LOAD:
                if(CameraMgr.Instance.State == STATE.READY && LevelContainer.Instance.State == STATE.READY)
                {
                    SetState(STATE.READY);
                }
                break;
        }
    }
}

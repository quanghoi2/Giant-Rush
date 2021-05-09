using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private STATE mState = STATE.LOAD;

    public PLAYER_STATE playerState;
    public PLAYER_STATE bossState;
    [HideInInspector]
    public int mMultiScore;
    public Player mPlayer;

    private int mBossHp = Define.MAX_HP;
    private int mPlayerHp = Define.MAX_HP;
    private Vector3 originalScale;

    private void Start()
    {
        originalScale = mPlayer.transform.localScale;
    }

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
        BossHP -= Define.DAME_UNIT;
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
        PlayerHP -= Define.DAME_UNIT;
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
        return mPlayerHp == Define.DAME_UNIT;
    }

    public bool IsBossLastHit()
    {
        return mBossHp == Define.DAME_UNIT;
    }

    public void ReStartLevel()
    {
        CameraMgr.Instance.SetState(STATE.PRE_LOAD);
        LevelContainer.Instance.SetState(STATE.PRE_LOAD);
        mState = STATE.LOAD;
    }

    public void NextLevel()
    {
        ProfileMgr.Instance.Level += 1;
        ProfileMgr.Instance.SaveProfile();
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
                mPlayer.transform.localScale = originalScale;
                mPlayer.transform.position = Vector3.zero;
                //mPlayer.transform.position = new Vector3(0, 0, 130);
                mPlayerHp = Define.MAX_HP;
                mBossHp = Define.MAX_HP;
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

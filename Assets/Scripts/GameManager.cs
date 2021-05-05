using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private STATE mState = STATE.READY;

    public PLAYER_STATE playerState;
    public BOSS_STATE bossState;

    private int mBossHp = 4;
    private int mPlayerHp = 4;

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
           bossState = BOSS_STATE.KNOCK_OUT;
        }
        else
        {
            bossState = BOSS_STATE.HITTED;
        }
    }
}

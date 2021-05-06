using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Animator mAnimator;

    private BOSS_STATE bossState;
    private string mAnimName;
    private Timer timerControl = new Timer();

    // Start is called before the first frame update
    void Start()
    {
        bossState = BOSS_STATE.IDLE;
        AnimPlay(CHAR_ANIM.IDLE);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (mAnimName)
            {
                case CHAR_ANIM.IDLE:
                    AnimPlay(CHAR_ANIM.READY_HIT);
                    break;
                case CHAR_ANIM.READY_HIT:
                    AnimPlay(CHAR_ANIM.HIT);
                    break;
                case CHAR_ANIM.HIT:
                    AnimPlay(CHAR_ANIM.HITTED);
                    break;
                case CHAR_ANIM.HITTED:
                    AnimPlay(CHAR_ANIM.KICK);
                    break;
                case CHAR_ANIM.KICK:
                    AnimPlay(CHAR_ANIM.KNOCK_OUT);
                    break;
                case CHAR_ANIM.KNOCK_OUT:
                    AnimPlay(CHAR_ANIM.READY_HIT);
                    break;
                default:
                    AnimPlay(CHAR_ANIM.READY_HIT);
                    break;
            }
        }

        UpdateState();
    }

    private void UpdateState()
    {
        switch(bossState)
        {
            case BOSS_STATE.IDLE:
                if(GameManager.Instance.State == STATE.FIGHT)
                {
                    SetState(BOSS_STATE.READY_HIT);
                }
                break;

            case BOSS_STATE.READY_HIT:
                if(GameManager.Instance.bossState == BOSS_STATE.READY_HIT && GameManager.Instance.playerState == PLAYER_STATE.READY_HIT)
                {
                    timerControl.Update(Time.deltaTime);
                    if(timerControl.JustFinished())
                    {
                        if(GameManager.Instance.IsPlayerLastHit())
                        {
                            SetState(BOSS_STATE.KICK);
                        }
                        else
                        {
                            SetState(BOSS_STATE.HIT);
                        }
                    }
                }
                else
                {
                    SetState(GameManager.Instance.bossState);                
                }
                break;
            case BOSS_STATE.HITTED:
                if (!GameManager.Instance.AnimatorIsPlaying(mAnimator, CHAR_ANIM.HITTED))
                {
                    SetState(BOSS_STATE.READY_HIT);
                }
                break;

            case BOSS_STATE.KNOCK_OUT:

                break;

            case BOSS_STATE.HIT:
                timerControl.Update(Time.deltaTime);
                if(timerControl.JustFinished())
                {
                    SetState(BOSS_STATE.FINISH_HIT);
                }
                break;
            case BOSS_STATE.FINISH_HIT:
                if(!GameManager.Instance.AnimatorIsPlaying(mAnimator, CHAR_ANIM.HIT))
                {
                    SetState(BOSS_STATE.READY_HIT);
                }
                break;

            case BOSS_STATE.KICK:
                timerControl.Update(Time.deltaTime);
                if(timerControl.JustFinished())
                {
                    SetState(BOSS_STATE.FINISH_KICK);
                }
                break;

            case BOSS_STATE.FINISH_KICK:
                if(!GameManager.Instance.AnimatorIsPlaying(mAnimator, CHAR_ANIM.KICK))
                {
                    SetState(BOSS_STATE.READY_HIT);
                }
                break;
        }
    }

    private void SetState(BOSS_STATE state)
    {
        bossState = state;
        GameManager.Instance.bossState = state;
        switch(state)
        {
            case BOSS_STATE.IDLE:

                break;

            case BOSS_STATE.READY_HIT:
                GameManager.Instance.bossState = BOSS_STATE.READY_HIT;
                AnimPlay(CHAR_ANIM.READY_HIT);
                timerControl.SetDuration(Define.TIME_AUTO_ATTACK);
                break;
            case BOSS_STATE.HITTED:                
                AnimPlay(CHAR_ANIM.HITTED);
                break;
            case BOSS_STATE.KNOCK_OUT:
                mAnimator.applyRootMotion = true;
                AnimPlay(CHAR_ANIM.KNOCK_OUT);
                break;

            case BOSS_STATE.HIT:
                timerControl.SetDuration(Define.TIME_FINISH_HIT);
                AnimPlay(CHAR_ANIM.HIT);
                break;

            case BOSS_STATE.FINISH_HIT:
                GameManager.Instance.PlayerHitted();
                GameManager.Instance.playerState = PLAYER_STATE.HITTED;
                break;

            case BOSS_STATE.KICK:
                timerControl.SetDuration(Define.TIME_FINISH_KICK);
                AnimPlay(CHAR_ANIM.KICK);
                break;

            case BOSS_STATE.FINISH_KICK:
                GameManager.Instance.PlayerHitted();
                GameManager.Instance.playerState = PLAYER_STATE.KNOCK_OUT;
                break;
        }
    }

    private void AnimPlay(string animName)
    {
        if(mAnimName == animName)
        {
            return;
        }
        mAnimName = animName;
        mAnimator.Play(animName);
    }
}

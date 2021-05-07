using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Animator mAnimator;

    [SerializeField]
    private PLAYER_STATE bossState;
    private string mAnimName;
    private Timer timerControl = new Timer();
    private Vector3 targetMultiScorePos;
    private int indexScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        bossState = PLAYER_STATE.READY;
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
            case PLAYER_STATE.READY:
                if(GameManager.Instance.State == STATE.FIGHT)
                {
                    SetState(PLAYER_STATE.PRE_READY_HIT);                    
                }
                break;

            case PLAYER_STATE.PRE_READY_HIT:
                timerControl.Update(Time.deltaTime);
                if(timerControl.JustFinished())
                {
                    SetState(PLAYER_STATE.READY_HIT);
                }
                break;

            case PLAYER_STATE.READY_HIT:
                if(GameManager.Instance.bossState == PLAYER_STATE.READY_HIT && GameManager.Instance.playerState == PLAYER_STATE.READY_HIT)
                {
                    timerControl.Update(Time.deltaTime);
                    if(timerControl.JustFinished())
                    {
                        if(GameManager.Instance.IsPlayerLastHit())
                        {
                            SetState(PLAYER_STATE.KICK);
                        }
                        else
                        {
                            SetState(PLAYER_STATE.HIT);
                        }
                    }
                }
                else
                {
                    switch(GameManager.Instance.playerState)
                    {
                        case PLAYER_STATE.FINISH_HIT:
                            SetState(PLAYER_STATE.HITTED);
                            break;
                        case PLAYER_STATE.FINISH_KICK:
                            SetState(PLAYER_STATE.KNOCK_OUT);
                            break;
                    }
                }
                break;
            case PLAYER_STATE.HITTED:
                if (!GameManager.Instance.AnimatorIsPlaying(mAnimator, CHAR_ANIM.HITTED))
                {
                    SetState(PLAYER_STATE.READY_HIT);
                }
                break;

            case PLAYER_STATE.KNOCK_OUT:
                transform.position = Vector3.Lerp(transform.position, targetMultiScorePos, Time.deltaTime);
                if(transform.position.z >= LevelManager.Instance.listMultiScore[indexScore].transform.position.z - 0.5f)
                {
                    if(!LevelManager.Instance.listMultiScore[indexScore].isActive)
                    {
                        LevelManager.Instance.listMultiScore[indexScore].UpdateActive();
                        indexScore++;
                    }
                }
                break;

            case PLAYER_STATE.HIT:
                timerControl.Update(Time.deltaTime);
                if(timerControl.JustFinished())
                {
                    SetState(PLAYER_STATE.FINISH_HIT);
                }
                break;
            case PLAYER_STATE.FINISH_HIT:
                if(!GameManager.Instance.AnimatorIsPlaying(mAnimator, CHAR_ANIM.HIT))
                {
                    SetState(PLAYER_STATE.READY_HIT);
                }
                break;

            case PLAYER_STATE.KICK:
                timerControl.Update(Time.deltaTime);
                if(timerControl.JustFinished())
                {
                    SetState(PLAYER_STATE.FINISH_KICK);
                }
                break;

            case PLAYER_STATE.FINISH_KICK:
                if(!GameManager.Instance.AnimatorIsPlaying(mAnimator, CHAR_ANIM.KICK))
                {
                    SetState(PLAYER_STATE.READY_HIT);
                }
                break;
        }
    }

    private void SetState(PLAYER_STATE state)
    {
        bossState = state;
        GameManager.Instance.bossState = state;
        switch(state)
        {
            case PLAYER_STATE.READY:

                break;

            case PLAYER_STATE.PRE_READY_HIT:
                timerControl.SetDuration(Define.TIME_PRE_READY_HIT);
                break;

            case PLAYER_STATE.READY_HIT:
                GameManager.Instance.bossState = PLAYER_STATE.READY_HIT;
                AnimPlay(CHAR_ANIM.READY_HIT);
                timerControl.SetDuration(Define.TIME_AUTO_ATTACK);
                break;
            case PLAYER_STATE.HITTED:
                GameManager.Instance.bossState = PLAYER_STATE.HITTED;
                AnimPlay(CHAR_ANIM.HITTED);
                break;
            case PLAYER_STATE.KNOCK_OUT:
                GameManager.Instance.bossState = PLAYER_STATE.KNOCK_OUT;
                AnimPlay(CHAR_ANIM.KNOCK_OUT);
                GameManager.Instance.mMultiScore = Random.Range(Define.MULTI_SCORE_LOCK_ID / 2, Define.MULTI_SCORE_LOCK_ID - 1);
                Debug.Log("GameManager.Instance.mMultiScore:" + GameManager.Instance.mMultiScore);
                for(int i = 0; i < LevelManager.Instance.listMultiScore.Count; i++)
                {
                    if(LevelManager.Instance.listMultiScore[i].ID == GameManager.Instance.mMultiScore)
                    {
                        targetMultiScorePos = LevelManager.Instance.listMultiScore[i].transform.position;
                    }
                }
                break;

            case PLAYER_STATE.HIT:
                GameManager.Instance.bossState = PLAYER_STATE.HIT;
                timerControl.SetDuration(Define.TIME_FINISH_HIT);
                AnimPlay(CHAR_ANIM.HIT);
                break;

            case PLAYER_STATE.FINISH_HIT:
                GameManager.Instance.PlayerHitted();
                //GameManager.Instance.playerState = PLAYER_STATE.HITTED;
                GameManager.Instance.bossState = PLAYER_STATE.FINISH_HIT;
                break;

            case PLAYER_STATE.KICK:
                timerControl.SetDuration(Define.TIME_FINISH_KICK);
                AnimPlay(CHAR_ANIM.KICK);
                GameManager.Instance.bossState = PLAYER_STATE.KICK;
                break;

            case PLAYER_STATE.FINISH_KICK:
                GameManager.Instance.bossState = PLAYER_STATE.FINISH_KICK;
                GameManager.Instance.PlayerHitted();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public SkinnedMeshRenderer sm;
    public List<Material> listColor;
    public CharacterController characterController;
    public Animator mAnimator;

    private CHAR_COLOR mCharColor = CHAR_COLOR.GREEN;
    private Vector3 mMoveVector;
    private Vector3 mRotVector;

    [SerializeField]
    private float mSpeed = 5f;
    private float mSpeedEndRun = 3f;
    [SerializeField]
    private float mRotSpeed = 50f;
    [SerializeField]
    private bool isRotation = false;
    private float mSpeedControl = 1.5f;
    float verticalVelocity = -10f;
    private float _movementForce = 10f;
    [SerializeField]
    private PLAYER_STATE playerState = PLAYER_STATE.NONE;
    private string mAnimName;
    private Timer timerControl = new Timer();

    const float MAX_ROTATION_Y = 0.15f;
    const float MAX_X = 3f;
    const float MAX_MOUSE_X = 5f;
    const float SCALE_UNIT = 0.05f;        

    private void Start()
    {
        //SetState(PLAYER_STATE.READY);
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    mColor++;
        //    if(mColor == CHAR_COLOR.COUNT)
        //    {
        //        mColor = CHAR_COLOR.GREEN;
        //    }
        //    UpdateColor(mColor);
        //}

        switch (playerState)
        {
            case PLAYER_STATE.READY:
                if(Input.GetMouseButtonDown(0) && GameManager.Instance.State == STATE.READY)
                {
                    SetState(PLAYER_STATE.START);
                }

                if(Input.GetKeyDown(KeyCode.Space))
                {
                    switch(mAnimName)
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
                break;
            case PLAYER_STATE.RUN:
                mMoveVector = Vector3.zero;
                mMoveVector.z = mSpeed;
                mMoveVector.x = Input.GetAxisRaw("Horizontal") * mSpeed;
                mRotVector = Vector3.zero;
                mMoveVector.y = verticalVelocity;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, mRotSpeed * Time.deltaTime);
                UpdateMove();
                if (Input.GetMouseButtonDown(0))
                {
                    SetState(PLAYER_STATE.CONTROL);
                }
                break;

            case PLAYER_STATE.START:
                mMoveVector = Vector3.zero;
                mMoveVector.z = mSpeed;
                mMoveVector.x = Input.GetAxisRaw("Horizontal") * mSpeed;
                mRotVector = Vector3.zero;
                mMoveVector.y = verticalVelocity;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, mRotSpeed * Time.deltaTime);
                UpdateMove();
                if (Input.GetMouseButtonUp(0))
                {
                    SetState(PLAYER_STATE.RUN);
                } else
                {
                    timerControl.Update(Time.deltaTime);
                    if(timerControl.JustFinished())
                    {
                        SetState(PLAYER_STATE.CONTROL);
                    }
                }
                break;

            case PLAYER_STATE.CONTROL:
                mMoveVector = Vector3.zero;
                mMoveVector.x = Input.GetAxisRaw("Horizontal") * mSpeed;
                mMoveVector.z = mSpeed;
                mRotVector = Vector3.zero;
                mMoveVector.y = verticalVelocity;
                mMoveVector.x = Input.GetAxis("Mouse X");
                mMoveVector.x = Mathf.Clamp(mMoveVector.x, -MAX_MOUSE_X, MAX_MOUSE_X);
                mMoveVector.x = mMoveVector.x * mSpeedControl;

                mRotVector.y = Input.GetAxis("Mouse X");
                mRotVector.y = Mathf.Clamp(mMoveVector.x, -MAX_MOUSE_X, MAX_MOUSE_X);
                mRotVector.y = mRotVector.y * mRotSpeed * Time.deltaTime;

                transform.Rotate(mRotVector);
                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, mRotSpeed * Time.deltaTime);
                Quaternion quaternion = transform.rotation;
                quaternion.y = Mathf.Clamp(quaternion.y, -MAX_ROTATION_Y, MAX_ROTATION_Y);
                transform.rotation = quaternion;

                UpdateMove();
                if (Input.GetMouseButtonUp(0))
                {
                    SetState(PLAYER_STATE.RUN);
                }
                break;

            case PLAYER_STATE.END_RUN:
                transform.position = Vector3.MoveTowards(transform.position, LevelManager.Instance.transPlayer.position, mSpeedEndRun * Time.deltaTime);
                //transform.position = Vector3.Lerp(transform.position, LevelManager.Instance.transPlayer.position, mSpeedEndRun * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, mRotSpeed * Time.deltaTime);
                float distance = Vector3.Distance(transform.position, LevelManager.Instance.transPlayer.position);
                if (distance <= Define.DISTANCE_TARGET_2)
                {
                    if(GameManager.Instance.State != STATE.PRE_FIGHT)
                    {
                        GameManager.Instance.State = STATE.PRE_FIGHT;
                        CameraMgr.Instance.UpdateCamera();
                        AnimPlay(CHAR_ANIM.IDLE);
                    }
                }
                if ( distance <= Define.DISTANCE_TARGET_1)
                {
                    transform.position = LevelManager.Instance.transPlayer.position;
                    SetState(PLAYER_STATE.PRE_READY_HIT);
                    GameManager.Instance.State = STATE.READY_FIGHT;
                }
                break;

            case PLAYER_STATE.PRE_READY_HIT:
                timerControl.Update(Time.deltaTime);
                if(timerControl.JustFinished())
                {
                    GameManager.Instance.State = STATE.FIGHT;
                    SetState(PLAYER_STATE.READY_HIT);
                }
                break;

            case PLAYER_STATE.READY_HIT:
                if(GameManager.Instance.playerState == PLAYER_STATE.READY_HIT && GameManager.Instance.bossState == PLAYER_STATE.READY_HIT)
                {
                    if(Input.GetMouseButtonDown(0))
                    {
                        if(GameManager.Instance.IsBossLastHit())
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
                    switch(GameManager.Instance.bossState)
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

            case PLAYER_STATE.HIT:
                timerControl.Update(Time.deltaTime);
                if(timerControl.JustFinished())
                {
                    SetState(PLAYER_STATE.FINISH_HIT);
                }
                break;

            case PLAYER_STATE.FINISH_HIT:
                if (!GameManager.Instance.AnimatorIsPlaying(mAnimator, CHAR_ANIM.HIT))
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

            case PLAYER_STATE.HITTED:
                if(!GameManager.Instance.AnimatorIsPlaying(mAnimator, CHAR_ANIM.HITTED))
                {
                    SetState(PLAYER_STATE.READY_HIT);
                }
                break;

            case PLAYER_STATE.KNOCK_OUT:
                timerControl.Update(Time.deltaTime);
                if (timerControl.IsDone())
                {
                    if (!GameManager.Instance.AnimatorIsPlaying(mAnimator, CHAR_ANIM.KNOCK_OUT))
                    {
                        SetState(PLAYER_STATE.GAME_OVER);
                    }
                }
                break;

            case PLAYER_STATE.GAME_OVER:
                timerControl.Update(Time.deltaTime);
                if(timerControl.JustFinished())
                {
                    SetState(PLAYER_STATE.RESTART_LEVEL);                    
                }
                break;
        }
        
    }

    public void SetState(PLAYER_STATE state)
    {
        playerState = state;
        GameManager.Instance.playerState = state;
        switch(playerState)
        {
            case PLAYER_STATE.READY:
                AnimPlay(CHAR_ANIM.IDLE);
                mCharColor = CHAR_COLOR.GREEN;
                UpdateColor(mCharColor);
                break;

            case PLAYER_STATE.START:
                mAnimator.Play(CHAR_ANIM.RUNNING);
                GameManager.Instance.State = STATE.START_GAME;
                CameraMgr.Instance.timerControl.SetDuration(Define.TIME_CAMERA_START);
                CameraMgr.Instance.UpdateCamera();
                timerControl.SetDuration(Define.TIME_START_RUN);
                break;

            case PLAYER_STATE.CONTROL:
                break;
            case PLAYER_STATE.END_RUN:
                break;

            case PLAYER_STATE.PRE_READY_HIT:
                timerControl.SetDuration(Define.TIME_PRE_READY_HIT);
                break;

            case PLAYER_STATE.READY_HIT:
                GameManager.Instance.playerState = PLAYER_STATE.READY_HIT;
                AnimPlay(CHAR_ANIM.READY_HIT);
                break;

            case PLAYER_STATE.HIT:
                GameManager.Instance.playerState = PLAYER_STATE.HIT;
                timerControl.SetDuration(Define.TIME_FINISH_HIT);
                AnimPlay(CHAR_ANIM.HIT);
                break;
            case PLAYER_STATE.FINISH_HIT:
                GameManager.Instance.playerState = PLAYER_STATE.FINISH_HIT;
                GameManager.Instance.BossHitted();
                if (GameManager.Instance.IsBossLastHit())
                {
                    LevelManager.Instance.ActiveBoardMultiScore();
                }
                break;

            case PLAYER_STATE.KICK:
                CameraMgr.Instance.UpdateCamera();
                GameManager.Instance.playerState = PLAYER_STATE.KICK;
                timerControl.SetDuration(Define.TIME_FINISH_KICK);
                AnimPlay(CHAR_ANIM.KICK);
                break;
            case PLAYER_STATE.FINISH_KICK:
                GameManager.Instance.playerState = PLAYER_STATE.FINISH_KICK;
                break;

            case PLAYER_STATE.HITTED:
                GameManager.Instance.playerState = PLAYER_STATE.HITTED;
                AnimPlay(CHAR_ANIM.HITTED);
                break;

            case PLAYER_STATE.KNOCK_OUT:
                GameManager.Instance.playerState = PLAYER_STATE.KNOCK_OUT;
                GameManager.Instance.State = STATE.PLAYER_KNOCK_OUT;
                AnimPlay(CHAR_ANIM.KNOCK_OUT);
                timerControl.SetDuration(Define.TIME_DELAY_UPDATE);
                break;

            case PLAYER_STATE.GAME_OVER:
                GameManager.Instance.State = STATE.GAME_OVER;
                timerControl.SetDuration(Define.TIME_DELAY_END_GAME);
                break;

            case PLAYER_STATE.RESTART_LEVEL:
                GameManager.Instance.ReStartLevel();
                break;
        }
    }

    private void UpdateColor(CHAR_COLOR charColor)
    {
        Material[] mats = sm.materials;
        mats[0] = listColor[(int)charColor];
        sm.materials = mats;
    }

    private void UpdateMove()
    {
        characterController.Move(mMoveVector * Time.deltaTime);
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -MAX_X, MAX_X);
        transform.position = pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        CHAR_COLOR charColor;
        switch (other.tag)
        {
            case TAG.COLOR:
                charColor = other.GetComponent<ColorChanger>().mCharColor;
                if (charColor == mCharColor)
                {
                    transform.localScale += new Vector3(SCALE_UNIT, SCALE_UNIT, SCALE_UNIT);
                }
                else
                {
                    transform.localScale -= new Vector3(SCALE_UNIT, SCALE_UNIT, SCALE_UNIT);
                }
                Destroy(other.gameObject);
                break;
            case TAG.STATION:
                charColor = other.GetComponent<ColorStation>().mCharColor;
                mCharColor = charColor;
                UpdateColor(mCharColor);
                break;

            case TAG.DIAMOND:
                Destroy(other.gameObject);
                break;

            case TAG.END_RUN:
                if(playerState != PLAYER_STATE.END_RUN)
                {
                    SetState(PLAYER_STATE.END_RUN);
                    GameManager.Instance.State = STATE.END_RUN;
                    CameraMgr.Instance.UpdateCamera();
                }
                break;
        }
    }

    private void AnimPlay(string animName)
    {
        mAnimName = animName;
        mAnimator.Play(animName);
    }
}

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
    private float mSpeed = 5f;
    [SerializeField]
    private float mRotSpeed = 500f;
    [SerializeField]
    private bool isRotation = false;
    private float mSpeedControl = 25f;
    float verticalVelocity = -10f;
    private float _movementForce = 10f;
    [SerializeField]
    private PLAYER_STATE playerState = PLAYER_STATE.READY;

    const float MAX_ROTATION_Y = 0.15f;
    const float MAX_X = 3f;
    const float SCALE_UNIT = 0.1f;

    enum PLAYER_STATE
    {
        READY,
        RUN,
        CONTROL,
    }

    private void Start()
    {
        SetState(PLAYER_STATE.READY);
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
                if(Input.GetMouseButtonDown(0))
                {
                    SetState(PLAYER_STATE.CONTROL);
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

            case PLAYER_STATE.CONTROL:
                mMoveVector = Vector3.zero;
                mMoveVector.x = Input.GetAxisRaw("Horizontal") * mSpeed;
                mMoveVector.z = mSpeed;
                mRotVector = Vector3.zero;
                mMoveVector.y = verticalVelocity;
                mMoveVector.x = Input.GetAxis("Mouse X") * mSpeedControl;
                mRotVector.y = Input.GetAxis("Mouse X") * mRotSpeed * Time.deltaTime;

                transform.Rotate(mRotVector);

                Quaternion quaternion = transform.rotation;
                quaternion.y = Mathf.Clamp(quaternion.y, -MAX_ROTATION_Y, MAX_ROTATION_Y);
                transform.rotation = quaternion;

                UpdateMove();
                if (Input.GetMouseButtonUp(0))
                {
                    SetState(PLAYER_STATE.RUN);
                }
                break;
        }
        
    }

    private void SetState(PLAYER_STATE state)
    {
        playerState = state;
        switch(playerState)
        {
            case PLAYER_STATE.READY:
                mAnimator.Play(CHAR_ANIM.IDLE);
                break;

            case PLAYER_STATE.CONTROL:
                mAnimator.Play(CHAR_ANIM.RUNNING);
                if(GameManager.Instance.State == STATE.READY)
                {
                    GameManager.Instance.State = STATE.START_GAME;
                }
                else if (GameManager.Instance.State == STATE.START_GAME)
                {
                    GameManager.Instance.State = STATE.PLAY_GAME;
                }
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
        }
    }

}

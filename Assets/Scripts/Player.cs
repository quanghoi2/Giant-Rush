using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public SkinnedMeshRenderer sm;
    public List<Material> listColor;
    public CharacterController characterController;

    private CHAR_COLOR mColor = CHAR_COLOR.GREEN;
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
    private PLAYER_STATE playerState = PLAYER_STATE.RUN;

    const float MAX_ROTATION_Y = 0.3f;

    enum PLAYER_STATE
    {
        NONE,
        RUN,
        CONTROL,
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            mColor++;
            if(mColor == CHAR_COLOR.COUNT)
            {
                mColor = CHAR_COLOR.GREEN;
            }
            UpdateColor(mColor);
        }

        mMoveVector = Vector3.zero;
        mMoveVector.x = Input.GetAxisRaw("Horizontal") * mSpeed;
        mMoveVector.z = 0;
        mRotVector = Vector3.zero;
        switch (playerState)
        {
            case PLAYER_STATE.RUN:
                //mMoveVector.z = mSpeed;
                mMoveVector.y = verticalVelocity;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, mRotSpeed * Time.deltaTime);
                characterController.Move(mMoveVector * Time.deltaTime);
                if (Input.GetMouseButtonDown(0))
                {
                    SetState(PLAYER_STATE.CONTROL);
                }
                break;

            case PLAYER_STATE.CONTROL:
                mMoveVector.y = verticalVelocity;
                mMoveVector.x = Input.GetAxis("Mouse X") * mSpeedControl;
                mRotVector.y = Input.GetAxis("Mouse X") * mRotSpeed * Time.deltaTime;

                transform.Rotate(mRotVector);

                Quaternion quaternion = transform.rotation;
                quaternion.y = Mathf.Clamp(quaternion.y, -MAX_ROTATION_Y, MAX_ROTATION_Y);
                transform.rotation = quaternion;

                characterController.Move(mMoveVector * Time.deltaTime);
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
    }

    private void UpdateColor(CHAR_COLOR charColor)
    {
        Material[] mats = sm.materials;
        mats[0] = listColor[(int)charColor];
        sm.materials = mats;
    }

    private void UpdateRotation()
    {
        if(transform.rotation != Quaternion.Euler(Vector3.zero))
        {
            transform.rotation = Quaternion.identity;
        }
    }
}

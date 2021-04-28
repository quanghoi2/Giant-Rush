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
    private float mSpeed = 5f;
    private float mSpeedControl = 25f;
    float verticalVelocity = -10f;
    private float _movementForce = 10f;
    private PLAYER_STATE playerState = PLAYER_STATE.RUN;

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
        switch (playerState)
        {
            case PLAYER_STATE.RUN:
                mMoveVector.z = mSpeed;
                if (Input.GetMouseButtonDown(0))
                {
                    SetState(PLAYER_STATE.CONTROL);
                }
                break;

            case PLAYER_STATE.CONTROL:
                mMoveVector.x = Input.GetAxis("Mouse X") * mSpeedControl;
                if (Input.GetMouseButtonUp(0))
                {
                    SetState(PLAYER_STATE.RUN);
                }
                break;
        }
        mMoveVector.y = verticalVelocity;
        
        characterController.Move(mMoveVector * Time.deltaTime);
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
}

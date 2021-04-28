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
    private PLAYER_STATE playerState = PLAYER_STATE.NONE;

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
        //mMoveVector.x = Input.GetAxisRaw("Horizontal") * mSpeed;
        mMoveVector.x = Input.GetAxis("Mouse X") * mSpeedControl;
        mMoveVector.y = verticalVelocity;
        mMoveVector.z = 0;
        
        characterController.Move(mMoveVector * Time.deltaTime);
    }

    private void UpdateColor(CHAR_COLOR charColor)
    {
        Material[] mats = sm.materials;
        mats[0] = listColor[(int)charColor];
        sm.materials = mats;
    }
}

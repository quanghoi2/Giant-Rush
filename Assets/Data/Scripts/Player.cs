using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public SkinnedMeshRenderer sm;
    public List<Material> listColor;
    public Rigidbody mRigidBody;

    private CHAR_COLOR mColor = CHAR_COLOR.GREEN;
    private Vector3 mMoveVector;
    private float mSpeed = 5f;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            mColor++;
            if(mColor == CHAR_COLOR.COUNT)
            {
                mColor = CHAR_COLOR.GREEN;
            }
            //UpdateColor(mColor);
        }

        mMoveVector = Vector3.zero;
        mMoveVector.x = Input.GetAxisRaw("Horizontal") * mSpeed * Time.deltaTime;
        mMoveVector.z = 0;
        mMoveVector.y = 0;
        //mRigidBody.AddForce(mMoveVector * Time.deltaTime, ForceMode.VelocityChange);
        mRigidBody.velocity += mMoveVector;
    }

    private void UpdateColor(CHAR_COLOR charColor)
    {
        Material[] mats = sm.materials;
        mats[0] = listColor[(int)charColor];
        sm.materials = mats;
    }
}

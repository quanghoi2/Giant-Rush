using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mace : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(TAG.PLAYER) && GameManager.Instance.State != STATE.GAME_OVER && GameManager.Instance.State != STATE.PLAYER_KNOCK_OUT)
        {
            Debug.Log("AAAAAAAAAAAAAAAAAAA");
            GameManager.Instance.mPlayer.SetState(PLAYER_STATE.KNOCK_OUT);
        }
    }
}

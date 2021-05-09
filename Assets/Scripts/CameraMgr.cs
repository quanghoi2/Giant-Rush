using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMgr : MonoBehaviour
{
    public static CameraMgr Instance;
    public Transform cameraContainer;

    [HideInInspector]
    public Timer timerControl = new Timer();

    private List<GameObject> listCamera;
    private int cameraIndex = 0;
    [SerializeField]
    private STATE mState = STATE.NONE;

    const string PRE_FIX = "CameraLevel_";

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetState(STATE.PRE_LOAD);
        //SetState(STATE.READY);        
    }

    void Update()
    {
        switch (GameManager.Instance.State)
        {
            case STATE.START_GAME:
                timerControl.Update(Time.deltaTime);
                if(timerControl.JustFinished())
                {
                    GameManager.Instance.State = STATE.PLAY_GAME;
                }
                break;
            case STATE.PLAY_GAME:
                break;
            case STATE.END_RUN:
                break;
        }

        UpdateState();
    }

    public void UpdateCamera()
    {
        listCamera[cameraIndex].SetActive(false);
        cameraIndex += 1;
        listCamera[cameraIndex].SetActive(true);
    }

    public STATE State
    {
        get { return mState; }
        set { mState = value; }
    }

    public void SetState(STATE state)
    {
        mState = state;
        switch(state)
        {
            case STATE.PRE_LOAD:
                foreach(Transform child in cameraContainer)
                {
                    Destroy(child.gameObject);
                }
                break;
            case STATE.LOAD:
                int level = ProfileMgr.Instance.Level;
                string namePrefabCam = PRE_FIX;
                if(level < 10)
                {
                    namePrefabCam += "0" + level;
                }
                else
                {
                    namePrefabCam += level;
                }
                GameObject prefabCam = Resources.Load(namePrefabCam) as GameObject;
                if(prefabCam != null)
                {
                    GameObject camLevel = Instantiate(prefabCam, cameraContainer);
                }
                break;

            case STATE.READY:
                listCamera = cameraContainer.GetChild(0).GetComponent<CameraLevel>().listCamera;
                for (int i = 0; i < listCamera.Count; i++)
                {
                    listCamera[i].SetActive(i == 0);

                }
                break;
        }
    }

    private void UpdateState()
    {
        switch(mState)
        {
            case STATE.PRE_LOAD:
                cameraIndex = 0;
                if (cameraContainer.childCount == 0)
                {
                    SetState(STATE.LOAD);
                }
                break;

            case STATE.LOAD:
                if(cameraContainer.childCount > 0)
                {
                    SetState(STATE.READY);
                }
                break;
        }
    }
}

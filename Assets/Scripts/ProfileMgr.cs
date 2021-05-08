using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Profile
{
    public int mLevel = 1;
}

public class ProfileMgr : Singleton<ProfileMgr>
{
    private Profile mProfile = new Profile();

    const string KEY_PROFILE = "PROFILE";
    private void Start()
    {
        LoadProfile();
    }

    public int Level
    {
        get { return mProfile.mLevel; }
        set { mProfile.mLevel = value; }
    }

    public void SaveProfile()
    {
        string json = JsonUtility.ToJson(mProfile);
        PlayerPrefs.SetString(KEY_PROFILE, json);
    }

    public void LoadProfile()
    {
        if(PlayerPrefs.HasKey(KEY_PROFILE))
        {
            string json = PlayerPrefs.GetString(KEY_PROFILE);
            mProfile = JsonUtility.FromJson<Profile>(json);
        }
        else
        {
            SaveProfile();
        }
    }
}

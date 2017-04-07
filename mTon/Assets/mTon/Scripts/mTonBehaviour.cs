using UnityEngine;
using System.Collections.Generic;
using System;
[ExecuteInEditMode]
public class mTonBehaviour : MonoBehaviour {
    public string mTemplate;
    public List<mTonInjection> Injecttion = new List<mTonInjection>();
    Dictionary<string, mTonInjection> mTemp = new Dictionary<string, mTonInjection>();
    public mTonInjection GetInject(string key)
    {
        
        if(mTemp.Count == 0)
        {
            foreach(var t in Injecttion)
            {
                mTemp[t.mKey] = t;
            }
        }
        mTonInjection o = null;
        mTemp.TryGetValue(key, out o);
        return o;
    }
}
[Serializable]
public class mTonInjection
{

    public string mKey;
    public GameObject mGo;
    public float mFloat;
    public int mInt;
    public bool mBool;
    public string mText;


}

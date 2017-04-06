using UnityEngine;
using System.Collections.Generic;
using System;
public class mTonBehaviour : MonoBehaviour {
    public string mTemplate;
    [SerializeField]
    public List<mTonInjection> mInjecttion = new List<mTonInjection>();
    public List<mTonInjection> Injecttion
    {
        get
        {
            return mInjecttion;
        }
    }
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
    [SerializeField]
    public string key;
    public string mKey
    {
        get
        {
            return key;
        }
        set
        {
            key = value;
        }
    }
    [SerializeField]
    public GameObject go;

    public GameObject mGo
    {
        get
        {
            return go;
        }
        set
        {
            go = value;
        }
    }

    [SerializeField]
    public float mf;
    public float mFloat
    {
        get
        {
            return mf;
        }
        set
        {
            mf = value;
        }
    }

    [SerializeField]
    public int mI;
    public int mInt
    {
        get
        {
            return mI;
        }
        set
        {
            mI = value;
        }
    }
    [SerializeField]
    public bool mB;
    public bool mBool
    {
        get
        {
            return mB;
        }
        set
        {
            mB = value;
        }
    }


    [SerializeField]
    public string mT;
    public string mText
    {
        set
        {
            mT = value;
        }
        get
        {
            return mT;
        }


    }


}

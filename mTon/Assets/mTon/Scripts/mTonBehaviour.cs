using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
[ExecuteInEditMode]
public class mTonBehaviour : MonoBehaviour {
    public string mTemplate;
  //  public List<mTonInjection> Injecttion = new List<mTonInjection>();
    public List<mTonGoInjection> mGoInjection = new List<mTonGoInjection>();
    public List<mTonFloatInjection> mFloatInjection = new List<mTonFloatInjection>();
    public List<mTonIntInjection> mIntInjection = new List<mTonIntInjection>();
    public List<mTonBoolInjection> mBoolInjection = new List<mTonBoolInjection>();
    public List<mTonStringInjection> mStringInjection = new List<mTonStringInjection>();
    public List<GameObject> mSelfDefine = new List<GameObject>();
    Dictionary<string, mTonInjection> mFinal = new Dictionary<string, mTonInjection>();
    bool mInit = false;
    void Init()
    {
        if (!mInit)
        {
            mInit = true;
            Push(mGoInjection);
            Push(mFloatInjection);
            Push(mIntInjection);
            Push(mBoolInjection);
            Push(mStringInjection);
        }
    }
    public int get_int(string key)
    {
        Init();
        mTonInjection o = null;
        if (mFinal.TryGetValue(key, out o))
        {
            return o.AsInt;
        }
        return 0;
    }

    public float get_float(string key)
    {
        Init();
        mTonInjection o = null;
        if (mFinal.TryGetValue(key, out o))
        {
            return o.AsFloat;
        }
        return 0;
    }

    public bool get_bool(string key)
    {
        Init();
        mTonInjection o = null;
        if (mFinal.TryGetValue(key, out o))
        {
            return o.AsBool;
        }
        return false;
    }

    public string get_string(string key)
    {
        Init();
        mTonInjection o = null;
        if (mFinal.TryGetValue(key, out o))
        {
            return o.AsString;
        }
        return null;
    }
    public System.Object get_object(string key,string szType)
    {
        Init();
        mTonInjection o = null;
        if (mFinal.TryGetValue(key, out o))
        {
            GameObject go = o.AsGameObject;
            if(null != go)
            {
                if (szType.ToLower() == "gameobject")
                    return go;
                else
                {
                    return go.GetComponent(szType);
                }
            }
        }
        return null; ;
    }
    void Push<T>(List<T> a) where T : mTonInjection
    {
        foreach(var k in a)
        {
            mFinal[k.mKey] = k;
        }
    }
 
}

[Serializable]
public  class mTonInjection
{
    public string mKey;
    public virtual float AsFloat
    {
        get
        {
            return 0;
        }

    }

    public virtual int AsInt
    {
        get
        {
            return 0;
        }

    }

    public virtual string AsString
    {
        get
        {
            return string.Empty;
        }

    }

    public virtual bool AsBool
    {
        get
        {
            return false;
        }

    }

    public virtual GameObject AsGameObject
    {
        get
        {
            return null;
        }

    }
}

[Serializable]
public class mTonGoInjection : mTonInjection
{
    public GameObject mV;
    public override GameObject AsGameObject
    {
        get
        {
            return mV;
        }
    }

}

[Serializable]
public class mTonFloatInjection : mTonInjection
{
    public float mV;
    public override float AsFloat
    {
        get
        {
            return mV;
        }
    }
}

[Serializable]
public class mTonIntInjection : mTonInjection
{
 
    public int mV;
    public override int AsInt
    {
        get
        {
            return mV;
        }
    }
}

[Serializable]
public class mTonBoolInjection : mTonInjection
{
    public bool mV;
    public override bool AsBool
    {
        get
        {
            return mV;
        }
    }
}

[Serializable]
public class mTonStringInjection :mTonInjection
{
    public string mV;
    public override string AsString
    {
        get
        {
            return mV;
        }
    }

}


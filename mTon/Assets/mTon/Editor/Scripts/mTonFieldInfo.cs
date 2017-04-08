using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class mTonFieldInfo
{

    public string mSzName;
    public string mSzType;
    public string mSzDes;

    public static Dictionary<string,mTonFieldInfo> Deserilize(string text)
    {
        Dictionary<string, mTonFieldInfo> ret = new Dictionary<string, mTonFieldInfo>();
        System.Object o = mTonMiniJSON.Json.Deserialize(text);
        List<System.Object> t = o as List<System.Object>;
        foreach (var k in t)
        {
            Dictionary<string, System.Object> cur = k as Dictionary<string, System.Object>;
            mTonFieldInfo fi = new mTonFieldInfo();
            if (cur.ContainsKey("field"))
                fi.mSzName = cur["field"].ToString();
            if (cur.ContainsKey("type"))
                fi.mSzType = cur["type"].ToString();
            if (cur.ContainsKey("des"))
                fi.mSzDes = cur["des"].ToString();

            ret[fi.mSzName] = fi;
            // mFields[k.Key] = cur;
        }
        return ret;
    }
         
}

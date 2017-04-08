using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class mTonTools  {

   public static bool IsValueType(string curType)
    {
        curType = curType.ToLower();
        return curType == "int"
                || curType == "float"
                || curType == "bool"
                || curType == "string";
    }
}

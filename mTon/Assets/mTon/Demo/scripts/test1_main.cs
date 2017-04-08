using UnityEngine;
using System.Collections;

public class test1_main : MonoBehaviour {

    // Use this for initialization
   public mTonBehaviour s;
	void Start () {
        test1 t = new test1();
        t.Init(s);
        t.Log();

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
public partial class test1
{
    public void Log()
    {
        Debug.Log(m1);

    }
}


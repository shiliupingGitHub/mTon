using UnityEngine;
public  partial class test1 : mTonBase 
{
	Light	m1;	//des
	public  void Init(mTonBehaviour s)
	{
		m1=(Light) s.get_object("m1","Light");

	}
}
using UnityEngine;
public  partial class test1 : mTonBase 
{
	Light	m2;
	int	m3;
	string	m4;
	bool	m5;
	GameObject	m6;
	public	override void Init(mTonBehaviour s)
	{
		if (null != s.GetInject("m2"))
			m2= s.GetInject("m2").mGo.GetComponent<Light>();

		m3= s.GetInject("m3").mInt;

		m4= s.GetInject("m4").mText;

		m5= s.GetInject("m5").mBool;

		m6= s.GetInject("m6").mGo;

	}
}
using UnityEngine;
public  partial class test1 : mTonBase 
{
	Light	m2;
	int	m3;
	string	m4;
	bool	m5;
	GameObject	m6;
	Animation	m7;
	public  void Init(mTonBehaviour s)
	{
		 mTonInjection temp = null;
		temp = s.GetInject("m2");
		if (null != temp)
		{
			if(temp.mGo != null)
				m2= temp.mGo.GetComponent<Light>();
		}

		temp = s.GetInject("m3");
		if (null != temp)
		{
			m3= temp.mInt;
		}

		temp = s.GetInject("m4");
		if (null != temp)
		{
			m4= temp.mText;
		}

		temp = s.GetInject("m5");
		if (null != temp)
		{
			m5= temp.mBool;
		}

		temp = s.GetInject("m6");
		if (null != temp)
		{
			m6= temp.mGo;
		}

		temp = s.GetInject("m7");
		if (null != temp)
		{
			if(temp.mGo != null)
				m7= temp.mGo.GetComponent<Animation>();
		}

	}
}
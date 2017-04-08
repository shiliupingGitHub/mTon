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
		m2=(Light) s.get_object("m2","Light");

		m3=s.get_int("m3");

		m4=s.get_string("m4");

		m5=s.get_bool("m5");

		m6=(GameObject) s.get_object("m6","GameObject");

		m7=(Animation) s.get_object("m7","Animation");

	}
}
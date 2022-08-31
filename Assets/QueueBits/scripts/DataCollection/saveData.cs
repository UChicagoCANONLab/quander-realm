using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

// for research data
//		DO NOT push to firebase and mix with non-research data

public class saveData : MonoBehaviour
{
#if UNITY_WEBGL == true && UNITY_EDITOR == false
	[DllImport("__Internal")]
	private static extern void QueueBitsGameSaved(string data);
#endif

	// Use this for initialization
	public static int Save(Data mydata)
	{
		string dataJson = JsonUtility.ToJson(mydata);
		// Debug.Log(dataJson);

#if UNITY_WEBGL == true && UNITY_EDITOR == false
		QueueBitsGameSaved(dataJson);
#endif

		return 0;
	}

}
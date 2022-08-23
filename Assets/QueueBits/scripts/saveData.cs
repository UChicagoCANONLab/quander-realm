using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public class saveData : MonoBehaviour
{
	[DllImport("__Internal")]
	private static extern void QueueBitsGameSaved(string data);

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
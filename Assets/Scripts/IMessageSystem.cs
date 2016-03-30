using UnityEngine;
using System.Collections;

public interface IMessageSystem
{
	//bool showMessage;

	IEnumerator ShowMessage(int situation);
	void MessageControl();

}

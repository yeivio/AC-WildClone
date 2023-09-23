using UnityEngine;
using System.Collections;

public class EnableDestroy : MonoBehaviour
// Allow the player to destroy the object
{

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			Destroy(gameObject,0.25F);
		}
	}
}


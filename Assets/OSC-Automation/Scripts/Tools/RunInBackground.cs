using UnityEngine;
using System.Collections;

public class RunInBackground : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        Application.runInBackground = true;
    }
}

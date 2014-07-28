using UnityEngine;
using System.Collections;

public class VictoryScript : MonoBehaviour
{
    public GUIStyle DuCul;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        const float labelWidth = 200;
        const float labelHeight = 70;
        GUI.Label(new Rect(Screen.width / 2f - labelWidth / 2f, Screen.height / 2f - labelHeight / 2f, labelWidth, labelHeight), "de mes fesses", this.DuCul);
    }
}

using UnityEngine;
using System.Collections;

public class VictoryScript : MonoBehaviour
{
    public GUIStyle Style;

    public float Scale = 2.0f;

    public bool Display;
    private Game Game;

	// Use this for initialization
	void Start ()
	{
	   // Style.fontSize = (int)(Style.fontSize * Scale);
	    Game = FindObjectOfType<Game>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (Display && Input.GetMouseButton(0))
	    {
            Destroy(Game.gameObject);
            Application.LoadLevel("MainMenu"); 
	    }

        if (Game.State == Game.GameState.Win)
        {
            Display = true;
        }
        else
        {
            Display = false;  
        }
	}

    void OnGUI()
    {
        if (Display)
        {
            const float labelWidth = 200;
            const float labelHeight = 70;
            GUI.Label(new Rect(Screen.width / 2f - labelWidth / 2f, Screen.height / 2f - labelHeight / 2f, labelWidth, labelHeight), "Victoire!", this.Style);            
        }
    }
}

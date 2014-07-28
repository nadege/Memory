using UnityEngine;
using System.Collections;

public class Particule : MonoBehaviour
{
    public ParticleSystem particleSystem;

	// Use this for initialization
    void Start ()
    {
        // Set the sorting layer of the particle system.
        //particleSystem.renderer.sortingLayerName = "foreground";
        particleSystem.renderer.sortingOrder = 2;
    }
}

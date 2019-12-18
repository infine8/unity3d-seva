using UnityEngine;
using System.Collections;

public class TextureTrailRope : MonoBehaviour
{
    public Transform StartPoint;
    public Transform EndPoint;

    public QuickRope2 Rope
    {
        get { return StartPoint.GetComponent<QuickRope2>(); }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

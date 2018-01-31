using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartieTank : MonoBehaviour {

    private Color nColor;
    
    void Awake() {
        GetComponent<Renderer>().material = new Material(GetComponent<Renderer>().sharedMaterial);
        nColor = GetComponent<Renderer>().material.color;
    }

    
    void OnMouseOver()
    {
        GetComponent<Renderer>().material.color = Color.Lerp(nColor, Color.blue, 0.5f);
    }

    void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = nColor;
    }

    public void Explode()
    {
        StartCoroutine(transform.GetComponent<TriangleExplosion>().SplitMesh(true));/*
        degat++;
        nColor = Color.Lerp(nColor, Color.black, 0.3f);
        GetComponent<Renderer>().material.color = nColor;
        if (degat == 5)
        {
            StartCoroutine(transform.GetComponent<TriangleExplosion>().SplitMesh(true));
        }*/
    }
}

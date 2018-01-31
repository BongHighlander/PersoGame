using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Case : MonoBehaviour {
    
    public PlayManager playManager;
    
    public TypeTerrains type;
    private Color nColor;
    private Color tempColor;
    private Boolean allume = false;

    public Dictionary<Vector3, int> voisins = new Dictionary<Vector3, int>();

    private void Awake()
    {
        GetComponent<Renderer>().material = new Material(GetComponent<Renderer>().sharedMaterial);
        nColor = GetComponent<Renderer>().material.color;
    }

    public void Configuration(PlayManager playManager, Vector3 position, TypeTerrains type, Dictionary<Vector3, int> voisins)
    {
        this.playManager = playManager;
        transform.SetPositionAndRotation(position, Quaternion.identity);
        this.voisins = voisins;
        this.type = type;
    }

    public void OnMouseDown()
    {
        playManager.ClicCase(transform.position);
    }

    void OnMouseOver()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }

    void OnMouseExit()
    {
        if (allume)
        {
            GetComponent<Renderer>().material.color = tempColor;
        } else
        {
            GetComponent<Renderer>().material.color = nColor;
        }
    }

    public void Allumer(Color color)
    {
        tempColor = color;
        GetComponent<Renderer>().material.color = tempColor;
        allume = true;
    }

    public void Eteindre()
    {
        GetComponent<Renderer>().material.color = nColor;
        allume = false;
    }


}
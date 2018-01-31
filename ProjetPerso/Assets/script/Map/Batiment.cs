using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Batiment : MonoBehaviour {

    PlayManager playManager;
    public void SetPlayManager(PlayManager p) { playManager = p; }

    private void Awake()
    {
        transform.parent.GetComponent<ParticleSystem>().Stop();
        playManager = PlayManager.getInstance();
    }

    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "missile")
        {
            print("missile trigger");
            if (playManager.tourIA)
            {
                playManager.TurnPlayer();
            }
            
            Destroy(other.gameObject);
            transform.parent.GetComponent<ParticleSystem>().Emit(15);
            StartCoroutine(transform.GetComponent<TriangleExplosion>().SplitMesh(true));  
        }
    }

    void OnMouseDown()
    {
        playManager.ClicShoot(transform.position);
        /*
        transform.parent.GetComponent<ParticleSystem>().Emit(15);
        StartCoroutine(transform.GetComponent<TriangleExplosion>().SplitMesh(true));
        */
    }
}

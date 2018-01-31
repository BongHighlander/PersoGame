using System;
using System.Collections.Generic;
using UnityEngine;

public class BatimentDistributeur : DistributeurTerrain
{

    public GameObject[] PlaineBatiment;

    public override void Awake()
    {
        Taux = Probas.BATIMENT;
        TypeTerrain = TypeTerrains.BATIMENT;
        listeTerrains = new List<TerrainModele>();
        RemplirListeTerrains(PlaineBatiment, -1, -1, -1, -1, 400);
    }

    internal override bool ConditionsSup(Dictionary<Vector3, int> voisins, Vector3 position)
    {
        if (position.x < 1 || position.z < 1)
        {
            return false;
        }
        return true;
    }
}

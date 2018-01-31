using System.Collections.Generic;
using UnityEngine;

public class PlaineDistributeur : DistributeurTerrain
{

    public GameObject[] Plain;

    public override void Awake()
    {
        Taux = Probas.PLAINE;
        TypeTerrain = TypeTerrains.PLAINE;
        listeTerrains = new List<TerrainModele>();
        RemplirListeTerrains(Plain, -1, -1, -1, -1, 400);
    }
}

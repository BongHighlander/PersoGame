using System.Collections.Generic;
using UnityEngine;

public class DistributionTerrain : MonoBehaviour
{

    private List<IDistributeurTerrain> listeDistributeurs;

    public void Setup()
    {
        listeDistributeurs = new List<IDistributeurTerrain>
        { //il faudrait probablement inverser le sens de l'injection pendant l'awake des Distributeurs
            GetComponent<PlaineDistributeur>(),
            GetComponent<RouteDistributeur>(),
            GetComponent<BatimentDistributeur>()
        };
    }


    public TerrainModele DonnerTerrain(Dictionary<Vector3, int> voisins, Vector3 position)
    {
        List<IDistributeurTerrain> listeDistributeursCompatibles = new List<IDistributeurTerrain>();
        int aleatoire = 0;
        listeDistributeurs.ForEach(delegate (IDistributeurTerrain distributeurTerrain)
        {
            if (distributeurTerrain.IsCompatible(voisins, position))
            {
                listeDistributeursCompatibles.Add(distributeurTerrain);
                aleatoire += distributeurTerrain.Taux;
            }
        });

        aleatoire = Random.Range(0, aleatoire);

        IDistributeurTerrain listeTerrains = listeDistributeursCompatibles[0];

        listeDistributeursCompatibles.ForEach(delegate (IDistributeurTerrain distributeurTerrain)
        {
            if (aleatoire > -1)
            {
                aleatoire -= distributeurTerrain.Taux;
                listeTerrains = distributeurTerrain;
            }
        });

        return listeTerrains.getTerrainCompatible(voisins, position);
    }
}



using System.Collections.Generic;
using UnityEngine;

public class RouteDistributeur : DistributeurTerrain
{

    public GameObject[] Carrefour;
    public GameObject[] CroisementTHaut;
    public GameObject[] CroisementTBas;
    public GameObject[] CroisementTGauche;
    public GameObject[] CroisementTDroite;
    public GameObject[] RouteHautBas;
    public GameObject[] RouteHautGauche;
    public GameObject[] RouteHautDroite;
    public GameObject[] RouteBasGauche;
    public GameObject[] RouteBasDroite;
    public GameObject[] RouteGaucheDroite;
    public GameObject[] FinHaut;
    public GameObject[] FinBas;
    public GameObject[] FinGauche;
    public GameObject[] FinDroite;
    
    public override void Awake()
    {
        Taux = Probas.ROUTE;
        TypeTerrain = TypeTerrains.ROUTE;
        listeTerrains = new List<TerrainModele>();
        RemplirListeTerrains(Carrefour, 1, 1, 1, 1, 1);
        RemplirListeTerrains(CroisementTHaut, -1, 1, 1, 1, 5);
        RemplirListeTerrains(CroisementTBas, 1, -1, 1, 1, 5);
        RemplirListeTerrains(CroisementTGauche, 1, 1, -1, 1, 5);
        RemplirListeTerrains(CroisementTDroite, 1, 1, 1, -1, 5);
        RemplirListeTerrains(RouteHautBas, 1, 1, -1, -1, 50);
        RemplirListeTerrains(RouteHautGauche, 1, -1, 1, -1, 7);
        RemplirListeTerrains(RouteHautDroite, 1, -1, -1, 1, 7);
        RemplirListeTerrains(RouteBasGauche, -1, 1, 1, -1, 7);
        RemplirListeTerrains(RouteBasDroite, -1, 1, -1, 1, 7);
        RemplirListeTerrains(RouteGaucheDroite, -1, -1, 1, 1, 50);
        RemplirListeTerrains(FinHaut, 1, -1, -1, -1, 1);
        RemplirListeTerrains(FinBas, -1, 1, -1, -1, 1);
        RemplirListeTerrains(FinGauche, -1, -1, 1, -1, 1);
        RemplirListeTerrains(FinDroite, -1, -1, -1, 1, 1);
    }
}

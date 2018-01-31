using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class DistributeurTerrain : MonoBehaviour, IDistributeurTerrain
{
    public List<TerrainModele> listeTerrains;
    public int Taux { get; set; }
    public TypeTerrains TypeTerrain { get; set; }

    public abstract void Awake();

    protected void RemplirListeTerrains(GameObject[] elements, int haut, int bas, int gauche, int droite, int chance)
    {
        int i = 0;
        for (i = 0; i < elements.Length; i++)
        {
            listeTerrains.Add(new TerrainModele(elements[i], haut, bas, gauche, droite, chance, TypeTerrain));
        }
    }


    public TerrainModele getTerrainCompatible(Dictionary<Vector3, int> voisins, Vector3 position)
    {
        int aleatoire = 0;
        List<TerrainModele> compatibles = getListTerrainCompatible(voisins);

        compatibles.ForEach(delegate (TerrainModele terrain)
        {
            aleatoire += terrain.degrePresence;
        });

        aleatoire = UnityEngine.Random.Range(0, aleatoire);
        TerrainModele terrainResult = compatibles[0];

        compatibles.ForEach(delegate (TerrainModele terrain)
        {
            if (aleatoire > -1)
            {
                aleatoire -= terrain.degrePresence;
                terrainResult = terrain;
            }
        });

        return terrainResult;
    }

    protected List<TerrainModele> getListTerrainCompatible(Dictionary<Vector3, int> voisins)
    {
        List<TerrainModele> listeTerrainsCompatibles = new List<TerrainModele>();

        listeTerrains.ForEach(delegate (TerrainModele terrain)
        {
            if (terrain.Compatible(voisins))
            {
                listeTerrainsCompatibles.Add(terrain);
            }
        });

        return listeTerrainsCompatibles;
    }

    public bool IsCompatible(Dictionary<Vector3, int> voisins, Vector3 position)
    {
        Boolean compatible = false;

        listeTerrains.ForEach(delegate (TerrainModele terrain)
        {
            if (terrain.Compatible(voisins))
            {
                compatible = true;
            }
        });

        return compatible && ConditionsSup(voisins, position);
    }

    internal virtual bool ConditionsSup(Dictionary<Vector3, int> voisins, Vector3 position)
    {
        return true;
    }
}
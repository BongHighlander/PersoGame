using System;
using System.Collections.Generic;
using UnityEngine;

public class TerrainModele
{
    public GameObject tile;
    
    public Dictionary<Vector3, int> modeleVoisins;

    public int degrePresence;
    public TypeTerrains type;

    public TerrainModele(GameObject gObject, int haut, int bas, int gauche, int droite, int proba, TypeTerrains typeTerrain)
    {
        tile = gObject;

        modeleVoisins = new Dictionary<Vector3, int>
        {
            { Direction.NORD, haut },
            { Direction.SUD, bas },
            { Direction.OUEST, gauche },
            { Direction.EST, droite }
        };

        degrePresence = proba;
        type = typeTerrain;
    }

    public Boolean Compatible(Dictionary<Vector3, int> voisins)
    {
        return CompatibiliteAcces(voisins);
    }

    private bool CompatibiliteAcces(Dictionary<Vector3, int> voisins)
    {
        Boolean condition = true;
        foreach (KeyValuePair<Vector3, int> entry in modeleVoisins)
        {
            if (voisins.ContainsKey(entry.Key))
            {
                condition = condition && (voisins[entry.Key] == 0 || voisins[entry.Key] == modeleVoisins[entry.Key]);
            }
        }
        return condition;
    }
}

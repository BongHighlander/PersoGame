using System;
using System.Collections.Generic;
using UnityEngine;

public interface IDistributeurTerrain
{
    int Taux { get; set; }
    Boolean IsCompatible(Dictionary<Vector3, int> voisins, Vector3 position);
    TerrainModele getTerrainCompatible(Dictionary<Vector3, int> voisins, Vector3 position);
}
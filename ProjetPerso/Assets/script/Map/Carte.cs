using System.Collections.Generic;
using System;
using UnityEngine;

public class Carte : MonoBehaviour
{
    public int columnsNumber;
    public int rowsNumber;
    public Dictionary<Vector3, GameObject> listRealTiles;

    public GameObject building;

    private DistributionTerrain distribution;
    private Transform boardHolder;
    public GameObject playManager;

    public void Awake()
    {
        listRealTiles = new Dictionary<Vector3, GameObject>();
    }

    internal void SetupScene()
    {
        boardHolder = new GameObject("Board").transform;

        Queue<Vector3> creationMap = new Queue<Vector3>();
        creationMap.Enqueue(new Vector3(0, 0, 0));

        distribution = GetComponent<DistributionTerrain>();
        distribution.Setup();

        while (creationMap.Count != 0)
        {

            Vector3 vector = creationMap.Dequeue();

            if (!listRealTiles.ContainsKey(vector))
            {
                Dictionary<Vector3, int> voisinsInstance = new Dictionary<Vector3, int>();

                Direction.VOISINS.ForEach(delegate (Vector3 voisin)
                {
                    TraitementVoisin(voisinsInstance, vector, voisin);
                });

                foreach (KeyValuePair<Vector3, int> entry in voisinsInstance)
                {
                    if (!listRealTiles.ContainsKey(entry.Key + vector))
                    {
                        creationMap.Enqueue(entry.Key + vector);
                    }
                }

                TerrainModele selection = distribution.DonnerTerrain(voisinsInstance, vector);

                listRealTiles.Add(vector, Instantiate(selection.tile) as GameObject);
                listRealTiles[vector].GetComponent<Case>().Configuration(playManager.GetComponent<PlayManager>(), vector, selection.type, new Dictionary<Vector3, int>(selection.modeleVoisins));
                listRealTiles[vector].transform.SetParent(boardHolder);

                if (selection.type == TypeTerrains.BATIMENT)
                {
                    GameObject instance2 =
                                    Instantiate(building, vector, Quaternion.identity) as GameObject;
                    instance2.transform.SetParent(boardHolder);
                    //instance2.GetComponent<Batiment>().setPlayManager(playManager.GetComponent<PlayManager>());
                }
            }
        }
    }

    public void TraitementVoisin(Dictionary<Vector3, int> voisinsInstance, Vector3 position, Vector3 directionVoisin)
    {
        if (IsInRange(position + directionVoisin))
        {
            try
            {
                voisinsInstance.Add(directionVoisin, listRealTiles[position + directionVoisin].GetComponent<Case>().voisins[-directionVoisin]);
            }
            catch
            {
                voisinsInstance.Add(directionVoisin, 0);
            }
        }
    }

    public Boolean IsInRange(Vector3 vector)
    {
        return (vector.x >= 0 && vector.z >= 0 && vector.x < rowsNumber && vector.z < columnsNumber);
    }

    public GameObject GetTile(Vector3 vector3)
    {
        try
        {
            return listRealTiles[vector3];
        } catch
        {
            return null;
        }
    }

    public TypeTerrains GetType(Vector3 vector3)
    {
        try
        {
            return listRealTiles[vector3].GetComponent<Case>().type;
        }
        catch
        {
            return TypeTerrains.PLAINE;
        }
    }

}
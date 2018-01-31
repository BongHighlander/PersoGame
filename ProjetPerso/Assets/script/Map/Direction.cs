using System.Collections.Generic;
using UnityEngine;

public class Direction
{
    public readonly static Vector3 NORD = new Vector3(1, 0, 0);
    public readonly static Vector3 SUD = -NORD;
    public readonly static Vector3 OUEST = new Vector3(0, 0, 1);
    public readonly static Vector3 EST = -OUEST;

    public readonly static List<Vector3> VOISINS = new List<Vector3>{
            { Direction.NORD },
            { Direction.SUD },
            { Direction.OUEST },
            { Direction.EST }
        };
}
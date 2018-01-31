using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAenemy : UniteJoueur
{
    PlayManager playManager;
    Path path = null; // pour pouvoir le tracer pour debug

    public void InitialiserIA(PlayManager playManager)
    {
        mouvements = 7;
        this.playManager = playManager;
        direction = Direction.EST;
    }


    
    private void OnMouseDown()
    {
        playManager.ClicShoot(transform.position);
    }
    
    
    // Algo A* pour trouver le chemin
    public Path FindPath(Vector3 start, Vector3 destination, Carte carte)
    {
        Path p = null;
        var closed = new HashSet<Vector3>();
        var queue = new PriorityQueue<double, Path>();
        queue.Enqueue(0, new Path(start));
        while (!queue.IsEmpty)
        {
            p = queue.Dequeue();
            if (closed.Contains(p.lastStep))
                continue;
            if (p.lastStep.Equals(destination))
            {

                return p;
            }
            closed.Add(p.lastStep);
            List<Vector3> lastStepVoisin = new List<Vector3>();

            Direction.VOISINS.ForEach(delegate (Vector3 voisin)
            {
                if (carte.IsInRange(p.lastStep + voisin) && carte.GetType(p.lastStep + voisin) != TypeTerrains.BATIMENT)
                {
                    lastStepVoisin.Add(p.lastStep + voisin);
                }
            });
            foreach (Vector3 n in lastStepVoisin)
            {
                double d = distance(p.lastStep, n);
                var newPath = p.AddStep(n, d);
                queue.Enqueue(newPath.cost + p.estimate(n), newPath);
            }
        }
        path = p;
        return p;
    }


    public double distance(Vector3 v1, Vector3 v2)
    {
        double res = Mathf.Sqrt((v2.x - v1.x) * (v2.x - v1.x) + (v2.z - v1.z) * (v2.z - v1.z));
        return res;
    }

    public void tracePath(Path p)
    {
        Vector3 source = p.lastStep;
        while (p.previousStep != null)
        {
            p = p.previousStep;
            tracer_trait(source, p.lastStep);
            source = p.lastStep;
        }
    }
    public void tracePath() // pour pouvoir le tracer pour debug dans playManager
    {
        if (path != null)
        {
            Path p = path;
            Vector3 source = p.lastStep;
            while (p.previousStep != null)
            {
                p = p.previousStep;
                tracer_trait(source, p.lastStep);
                source = p.lastStep;
            }
        }  
    }

    public void tracer_trait(Vector3 v1, Vector3 v2)
    {
        Debug.DrawLine(v1, v2, Color.red);
    }



}

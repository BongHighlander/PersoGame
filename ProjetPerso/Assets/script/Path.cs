using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
    public Vector3 lastStep;
    public Path previousStep { get; private set; }
    public double cost { get; private set; }
    public int count;
    public Path(Vector3 lastStep, Path previousStep, double cost)
    {
        this.count = previousStep.count + 1;
        this.lastStep = lastStep;
        this.previousStep = previousStep;
        this.cost = cost;
    }

    public Path(Vector3 start)
    {
        this.lastStep = start;
        this.previousStep = null;
        this.cost = 0;
        this.count = 1;
    }

    public Path AddStep(Vector3 step, double stepCost)
    {
        return new Path(step, this, cost + stepCost);
    }
    public double estimate(Vector3 v1)
    {
        double res = Mathf.Sqrt((v1.x - lastStep.x) * (v1.x - lastStep.x) + (v1.z - lastStep.z) * (v1.z - lastStep.z));
        return res;
    }

}

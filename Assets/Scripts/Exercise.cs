using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[Serializable]
public class Exercise
{
    public string name;
    public int reps;
    public int sets;

    public int lastReps;
    public int lastSets;
    public float lastWeight;
    public Exercise(string name, int reps, int sets, int lastReps, int lastSets, float lastWeight)
    {
        this.name = name;
        this.reps = reps;
        this.sets = sets;
        this.lastReps = lastReps;
        this.lastSets = lastSets;
        this.lastWeight = lastWeight;
    }
}

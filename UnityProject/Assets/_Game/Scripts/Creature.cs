using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature 
{
    private List<Vector2> genPath = new List<Vector2>();
    public List<Vector2> GenPath => genPath;


    public Creature(int geneLength = 25)
    {
        for (int i = 0; i < geneLength; i++)
        {
            genPath.Add(GetRndVec());
        }
    }

    public Creature(List<Vector2> newGenPath)
    {
        genPath.Clear();
        for (int i = 0; i < newGenPath.Count; i++)
        {
            genPath.Add(newGenPath[i]);
        }
    }


    private Vector2 GetRndVec()
    {
        return new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
    }

    public void Mutate(float mutationChance)
    {
        for (int i = 0; i < genPath.Count; i++)
        {

            if (Random.Range(0.0f, 1.0f) <= mutationChance)
            {
                genPath[i] = GetRndVec();
            }
        }
    }

    public void Crossover(Creature other, int crossoverPoint)
    {
        var newGenPath = genPath.GetRange(0, crossoverPoint); 
        newGenPath.AddRange(other.genPath.GetRange(crossoverPoint, other.genPath.Count-crossoverPoint));
        genPath = newGenPath;
    }
    
}

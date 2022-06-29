using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//DNA class 
public class DNA
{
    //Vector list for genes 
    public List<Vector2> genes = new List<Vector2>();

    //This handles the random movement DNA of the agents
    public DNA(int genomeLength = 50)
    {
        for(int i = 0; i < genomeLength; i++)
        {
            genes.Add(new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)));
        }
    }

    //The following handles the DNA mutation and addition of parent genes 
    public DNA(DNA parent,DNA partner, float mutationRate =0.01f)
    {
        for (int i = 0; i < parent.genes.Count; i++)
        {
            float mutationChance = Random.Range(0f, 1f);
            if(mutationChance <= mutationRate)
            {
                genes.Add(new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)));
            }
            else
            {
                int chance = Random.Range(0, 2);

                if(chance ==0)
                {
                    genes.Add(parent.genes[i]);
                }
                else
                {
                    genes.Add(partner.genes[i]);
                }
                
            }
        }
    }




}

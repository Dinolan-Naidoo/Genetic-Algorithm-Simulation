using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulationController : MonoBehaviour
{
    List<GeneticPathFinder> population = new List<GeneticPathFinder>();
    public GameObject creaturePrefab;
    public Transform spawnPoint;
    public Transform end;
    public int genomeLength;
    public int keepSurvivors=5;

    [Range(0f,1f)]
    public float mutationRate = 0.01f;

    public int populationSize = 100;
    public float cutoff = 0.3f;
    public float generation;
    public Text genText;
    

    private void Start()
    {
        //Initialize the population 
        initPopulation();
    }

    //Setting the correct population size 
    //Instantiating each agent
    void initPopulation()
    {
        generation = 1;
        
        for(int i =0; i< populationSize; i++)
        {
            GameObject go = Instantiate(creaturePrefab, spawnPoint.position, Quaternion.identity);
            go.GetComponent<GeneticPathFinder>().InitCreature(new DNA(genomeLength), end.position);
            population.Add(go.GetComponent<GeneticPathFinder>());

        }


    }


    //Updating the next generation with the fittest agents being kept and destroting the old population
    public void NextGeneration()
    {
        generation += 1;
        //playerCount = 0;
        int survivorCut = Mathf.RoundToInt(populationSize * cutoff);
        List<GeneticPathFinder> survivors = new List<GeneticPathFinder>();
        for(int i =0; i<survivorCut; i++)
        {
            survivors.Add(GetFittest());
        }

        for(int i =0; i< population.Count;i++)
        {
            Destroy(population[i].gameObject);
        }

        population.Clear();

        for(int i =0; i < keepSurvivors; i ++)
        {
            GameObject go = Instantiate(creaturePrefab, spawnPoint.position, Quaternion.identity);
            go.GetComponent<GeneticPathFinder>().InitCreature(survivors[i].dna, end.position);
            population.Add(go.GetComponent<GeneticPathFinder>());
        }

        while(population.Count < populationSize)
        {
            for(int i = 0; i < survivors.Count;i++)
            {
                GameObject go = Instantiate(creaturePrefab, spawnPoint.position, Quaternion.identity);
                go.GetComponent<GeneticPathFinder>().InitCreature(new DNA(survivors[i].dna, survivors[Random.Range(0, 10)].dna , mutationRate), end.position);
                population.Add(go.GetComponent<GeneticPathFinder>());
                if(population.Count >= populationSize)
                {
                    break;
                }
            }
        }

        for( int i = 0; i < survivors.Count;i++)
        {
            Destroy(survivors[i].gameObject);
        }
    }



    //Updating the next generation 
    private void Update()
    {
        if(!HasActive())
        {
            NextGeneration();
        }

        genText.text = generation.ToString();
    }

    //Getting the fittest agents by using their distance to the target
    GeneticPathFinder GetFittest()
    {
        float maxFitness = float.MinValue;
        int index = 0;

        for(int i=0; i < population.Count; i++)
        {
            if(population[i].fitness > maxFitness)
            {
                maxFitness = population[i].fitness;
                index = i;

            }
        }

        GeneticPathFinder fittest = population[index];
        population.Remove(fittest);
        return fittest;
    }



    //Checking if the agents are still alive 
    public bool HasActive()
    {
        for(int i=0; i< population.Count;i++)
        {
            if(!population[i].hasFinished)
            {
                return true;
            }
        }
        return false;
    }

}

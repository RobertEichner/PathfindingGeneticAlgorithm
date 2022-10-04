using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CreatureManager : MonoBehaviour
{
    [Header("Helper")]
    [SerializeField] private GameObject creaturePrefab = null;
    [SerializeField] private Tilemap tileMap = null;
    [SerializeField] private PosHolder posHolder = null;
    private bool hasInitialized = false;
    
    [Header("UI")]
    [SerializeField] private Text generationText = null;
    [SerializeField] private Button startButton = null;
    [SerializeField] private UIChanger uiChanger = null;

    [Header("Creature Characteristics")] 
    private int genCount = 50;
    [SerializeField] private float creatureSpeed = 1.0f;
    [SerializeField] private float creatureStepLength = 1.0f;
    [SerializeField] private float maxTargetDistance = 0.5f;
    
    //Genereration Characteristics
    private int populationCount = 100;
    private int amountTopFittest = 2;
    private float mutationChance = 0.1f;
    private int currentGeneration = 0;
    private List<CreatureAgent> creatures = new List<CreatureAgent>();


    public void InitPopulation()
    {
        creatures = new List<CreatureAgent>();
        currentGeneration = 0;
        for (int i = 0; i < populationCount; i++)
        {
            GameObject creature = Instantiate(creaturePrefab, tileMap.CellToWorld(posHolder.StartPosition), Quaternion.identity, gameObject.transform);
            CreatureAgent creatureAgent = creature.GetComponent<CreatureAgent>();
            creatureAgent.SetAgentValues(genCount, creatureSpeed, maxTargetDistance, creatureStepLength);
            creatures.Add(creatureAgent);
            creatureAgent.StartAgent();
        }
        UpdateGenerationText();
        ChangeStartButton();
        hasInitialized = true;
    }

    private void Update()
    {
        if (AllCreaturesFinished() && hasInitialized)
        {
            RankCreatures();
            CrossoverAll();
            MutateAll();
            UpdateGenerationText();
            ResetAll();
        }
    }

    private void RankCreatures()
    {
        creatures = creatures.OrderBy(o => o.GetFitness(tileMap.CellToWorld(posHolder.GoalPosition))).ToList();
        
    }

    private void CrossoverAll()
    {
        int randomCrossoverPoint = Random.Range(1, genCount);
        
        amountTopFittest = Mathf.Clamp(amountTopFittest, 2, populationCount);
        List<CreatureAgent> fittestAgents = creatures.GetRange(0, amountTopFittest);
        
        List<Creature> newGeneration = new List<Creature>();

        
        for (int i = 0; i < amountTopFittest; i++)
        {
            for (int j = i + 1; j < amountTopFittest; j++)
            {
                Creature bestOne = new Creature(fittestAgents[i].Creature.GenPath);
                bestOne.Crossover(fittestAgents[j].Creature, randomCrossoverPoint);
                newGeneration.Add(bestOne);

                if (newGeneration.Count >= populationCount)
                    break;
            }
        }
        
        
        for (int i = 0; i < creatures.Count; i++)
        {
            creatures[i].Creature = new Creature(newGeneration[i%newGeneration.Count].GenPath); 
        }
    }
    
    private void MutateAll()
    {
        foreach (var creature in creatures)
        {
            creature.Creature.Mutate(mutationChance);
        }
    }
    
    private void ResetAll()
    {
        foreach (var creature in creatures)
        {
            creature.ResetCreature(tileMap.CellToWorld(posHolder.StartPosition));
        }
    }


    private bool AllCreaturesFinished()
    {
        foreach (var creature in creatures)
        {
            if (!creature.HasFinished)
                return false;
        }
        return true;
    }

    private void UpdateGenerationText()
    {
        currentGeneration++;
        generationText.text = "Generation:\n" + currentGeneration;
    }
    
    
    //UI Events

    public void SetPopulationCount(float amount)
    {
        populationCount = (int) amount;
    }

    public void SetGenCount(float amount)
    {
        genCount = (int) amount;
    }
    
    public void SetAmountTopFittest(float amount)
    {
        amountTopFittest = (int)amount;
    }
    
    public void SetMutationChance(float amount)
    {
        mutationChance = amount;
    }

    public void SetObstacleMulti(float amount)
    {
        foreach (var creature in creatures)
        {
            creature.ObstaclePenalty = amount;
        }
    }

    public void InitStartButton()
    {
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(InitPopulation);
        startButton.GetComponentInChildren<Text>().text = "Start Simulation";
    }

    public void ChangeStartButton()
    {
        startButton.onClick.RemoveAllListeners();
        startButton.GetComponentInChildren<Text>().text = "End Simulation";
        startButton.onClick.AddListener(uiChanger.ActivateTile);
        startButton.onClick.AddListener(EndPopulation);
    }

    private void EndPopulation()
    {
        foreach (var creature in creatures)
        {
            Destroy(creature.gameObject);
        }
        generationText.text = "Generation:\n" + 0;
    }
}

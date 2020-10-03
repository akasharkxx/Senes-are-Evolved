using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopulationManagerMazeSolver : MonoBehaviour
{
    public static float elapsed;    //0

    public GameObject botPrefab;
    public GameObject startPos;
    public int populationSize;      //50
    public float trialTime;         //5

    private int generation;
    private List<GameObject> population;

    GUIStyle guiStyle = new GUIStyle();
    private void OnGUI()
    {
        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.white;
        GUI.BeginGroup(new Rect(10, 10, 250, 150));
        GUI.Box(new Rect(0, 0, 140, 140), "Stats", guiStyle);
        GUI.Label(new Rect(10, 25, 200, 30), "Generation: " + generation, guiStyle);
        GUI.Label(new Rect(10, 50, 200, 30), string.Format("Time: {0:0.00}", elapsed), guiStyle);
        GUI.Label(new Rect(10, 75, 200, 30), "Population: " + population.Count, guiStyle);
        GUI.EndGroup();
    }

    private void Start()
    {
        generation = 1;
        population = new List<GameObject>();
        for(int i = 0; i < populationSize; i++)
        {
            GameObject bot = Instantiate(botPrefab, startPos.transform.position, Quaternion.identity);
            bot.GetComponent<MazeBrain>().Init();
            population.Add(bot);
        }
    }

    private GameObject Breed(GameObject parent1, GameObject parent2)
    {
        GameObject offspring = Instantiate(botPrefab, startPos.transform.position, this.transform.rotation);
        MazeBrain brain = offspring.GetComponent<MazeBrain>();
        brain.Init();
        if (Random.Range(0, 100) == 1)
        {
            brain.dna.Mutate();
        }
        else
        {
            brain.dna.Combine(parent1.GetComponent<MazeBrain>().dna, parent2.GetComponent<MazeBrain>().dna);
        }
        return offspring;
    }

    private void BreedNewPopulation()
    {
        List<GameObject> sortedList = population.OrderBy(o => o.GetComponent<MazeBrain>().distanceTravelled).ToList();
        
        population.Clear();
        print("Sorted List: " + sortedList.Count);
        //breed upper half of list
        for (int i = (int)(sortedList.Count / 2.0f) - 1; i < sortedList.Count - 1; i++)
        {
            population.Add(Breed(sortedList[i], sortedList[i + 1]));
            population.Add(Breed(sortedList[i + 1], sortedList[i]));
        }
         
        //Killing started
        for (int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }
        generation++;
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= trialTime)
        {
            BreedNewPopulation();
            elapsed = 0;
        }
    }
}

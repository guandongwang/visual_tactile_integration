using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StimulusCreation : MonoBehaviour
{
    public int numberOfDisks;
    public int numberOfRepetitions;
    public List<List<int>> stimulusPairSequence;
    public string condition;

    // Start is called before the first frame update
    void Start()
    {
        numberOfDisks = 5;
        numberOfRepetitions = 10;
        stimulusPairSequence =  GenerateStimulusSequence(numberOfDisks, numberOfRepetitions);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<List<int>> GenerateStimulusSequence(int numberOfDisks, int numberOfRepetitions)
    {
        List<int> testingStimulusSequence = new List<int>();
        List<List<int>> stimulusPairSequence = new List<List<int>>();

        for (int i = 0; i < numberOfDisks; i++)
        {
            for (int j = 0; j < numberOfRepetitions; j++)
            {
                testingStimulusSequence.Add(i);
            }
        }
        Shuffle(testingStimulusSequence);

        foreach (int value in testingStimulusSequence)
        {

            int referenceDisk = (0 + numberOfDisks) / 2;

            List<int> pair = new List<int> {value, referenceDisk};

            stimulusPairSequence.Add(Shuffle(pair));
        }

        return stimulusPairSequence;
    }


    List<int> Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }


}

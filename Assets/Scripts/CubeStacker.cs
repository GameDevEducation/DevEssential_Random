using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnConfig
{
    public GameObject Prefab;
    public float Weighting;
}

public class CubeStacker : MonoBehaviour
{
    [SerializeField] int NumToSpawn = 50;
    [SerializeField] float VerticalSpacing = 1f;
    [SerializeField] List<SpawnConfig> SpawnConfigs;

    public enum ESpawnMethod
    {
        PureRandom,
        WeightedRandom,
        FixedPropertions
    }

    [SerializeField] ESpawnMethod Method = ESpawnMethod.PureRandom;
    [SerializeField] int RandomSeed = 0;

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(RandomSeed);
        
        if (Method == ESpawnMethod.PureRandom)
            SpawnStack_Method1();
        else if (Method == ESpawnMethod.WeightedRandom)
            SpawnStack_Method2();
        else if (Method == ESpawnMethod.FixedPropertions)
            SpawnStack_Method3();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnStack_Method1() // pure random
    {
        for (int index = 0; index < NumToSpawn; ++index)
        {
            var spawnConfig = SpawnConfigs[Random.Range(0, SpawnConfigs.Count)];

            var newCube = Instantiate(spawnConfig.Prefab, new Vector3(0f, index * VerticalSpacing, 0f), Quaternion.identity, transform);
            newCube.name = "Cube_" + (index + 1);
        }
    }

    void SpawnStack_Method2() // weighted random - can still get streaks of good or bad luck
    {
        List<float> normalisedWeightings = new List<float>();

        // calculate the probability sum
        float sum = 0f;
        foreach(var config in SpawnConfigs)
            sum += config.Weighting;

        // calculate the weighting buckets
        float weightingOffset = 0f;
        foreach(var config in SpawnConfigs)
        {
            float normalisedWeighting = config.Weighting / sum;

            normalisedWeightings.Add(weightingOffset + normalisedWeighting);
            weightingOffset += normalisedWeighting;
        }

        // spawn the blocks
        for (int index = 0; index < NumToSpawn; ++index)
        {
            float randomRoll = Random.Range(0f, 1f);

            // find the corresponding spawn config
            var spawnConfig = SpawnConfigs[0];
            for (int configIndex = 1; configIndex < SpawnConfigs.Count; ++configIndex)
            {
                // roll is within the bucket
                if (randomRoll < normalisedWeightings[configIndex] && randomRoll >= normalisedWeightings[configIndex - 1])
                    spawnConfig = SpawnConfigs[configIndex];
                else if (randomRoll < normalisedWeightings[configIndex])
                    break;
            }

            var newCube = Instantiate(spawnConfig.Prefab, new Vector3(0f, index * VerticalSpacing, 0f), Quaternion.identity, transform);
            newCube.name = "Cube_" + (index + 1);
        }        
    }

    void SpawnStack_Method3() // weighted random - fixed proportions
    {
        List<SpawnConfig> configsToSpawn = new List<SpawnConfig>();

        // calculate the probability sum
        float sum = 0f;
        foreach (var config in SpawnConfigs)
            sum += config.Weighting;

        // calculate the weighting buckets
        foreach (var config in SpawnConfigs)
        {
            int numOfThisConfig = Mathf.RoundToInt(NumToSpawn * (config.Weighting / sum));

            for (int count = 0; count < numOfThisConfig; ++count)
                configsToSpawn.Add(config);
        }

        // spawn the blocks
        for (int index = 0; index < NumToSpawn; ++index)
        {
            int configIndex = Random.Range(0, configsToSpawn.Count);
            var spawnConfig = configsToSpawn[configIndex];
            configsToSpawn.RemoveAt(configIndex);

            var newCube = Instantiate(spawnConfig.Prefab, new Vector3(0f, index * VerticalSpacing, 0f), Quaternion.identity, transform);
            newCube.name = "Cube_" + (index + 1);
        }
    }    
}

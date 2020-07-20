using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalsCounter : MonoBehaviour
{
    // Variables

    // Each list of animal types:
    public List<GameObject> HerbivoresCounter = new List<GameObject>();
    public List<GameObject> CarnivoresCounter = new List<GameObject>();
    public List<GameObject> OmnivoresCounter = new List<GameObject>();



    // Functions to add animals to their lists:

    // Add Herbivores to their List
    public void AddHerbivoreToList(GameObject HerbivoreObject)
    {
        // If the Herbivore that has just been instantiated is not already in the list then add it
        if (!HerbivoresCounter.Contains(HerbivoreObject))
            HerbivoresCounter.Add(HerbivoreObject);
    }

    // Add Carnivores to their List
    public void AddCarnivoreToList(GameObject CarnivoreObject)
    {
        // If the Carnivore that has just been instantiated is not already in the list then add it
        if (!CarnivoresCounter.Contains(CarnivoreObject))
            CarnivoresCounter.Add(CarnivoreObject);
    }

    // Add Omnivores to their List
    public void AddOmnivoreToList(GameObject OmnivoreObject)
    {
        // If the Omnivore that has just been instantiated is not already in the list then add it
        if (!OmnivoresCounter.Contains(OmnivoreObject))
            OmnivoresCounter.Add(OmnivoreObject);
    }


    void Update()
    {
        // Constantly remove all eaten animals from their Lists (if they are null, they have been eaten)
        HerbivoresCounter.RemoveAll(item => item == null);
        CarnivoresCounter.RemoveAll(item => item == null);
        OmnivoresCounter.RemoveAll(item => item == null);
    }
}

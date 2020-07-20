using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herbivore : Animal, Animals
{


    void Start()
    {
        // The agent's age begins at 0 for obvious reasons
        age = 0f;

        // The agent's hunger begins at 0
        hunger = 0f;

        // The agent's powerPoints begin at 0
        powerPoints = 0f;

        // The agent's reproductionPoints begin at 0
        reproductionPoints = 0f;
    }


    void Update()
    {
        // Increase the agent's age over time
        age += (ageRate * Time.deltaTime);

        // Animal (sadly) dies when they reach a certain age
        if (age >= ageLimit || hunger >= hungerLimit)
            Destroy(this.gameObject);

        // Increase the agent's hunger over time
        hunger += (hungerRate * Time.deltaTime);

        // Tells the animal to find it's closest Food item
        FindFood();

        // Tells the animal to move to it's closest Food item
        Move();

        // Check if the animal has met reproduction requirements
        if (reproductionPoints >= 2)
        {
            // Create a baby animal of this type
            Reproduce();
        }
    }


    // Sets up and OnTriggerEnter method for the animal
    void OnTriggerEnter(Collider other)
    {
        // Delete any Food the animal comes into contact with
        if (other.tag == "Food")
        {
            // Destroy the Food item
            Destroy(other.gameObject);

            // Call for growth of the animal
            Grow();
        }
    }

    // Override the Reproduce method to add the animal to the animal type's list
    public override void Reproduce()
    {
        // Reset reproductionPoints
        reproductionPoints = 0;

        // Spawn a child animal just to the right of this animal
        GameObject childAnimal = Instantiate(HerbivoreObject, new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z), transform.rotation) as GameObject; 

        // Call the 'AddHerbivoreToList' method from the 'AnimalsCounter' script
        GameObject.Find("Animals Counter").GetComponent<AnimalsCounter>().AddHerbivoreToList(childAnimal);
    }
}

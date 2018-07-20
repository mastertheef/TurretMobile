using Assets.Scripts.HarvestResources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Resource : MonoBehaviour {
    [SerializeField] List<ResourceDTO> resourecs;

    public void GiveResource()
    {
        if (!resourecs.Any(x=>x.ammount > 0))
        {
            Debug.Log("Asteroid is empty");
            return;
        }

        var resource = resourecs.First(x => x.ammount == resourecs.Max(y => y.ammount));
        var actualAmmount = Random.Range(1, resource.ammount);
        resource.ammount -= actualAmmount;
        Debug.Log(resource.metal.ToString() + ": " + actualAmmount);
    }
}

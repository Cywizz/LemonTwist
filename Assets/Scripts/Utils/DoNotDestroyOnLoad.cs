using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DoNotDestroyOnLoad : MonoBehaviour
{
    private static bool created = false;

    private bool _main = false;
    
    void Awake()
    {
        var allObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<DoNotDestroyOnLoad>().ToArray();
        if (allObjects.Length > 1)
        {
            //means there is one that should not be there

            foreach (var obj in allObjects)
            {
                if (obj._main == false)
                {
                    Destroy(obj.gameObject);
                }
            }
        }


        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
                        
            created = true;   
            _main = true;
        }
       
    }
}

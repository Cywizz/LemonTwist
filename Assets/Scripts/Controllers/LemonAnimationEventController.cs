using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LemonAnimationEventController : MonoBehaviour
{

    public LemonAnimationController _animationController;


    #region Unity Events
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion


    #region Animation Events

    public void Juice_End()
    {
        _animationController.AnimationEnded("Juice");
    }

    #endregion

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    private BoxCollider2D _collider;
    private Vector3 _startingPosition;
    private LemonGameController _lemonCapturer;

    #region Unity Events
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_lemonCapturer != null)
        {
            var capturerPos = _lemonCapturer.transform.position + new Vector3(0, _collider.bounds.extents.y*2, _lemonCapturer.transform.position.z);
            this.transform.position = capturerPos;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_lemonCapturer != null) return;

        var lemonController = collision.gameObject.GetComponent<LemonGameController>();

        if (lemonController != null)
        {
            lemonController.LemonPickedUpKey(this);            
            _lemonCapturer = lemonController;
        }

    }

    #endregion


    #region Public Members

    public void KeyHolderDied()
    {
        _lemonCapturer = null;
        this.transform.position = _startingPosition;
    }

    #endregion





}

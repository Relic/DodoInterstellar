using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTarget : BaseTriggerListener
{

    public override void OnStateChanged(bool state, GameObject triggeringObject)
    {
        if (triggeringObject != null && state)
        {
            bool isMain = FindObjectOfType<LevelManager>().IsMainPlayerObject(triggeringObject);
            SmoothCamera camera = FindObjectOfType<SmoothCamera>();
            Vector3 offset = camera.CameraPosition - triggeringObject.transform.position;
            offset.z = camera.CameraPosition.z;

            triggeringObject.transform.position = this.transform.position;

            if (isMain) camera.SnapCamera(triggeringObject.transform.position + offset);
        }
    }

    public override void InitializeLevelObject()
    {
        LevelObjectReference.OnTriggerValueChanged += OnStateChanged;
    }

    void Update()
    {
        transform.Rotate(0, 0, -50 * Time.deltaTime);
    }

}

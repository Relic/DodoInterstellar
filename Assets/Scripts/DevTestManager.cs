using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevTestManager : MonoBehaviour {

public void ResetLevelCounter()
    {
        PlayerPrefs.DeleteKey("CurrentLevel");
        Debug.Log("CurrentLevel key deleted");
    }
}

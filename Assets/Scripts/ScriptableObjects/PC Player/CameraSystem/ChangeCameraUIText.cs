using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCameraUIText : MonoBehaviour
{
    [SerializeField] private IntReference currentCameraIndex;

    public void ChangeText()
    {
        GetComponent<Text>().text = currentCameraIndex.Value.ToString();
    }
}

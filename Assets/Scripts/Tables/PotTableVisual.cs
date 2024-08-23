using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotTableVisual : MonoBehaviour
{
    [SerializeField]
    private PotTable potTable;
    [SerializeField]
    private GameObject potOnGameObject;
    [SerializeField]
    private GameObject particlesGameObject;

    private void Start()
    {
        potTable.OnStateChanged += PotTable_OnStateChanged;
    }

    private void PotTable_OnStateChanged(object sender,PotTable.OnStateChangedEventArgs e)
    {
        bool showVisual = e.state == PotTable.State.Boiling || e.state == PotTable.State.Boiled;
        potOnGameObject.SetActive(showVisual);
        particlesGameObject.SetActive(showVisual);
    }
}

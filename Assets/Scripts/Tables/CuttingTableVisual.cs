using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingTableVisual : MonoBehaviour
{
    private const string CUT = "Cut";

    [SerializeField]
    private CuttingTable cuttingTable;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        cuttingTable.OnCut += CuttingTable_OnCut;
    }

    private void CuttingTable_OnCut(object sender, System.EventArgs e)
    {
        animator.SetTrigger(CUT);
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
//Handling movement, unit selection and unitselection visuals (observe and singletton pattern)
    public static UnitActionSystem Instance { get; private set; } //get -> for other scripts to red, private set -> so other scripts cannot write


    public event EventHandler OnSelectedUnitChanged;


    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

//Singleton
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one UnitActionSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {//Movment func
        if (Input.GetMouseButtonDown(0))
        {
            if (TryHandleUnitSelection()) return;

            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition))
            {
                selectedUnit.GetMoveAction().Move(mouseGridPosition);
            }
        }
    }
  //Unit selection
    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
        {
            if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
            {
                SetSelectedUnit(unit);
                return true;
            }
        }

        return false;
    }
  //Visual Selection Effect - observer pattern
    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        if (OnSelectedUnitChanged != null) // if true means there are subscribers
        {
            OnSelectedUnitChanged(this, EventArgs.Empty);
        }
        //OnSelectedUnitChange?Invoke(This, EventArgs.Empty); the same as the if
    }

     //making it accesable for other scripts to know what unit is selected
    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
}

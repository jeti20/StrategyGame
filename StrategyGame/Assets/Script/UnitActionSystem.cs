using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
//Handling movement, unit selection and unitselection visuals (observe and singletton pattern)
public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance {get; private set;} //get -> for other scripts to red, private set -> so other scripts cannot write
        
    public event EventHandler OnSelectedUnitChanged;
    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;
    
    //Singleton
    private void Awake()
    {
        if (Instance != null) 
        {
            Debug.LogError("There is more than one UnitActionSystem " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    private void Update() 
    {      
        //Movment func
        if (Input.GetMouseButtonDown(0))
        {
            if (TryHandleUnitSelection()) return;
            selectedUnit.Move(MouseWorld.GetPosition()); 
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

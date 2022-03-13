using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }

    private bool _previewing;
    private GameUnit _previewingBuilding;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_previewing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                BuildBuilding(_previewingBuilding.Data, GetMousePosition());
                EndPreview();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                EndPreview();
            }
            else
            {
                _previewingBuilding.SetWorldPosition(GetMousePosition());
            }
        }
    }

    public void StartPreviewBuilding(GameUnitData data)
    {
        _previewing = true;
        _previewingBuilding = new GameUnit(data);
        _previewingBuilding.InstantiatePrefab(GetMousePosition());
        Material previewMaterial =  Resources.Load($"Materials/BuildingPreview") as Material;
        _previewingBuilding.SetMaterial(previewMaterial);
    }

    public void BuildBuilding(GameUnitData data, Vector3 position)
    {
        GameUnit building = new GameUnit(data);
        BuildBuilding(building, position);
    }

    public void BuildBuilding(GameUnit building, Vector3 position)
    {
        bool canBuild = true;

        // Check resource constraints
        foreach (CostValue cost in building.Data.Costs)
        {
            if (!Globals.RESOURCE_DATA[cost.Code].CanConsumeResource(cost.Value))
            {
                canBuild = false;
                break;
            }
        }

        if (canBuild)
        {
            building.InstantiatePrefab(position);
            foreach (CostValue cost in building.Data.Costs)
                Globals.RESOURCE_DATA[cost.Code].ConsumeResource(cost.Value);
        }
        else
        {
            EndPreview();
        }
    }

    void EndPreview()
    {
        if (_previewingBuilding != null) _previewingBuilding.Destroy();
        _previewing = false;
        _previewingBuilding = null;
    }

    Vector3 GetMousePosition()
    {
        Vector3 totalMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
        Vector3 modifiedPosition = new Vector3(totalMousePosition.x, 0, totalMousePosition.z);
        return modifiedPosition;
    }

    //bool CanBuildAtPosition(Vector3 position)
    //{

    //}

    
}
using System.Collections.Generic;
using UnityEngine;

public class FootstepsManager : MonoBehaviour
{
    private Dictionary<string, string> _floorMaterialToFMODParameter = new Dictionary<string, string>
    {
        {"Default", "Default"},
        {"Concrete", "Concrete"},
        {"Gravel", "Gravel"}
    };

    private string _currentFloorMaterial = "Default";
    
    void SetFloorMaterial(string materialName)
    {
        if (_floorMaterialToFMODParameter.ContainsKey(materialName))
        {
            _currentFloorMaterial = _floorMaterialToFMODParameter[materialName];
        }
    }

    public string GetCurrentFloorMaterial() => _currentFloorMaterial;

    public void Update()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 2f))
        {
            if (hit.collider.CompareTag("Concrete"))
            {
                SetFloorMaterial("Concrete");
            }
            else if (hit.collider.CompareTag("Gravel"))
            {
                SetFloorMaterial("Gravel");
            }
            else
            {
                SetFloorMaterial("Default");
            }
        }
    }
}

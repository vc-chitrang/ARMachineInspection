using System;
using System.Collections.Generic;
using UnityEngine;

public class Machine:MonoBehaviour {
    [SerializeField] private MachineData machineData;
    [SerializeField] private Material glassMaterial;

    [ContextMenu("ApplyGlassMaterial")]
    public void ApplyGlassMaterial() {
        machineData.ApplyGlassMaterial(glassMaterial);
    }

    private void Start() {
        UIManager.Instance.DisplayMachineInformation(machineData);
    }
}//Machine Class end.

[Serializable]
public class MachineData {
    public List<ComponentData> components;

    public void ApplyGlassMaterial(Material glassMaterial) {
        components.ForEach(c => c.ApplyGlassMaterial(glassMaterial));        
    }
}

[Serializable]
public class ComponentData {
    public string componentName;
    public string instructions;
    public List<GameObject> componentList;

    public void ApplyGlassMaterial(Material glassMaterial) {
        componentList.ForEach(c => {
            if (c.TryGetComponent(out MeshRenderer meshRenderer)) {
                meshRenderer.material = glassMaterial;
            }
        });
    }
}
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

    [ContextMenu("ApplyDefaultMaterial")]
    public void ApplyDefaultMaterial() {
        machineData.ApplyDefaultMaterial();
    }

    public void HighlightComponent(int index) {
        machineData.HighlightComponent(index);
    }

    public void Initialize() {
        machineData.Init();
    }

    private void Start() {
        Initialize();
        UIManager.Instance.SetMachine(this);
        UIManager.Instance.DisplayMachineInformation(machineData);
    }
}//Machine Class end.

[Serializable]
public class MachineData {
    public List<ComponentData> components;

    public void Init() {
        components.ForEach(c => c.Init());
        for (int i = 0;i < components.Count;i++) {
            ComponentData c = components[i];
            c.componentId = i;
        }
    }

    public void ApplyGlassMaterial(Material mat) {
        components.ForEach(c => c.ApplyGlassMaterial(mat));
    }
    public void ApplyDefaultMaterial() {
        components.ForEach(c => c.ApplyDefaultMaterial());
    }

    public void HighlightComponent(int index) {
        components.ForEach(c => c.HighlightComponent(index));
    }
}

[Serializable]
public class ComponentData {
    public string componentName;
    public string instructions;
    public List<Component> componentList;
    public int componentId;

    public void Init() {
        componentList.ForEach(c => c.Init());
    }
    public void ApplyGlassMaterial(Material mat) {
        componentList.ForEach(c => c.ApplyGlassMaterial(mat));
    }
    public void ApplyDefaultMaterial() {
        componentList.ForEach(c => c.ApplyDefaultMaterial());
    }

    internal void HighlightComponent(int index) {
        componentList.ForEach(c => {
            if (componentId == index) {
                c.ApplyDefaultMaterial();
            }
        });
    }
}

[Serializable]
public class Component {
    public GameObject componentObject;
    private MeshRenderer _meshRenderer;
    private List<Material> _materials;

    public void Init() {
        if (componentObject.TryGetComponent(out MeshRenderer meshRenderer)) {
            _meshRenderer = meshRenderer;

            _materials = new List<Material>();
            foreach (var material in _meshRenderer.materials) {
                _materials.Add(material);
            }
        }
    }
    public void ApplyGlassMaterial(Material mat) {
        if (_meshRenderer != null) {
            var materials = new Material[_meshRenderer.materials.Length];
            for (int i = 0;i < materials.Length;i++) {
                materials[i] = mat;
            }
            _meshRenderer.materials = materials;
        }
    }

    public void ApplyDefaultMaterial() {
        if (_meshRenderer != null) {
            var materials = new Material[_materials.Count];
            for (int i = 0;i < materials.Length;i++) {
                materials[i] = _materials[i];
            }
            _meshRenderer.materials = materials;
        }
    }
}
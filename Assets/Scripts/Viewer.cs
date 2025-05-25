using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewer : MonoBehaviour
{
    private MeshFilter meshFilter;

    private MeshRenderer meshRenderer;

    [SerializeField]
    private Material defaultMaterial;

    private Mesh[] _frames;

    public Mesh[] Frames {
        set { _frames = value; }
        get { return _frames; } 
    }

    public event Action<int> OnFrameChanged;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();

        meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer != null && defaultMaterial != null)
        {
            meshRenderer.material = defaultMaterial;
        }
    }

    private void Start()
    {
        OnFrameChanged.Invoke(0);

    }

    private void OnEnable()
    {
        OnFrameChanged += UpdateMesh;
    }

    private void OnDisable()
    {
        OnFrameChanged -= UpdateMesh;
    }

    private void UpdateMesh(int frameIndex) 
    {
        meshFilter.mesh = _frames[frameIndex];
    }

    public void SetFrame(int index)
    {
        index = Mathf.Clamp(index, 0, _frames.Length - 1);
        if (index != CurrentFrame)
        {
            CurrentFrame = index;
            OnFrameChanged?.Invoke(index);
        }
    }

    public int CurrentFrame { get; private set; }
}

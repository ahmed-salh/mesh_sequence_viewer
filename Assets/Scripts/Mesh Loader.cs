using System;
using UnityEngine;

public class MeshLoader : MonoBehaviour
{
    public Mesh[] frames;

    private MeshFilter meshFilter;

    private MeshRenderer meshRenderer;

    public Material defaultMaterial;

    public bool autoAjustScale = true;

    public int CurrentFrame { get; private set; }

    public event Action<int> OnFrameChanged;

    public event Action<int> OnFrameLoaded;

    public void LoadFrames() 
    {
        meshFilter = GetComponent<MeshFilter>();

        meshRenderer = GetComponent<MeshRenderer>();

        frames = Resources.LoadAll<Mesh>("Mesh_1");

        Debug.Log("Mesh is loaded!");

        OnFrameLoaded.Invoke(frames.Length - 1);

    }

    private void Start()
    {
        if (meshRenderer != null && defaultMaterial != null)
        {
            meshRenderer.material = defaultMaterial;
        }

        OnFrameChanged.Invoke(0);

        SetOpaque();
    }

    void OnEnable()
    {
        OnFrameChanged += UpdateMesh;

    }

    private void Awake()
    {
        LoadFrames();

    }

    void OnDisable()
    {
        OnFrameChanged -= UpdateMesh;
    }

    void UpdateMesh(int frameIndex)
    {
        meshFilter.mesh = frames[frameIndex];

        if(autoAjustScale)
            Scale(frames[frameIndex], transform);


    }

    void Scale(Mesh mesh, Transform target)
    {
        var size = mesh.bounds.size;
        
        float maxDim = Mathf.Max(size.x, size.y, size.z);

        float scale = 1f / maxDim; // normalize largest dimension to 1
        
        target.localScale = Vector3.one * scale;
    }

    public void SetFrame(int index)
    {
        index = Mathf.Clamp(index, 0, frames.Length - 1);
        if (index != CurrentFrame)
        {
            CurrentFrame = index;
            OnFrameChanged?.Invoke(index);
        }
    }

    public void SetTransparent()
    {
        if (defaultMaterial != null)
        {
            defaultMaterial.SetFloat("_Mode", 3); // Transparent mode
            defaultMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            defaultMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            defaultMaterial.SetInt("_ZWrite", 0);
            defaultMaterial.DisableKeyword("_ALPHATEST_ON");
            defaultMaterial.DisableKeyword("_ALPHABLEND_ON");
            defaultMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            defaultMaterial.renderQueue = 3000;
            var color = defaultMaterial.color;
            color.a = 0.5f; // 50% transparent
            defaultMaterial.color = color;
        }
    }

    public void SetOpaque()
    {
        if (defaultMaterial != null)
        {
            defaultMaterial.SetFloat("_Mode", 0); // Opaque mode
            defaultMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            defaultMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            defaultMaterial.SetInt("_ZWrite", 1);
            defaultMaterial.DisableKeyword("_ALPHATEST_ON");
            defaultMaterial.DisableKeyword("_ALPHABLEND_ON");
            defaultMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            defaultMaterial.renderQueue = -1;
            var color = defaultMaterial.color;
            color.a = 1f;
            defaultMaterial.color = color;
        }
    }
}

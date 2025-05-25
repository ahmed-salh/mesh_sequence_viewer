using System;
using UnityEngine;

[RequireComponent(typeof(Viewer))]
public class Loader : MonoBehaviour
{
    private Viewer viewer;

    public event Action<int> OnFrameLoaded;


    private void Awake()
    {
        viewer = GetComponent<Viewer>();

        LoadFrames();
    }

    public void LoadFrames() 
    {
        viewer.Frames = Resources.LoadAll<Mesh>("Mesh_1");

        int maxFrameIndex = viewer.Frames.Length - 1;

        OnFrameLoaded.Invoke(maxFrameIndex);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public struct NavMeshChunk
{
    public Vector3 EulerRotation;

    //---- DRAG HERE YOUR BAKED NAVMESH CHUNK
    public NavMeshData Data;
    public bool Enabled;
}


public class NavMeshSphere : MonoBehaviour
{
    //----- HERE ARE YOUR BAKED NAVMESH CHUNKS
    [SerializeField]
    private List<NavMeshChunk> _navMeshChunks;

    [SerializeField]
    private List<NavMeshDataInstance> _instances = new List<NavMeshDataInstance>();

    [SerializeField]
    private Transform _pivot;

    public void OnEnable()
    {
        RemoveAllNavMeshLoadedData();

        LoadNavmeshData();
    }

    public void RemoveAllNavMeshLoadedData()
    {
        NavMesh.RemoveAllNavMeshData();
    }

    public void LoadNavmeshData()
    {
        foreach (var chunk in _navMeshChunks)
        {
            if (chunk.Enabled)
            {
                _instances.Add(
                    NavMesh.AddNavMeshData(
                        chunk.Data,
                        _pivot.transform.position,
                        Quaternion.Euler(chunk.EulerRotation)));
            }
        }
    }

    public void OnDisable()
    {
        foreach (var instance in _instances)
        {
            instance.Remove();
        }
    }
}
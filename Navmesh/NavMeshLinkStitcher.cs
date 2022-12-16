using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

[Serializable]
public class MeshPair
{
    public MeshRenderer mesh1;
    public MeshRenderer mesh2;
}


[Serializable]
public class ModelEdge
{
    public Vector3 point1;
    public Vector3 point2;
}



[Serializable]
public class Triangle
{
    public Vector3[] points = new Vector3[3];

    public Triangle(Vector3 point1, Vector3 point2, Vector3 point3)
    {
        points = new Vector3[3];
        points[0] = point1;
        points[1] = point2;
        points[2] = point3;

    }
}


[ExecuteInEditMode]
public class NavMeshLinkStitcher : MonoBehaviour
{

    [SerializeField]
    [Tooltip("How close should the two vertices be to be considered shared?")]
    private float allowedPointDistance = 0.05f;
    [SerializeField]
    [Tooltip("How far into the mesh should the endpoint of the link be?")]
    private float linkEndpointOffset = 0.1f;
    [SerializeField]
    private NavMeshLink linkPrefab;
    [SerializeField]
    private MeshPair[] meshes;

    [SerializeField]
    [HideInInspector]
    private List<GameObject> results = new List<GameObject>();


    [ContextMenu("Generate Links")]
    public void GenerateLinks()
    {
        // Clear existing results
        for (int i = 0; i < results.Count; ++i)
        {
            DestroyImmediate(results[i]);
        }
        results.Clear();

        for (int i = 0; i < meshes.Length; ++i)
        {
            Debug.Log("Array index = " + i);
            GenerateLink(meshes[i].mesh1, meshes[i].mesh2);
        }
    }

    void GenerateLink(MeshRenderer meshRendererA, MeshRenderer meshRendererB)
    {
        Debug.Log("GenerateLink");

        Mesh meshA = meshRendererA.GetComponent<MeshFilter>().sharedMesh;
        Mesh meshB = meshRendererB.GetComponent<MeshFilter>().sharedMesh;

        // Transform the meshes to match the in-scene objects
        Vector3[] verticesA = meshA.vertices;
        Vector3[] verticesB = meshB.vertices;
        for (int i = 0; i < verticesA.Length; ++i)
        {
            verticesA[i] = meshRendererA.transform.TransformPoint(verticesA[i]);
        }
        for (int i = 0; i < verticesB.Length; ++i)
        {
            verticesB[i] = meshRendererB.transform.TransformPoint(verticesB[i]);
        }


        // Determine all shared edges of two meshes (use model? or navmesh?)
        // Shared edge is when the two vertices of the edge are within a set range
        // when a shared edge is detected. determine center point of both versions of the edge
        // generate a link between the two centre points
        // determine the width of the link based on the distance between the two edge endpoints

        // Loop through the triangle array, looking at each set of three indices for the three vertices in a triangle
        for (int i = 0; i < meshA.triangles.Length; i += 3)
        {
            // Mesh A triangle indices
            int vertexIndexA1 = meshA.triangles[i];
            int vertexIndexA2 = meshA.triangles[i+1];
            int vertexIndexA3 = meshA.triangles[i+2];
            Triangle triA = new Triangle(verticesA[vertexIndexA1], verticesA[vertexIndexA2], verticesA[vertexIndexA3]);

            // Loop through all triangles in Mesh B

            for (int j = 0; j < meshB.triangles.Length; j += 3)
            {
                // Mesh B triangle indices
                int vertexIndexB1 = meshB.triangles[j];
                int vertexIndexB2 = meshB.triangles[j + 1];
                int vertexIndexB3 = meshB.triangles[j + 2];
                Triangle triB = new Triangle(verticesB[vertexIndexB1], verticesB[vertexIndexB2], verticesB[vertexIndexB3]);

                // Check if these triangles share an edge
                ModelEdge sharedEdge = FindSharedEdge(triA, triB);

                if (sharedEdge!= null)
                {
                    // They share an edge, so build a link between them
                    GameObject navLinkObject = Instantiate(linkPrefab.gameObject,transform);
                    NavMeshLink navLink = navLinkObject.GetComponent<NavMeshLink>();

                    // Determine width of link = width of edge
                    navLink.width = (sharedEdge.point1 - sharedEdge.point2).magnitude;

                    // Set start and endpoints based on desired offset
                    navLink.startPoint = Vector3.forward * -linkEndpointOffset;
                    navLink.endPoint = Vector3.forward * linkEndpointOffset;

                    // Determine position of link = midpoint point on edge
                    Vector3 midPoint = FindMidpoint(sharedEdge.point1, sharedEdge.point2);
                    navLink.transform.position = midPoint;

                    // Determine end points for the nav link, relative to mid point
                    Vector3 endpointA = FindEdpointForTri(sharedEdge, triA) - midPoint;
                    Vector3 endpointB = FindEdpointForTri(sharedEdge, triB) - midPoint;

                    // Determine vector of direction the link should face
                    Vector3 linkDirection = (endpointA - endpointB).normalized;

                    // Orient endpoint based on link direction
                    Vector3 normalA = FindTriNormal(triA);
                    Vector3 normalB = FindTriNormal(triB);
                    Vector3 averageNormal = (normalA + normalB) / 2.0f;
                 
                    navLink.transform.localRotation = Quaternion.LookRotation(linkDirection, Vector3.up);
                    navLink.transform.rotation = Quaternion.FromToRotation(navLink.transform.up, averageNormal) * navLink.transform.rotation;

                    // Add link object to navigation
                    results.Add(navLinkObject);
                }
            }
        }
    }



    public ModelEdge FindSharedEdge(Triangle triA, Triangle triB)
    {
        ModelEdge edge = new ModelEdge();

        int sharedPoints = 0;

        for (int i = 0; i < triA.points.Length; ++i)
        {
            for (int j = 0; j < triB.points.Length; ++j)
            {
                float squareDistance = (triA.points[i] - triB.points[j]).sqrMagnitude;
                if (squareDistance <= allowedPointDistance*allowedPointDistance)
                {
                    ++sharedPoints;
                    Vector3 midPoint = FindMidpoint(triA.points[i], triB.points[j]);
                    if (sharedPoints == 1)
                        edge.point1 = midPoint;
                    else if (sharedPoints == 2)
                        edge.point2 = midPoint;
                    else if (sharedPoints > 2)
                        Debug.LogError("Found complete duplicate triangles between two meshes, were meshes incorrectly matched?");
                }
            }
        }

        if (sharedPoints < 2)
            return null;
        else
            return edge;

    }


    public Vector3 FindMidpoint(Vector3 point1, Vector3 point2)
    {
        return point1 + (point2 - point1) * 0.5f;
    }

    public Vector3 FindTriNormal(Triangle tri)
    {
        // Find a vector perpendicular to the edge along the plane of the tri
        return Vector3.Cross(tri.points[0] - tri.points[1], tri.points[0] - tri.points[2]);
    }

    public Vector3 FindEdpointForTri(ModelEdge edge, Triangle tri)
    {
        // Do this by using the center point of the shared edge.
        Vector3 midPoint = FindMidpoint(edge.point1, edge.point2);

        // Find a vector perpendicular to the edge along the plane of the tri
        Vector3 planeNormal = FindTriNormal(tri);

        // Find a vector perpendicular to this vector and to the edge
        Vector3 vectorAlongPlane = Vector3.Cross(edge.point1 - edge.point2, planeNormal);

        // Normalize
        Vector3 offsetDirection = vectorAlongPlane.normalized;

        // Calculate two potential endpoints by moving in the offset direction or against it (we're not sure which way our perpendicular vector was pointing)
        Vector3 endpoint1 = midPoint + offsetDirection * linkEndpointOffset;
        Vector3 endpoint2 = midPoint - offsetDirection * linkEndpointOffset;

        // Whichever of these two endpoints is closest to the third tri point, that is the correct one to use
        Vector3 thirdPoint = DetermineThirdPoint(edge, tri);
        float endpoint1Distance = (endpoint1 - thirdPoint).sqrMagnitude;
        float endpoint2Distance = (endpoint2 - thirdPoint).sqrMagnitude;
        if (endpoint1Distance < endpoint2Distance)
            return endpoint1;
        else
            return endpoint2;
    }

    // Calculate the point of the tri that is NOT on the edge
    public Vector3 DetermineThirdPoint(ModelEdge edge, Triangle tri)
    {
        for (int i = 0; i < tri.points.Length; ++i)
        {
            float squareDistance1 = (tri.points[i] - edge.point1).sqrMagnitude;
            float squareDistance2 = (tri.points[i] - edge.point2).sqrMagnitude;
            if (squareDistance1 > allowedPointDistance * allowedPointDistance
                && squareDistance2 > allowedPointDistance * allowedPointDistance)
            {
                // This point of the tri does not match either edge point
                // Therefore it is the third point
                return tri.points[i];
            }
        }
        Debug.LogError("All tri points appear to match the edge - is this tri malformed?");
        return Vector3.zero;
    }
}

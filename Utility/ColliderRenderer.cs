using UnityEngine;
using System.Collections.Generic;

namespace Bounder.Framework
{
    [ExecuteAlways]
    public class ColliderRenderer : MonoBehaviour
    {
        [SerializeField]
        bool showCollision = true;
        [SerializeField]
        bool showTrigger = true;

        [SerializeField]
        Color normalColor = new(1f, 1f, 1f, 0.5f);
        [SerializeField]
        Color collisionColor = new(1f, 0f, 0f, 0.5f);
        [SerializeField]
        Color triggerColor = new(0f, 0f, 1f, 0.5f);
        [SerializeField]
        Color bothColor = new(1f, 0f, 1f,0.5f);

        [SerializeField]
        bool limitByLayer = false;
        [SerializeField]
        LayerMask limitedLayers;
        [SerializeField]
        bool limitByTag = false;
        [SerializeField]
        List<string> limitedTags = new();
        [SerializeField]
        bool limitByComponent = false;
        [SerializeField]
        List<string> limitedComponents = new();
        [SerializeField]
        bool limitByName = false;
        [SerializeField]
        List<string> limitedNames = new();


        int numCollisions = 0;
        int numTriggers = 0;
        Mesh[] meshes = null;

        private void OnEnable()
        {
            // Make meshes
            Collider[] colliders = GetComponents<Collider>();
            meshes = new Mesh[colliders.Length];
            for (int i = 0; i < colliders.Length; i++)
            {
                System.Type colliderType = colliders[i].GetType();
                if (colliderType == typeof(MeshCollider))
                {
                    MeshCollider meshCollider = (MeshCollider)colliders[i];
                    meshes[i] = meshCollider.sharedMesh;
                }
                else if (colliderType == typeof(SphereCollider))
                {
                    meshes[i] = Resources.GetBuiltinResource<Mesh>("Sphere.fbx");
                }
                else if (colliderType == typeof(BoxCollider))
                {
                    meshes[i] = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
                }
                else if (colliderType == typeof(CapsuleCollider))
                {
                    meshes[i] = Resources.GetBuiltinResource<Mesh>("Capsule.fbx");
                }
                else
                {
                    Debug.LogWarning("ColliderRenderer does not support collider type " + colliderType.Name);
                }
            }
        }

        private void FixedUpdate()
        {
            numCollisions = 0;
            numTriggers = 0;
        }

        private void OnDrawGizmos()
        {
            if (showCollision && showTrigger && numCollisions > 0 && numTriggers > 0)
            {
                Gizmos.color = bothColor;
            }
            else if (showCollision && numCollisions > 0)
            {
                Gizmos.color = collisionColor;
            }
            else if (showTrigger && numTriggers > 0)
            {
                Gizmos.color = triggerColor;
            }
            else
            {
                Gizmos.color = normalColor;
            }

            // loop through all colliders
            Collider[] colliders = GetComponents<Collider>();
            for (int i = 0; i < colliders.Length; i++)
            {
                // Determine type of collider
                System.Type colliderType = colliders[i].GetType();
                if (colliderType == typeof(MeshCollider))
                {
                    // Often same as model, so scale up a tiny bit
                    float scaleFactor = 1.05f;
                    Gizmos.DrawMesh(meshes[i], transform.position, transform.rotation, transform.lossyScale* scaleFactor);
                }
                else if (colliderType == typeof(SphereCollider))
                {
                    SphereCollider collider = colliders[i] as SphereCollider;

                    // Position
                    Vector3 offset = collider.center;
                    offset.x *= transform.lossyScale.x;
                    offset.y *= transform.lossyScale.y;
                    offset.z *= transform.lossyScale.z;
                    offset = transform.rotation * offset;
                    Vector3 pos = transform.position + offset;

                    // Rotation (none - sphere)

                    // Scale
                    float meshScaleFactor = 0.5f; // Sphere mesh is too big.
                    float colliderScaleFactor = collider.radius / 0.5f; // 0.5f being default size of sphere collider
                    float maxLossyScale = 0f;
                    Vector3 objectScale = transform.lossyScale;
                    if (objectScale.x > maxLossyScale)
                        maxLossyScale = objectScale.x;
                    if (objectScale.y > maxLossyScale)
                        maxLossyScale = objectScale.y;
                    if (objectScale.z > maxLossyScale)
                        maxLossyScale = objectScale.z;
                    float combinedScale = maxLossyScale * meshScaleFactor * colliderScaleFactor;
                    Vector3 scale = new(combinedScale, combinedScale, combinedScale);

                    // Render
                    Gizmos.DrawMesh(meshes[i], pos, transform.rotation, scale);
                }
                else if (colliderType == typeof(BoxCollider))
                {
                    BoxCollider collider = colliders[i] as BoxCollider;

                    // Position
                    Vector3 offset = collider.center;
                    offset.x *= transform.lossyScale.x;
                    offset.y *= transform.lossyScale.y;
                    offset.z *= transform.lossyScale.z;
                    offset = transform.rotation * offset;
                    Vector3 pos = transform.position + offset;

                    // Rotation
                    // No special rotation, use transform

                    // Scale
                    Vector3 scale = transform.lossyScale;
                    scale.x *= collider.size.x;
                    scale.y *= collider.size.y;
                    scale.z *= collider.size.z;

                    // Render
                    Gizmos.DrawMesh(meshes[i], pos, transform.rotation, scale);
                }
                else if (colliderType == typeof(CapsuleCollider))
                {
                    CapsuleCollider collider = colliders[i] as CapsuleCollider;

                    // Position
                    Vector3 offset = collider.center;
                    offset.x *= transform.lossyScale.x;
                    offset.y *= transform.lossyScale.y;
                    offset.z *= transform.lossyScale.z;
                    offset = transform.rotation * offset;
                    Vector3 pos = transform.position + offset;

                    // Rotation
                    Quaternion rotation = transform.rotation;
                    if (collider.direction == 0) // x axis
                        rotation *= Quaternion.AngleAxis(90f, Vector3.forward);
                    if (collider.direction == 2) // z axis
                        rotation *= Quaternion.AngleAxis(90f, Vector3.right);
                    // y axis default, no special rotation

                    // Scale
                    Vector3 objectScale = transform.lossyScale;
                    Vector3 scale = Vector3.one;
                    float meshScaleFactorXZ = 0.5f; // mesh is too big.
                    float meshScaleFactorY = 0.25f; // mesh is too big.

                    if (collider.direction == 1) // y axis (default)
                    {
                        float maxLossyScaleXZ = 0f;
                        if (objectScale.x > maxLossyScaleXZ)
                            maxLossyScaleXZ = objectScale.x;
                        scale.x = maxLossyScaleXZ * meshScaleFactorXZ * collider.radius / 0.5f;
                        scale.z = maxLossyScaleXZ * meshScaleFactorXZ * collider.radius / 0.5f;
                        scale.y = objectScale.y * meshScaleFactorY * collider.height / 1.0f;
                        if (scale.y < scale.x * 0.5f)
                            scale.y = scale.x * 0.5f;
                    }
                    else if (collider.direction == 0) // x axis
                    {
                        float maxLossyScaleYZ = 0f;
                        if (objectScale.y > maxLossyScaleYZ)
                            maxLossyScaleYZ = objectScale.y;
                        if (objectScale.z > maxLossyScaleYZ)
                            maxLossyScaleYZ = objectScale.z;
                        scale.x = maxLossyScaleYZ * meshScaleFactorXZ * collider.radius / 0.5f;
                        scale.z = maxLossyScaleYZ * meshScaleFactorXZ * collider.radius / 0.5f;
                        scale.y = objectScale.x * meshScaleFactorY * collider.height / 1.0f;
                        if (scale.y < scale.x * 0.5f)
                            scale.y = scale.x * 0.5f;
                    }
                    else if (collider.direction == 2) // z axis
                    {
                        float maxLossyScaleXY = 0f;
                        if (objectScale.x > maxLossyScaleXY)
                            maxLossyScaleXY = objectScale.x;
                        if (objectScale.y > maxLossyScaleXY)
                            maxLossyScaleXY = objectScale.y;
                        scale.x = maxLossyScaleXY * meshScaleFactorXZ * collider.radius / 0.5f;
                        scale.z = maxLossyScaleXY * meshScaleFactorXZ * collider.radius / 0.5f;
                        scale.y = objectScale.z * meshScaleFactorY * collider.height / 1.0f;
                        if (scale.y < scale.x * 0.5f)
                            scale.y = scale.x * 0.5f;
                    }

                    // Render
                    Gizmos.DrawMesh(meshes[i], pos, rotation, scale);
                }
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            bool count = true;
            GameObject hitObject = collision.gameObject;

            if (limitByLayer)
            {
                count = count && hitObject.IsInLayermask(limitedLayers);
            }
            if (limitByTag)
            {
                bool hasTag = false;
                foreach (var tag in limitedTags)
                {
                    hasTag = hasTag || hitObject.CompareTag(tag);
                }
                count = count && hasTag;
            }
            if (limitByName)
            {
                bool hasName = false;
                foreach (var name in limitedNames)
                {
                    hasName = hasName || hitObject.name.Contains(name);
                }
                count = count && hasName;
            }
            if (limitByComponent)
            {
                bool hasComponent = false;
                foreach (var componentName in limitedComponents)
                {
                    System.Type componentType = System.Type.GetType(componentName);
                    hasComponent = hasComponent || hitObject.GetComponent(componentType);
                }
                count = count && hasComponent;
            }

            if (count)
                ++numCollisions;
        }

        private void OnTriggerStay(Collider other)
        {
            bool count = true;
            GameObject hitObject = other.gameObject;

            if (limitByLayer)
            {
                count = count && hitObject.IsInLayermask(limitedLayers);
            }
            if (limitByTag)
            {
                bool hasTag = false;
                foreach (var tag in limitedTags)
                {
                    hasTag = hasTag || hitObject.CompareTag(tag);
                }
                count = count && hasTag;
            }
            if (limitByName)
            {
                bool hasName = false;
                foreach (var name in limitedNames)
                {
                    hasName = hasName || hitObject.name.Contains(name);
                }
                count = count && hasName;
            }
            if (limitByComponent)
            {
                bool hasComponent = false;
                foreach (var componentName in limitedComponents)
                {
                    System.Type componentType = System.Type.GetType(componentName);
                    hasComponent = hasComponent || hitObject.GetComponent(componentType);
                }
                count = count && hasComponent;
            }

            if (count)
                ++numTriggers;
        }


    }

}

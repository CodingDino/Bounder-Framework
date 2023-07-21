// ************************************************************************ 
// File Name:   RendererExtension.cs 
// Purpose:    	Extends the Renderer class
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2023 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
#region Imports
// ************************************************************************
using UnityEngine;
#endregion
// ************************************************************************


// ************************************************************************ 
#region Class: RendererExtension
// ************************************************************************
public static class RendererExtension
{
    // ********************************************************************
    #region Extension Methods 
    // ********************************************************************
    public static void SetAlpha(this Renderer renderer, float val)
    {
        // NOTE: Will not work for sprites, if needed, write separate function for sprites
        // Avoid checking if this renderer is a SpriteRenderer to avoid unneeded dynamic cast to slow down execution
        // Set this object's alpha
        foreach (Material mat in renderer.materials)
        {
            if (mat.HasProperty("_Color"))
            {
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, val);
            }
            else if (mat.HasProperty("_TintColor"))
            {
                Color col = mat.GetColor("_TintColor");
                mat.SetColor("_TintColor", new Color(col.r, col.g, col.b, val));
            }
        }
    }
    // ********************************************************************
    public static void SetAlphaRecursive(this Renderer renderer, float val)
    {
        // NOTE: Will not work for sprites, if needed, write separate function for sprites
        // Avoid checking if this renderer is a SpriteRenderer to avoid unneeded dynamic cast to slow down execution

        // Set this object's alpha
        renderer.SetAlpha(val);

        // Set child object alpha
        if (renderer.transform.childCount > 0)
        {
            foreach (Transform child in renderer.transform)
            {
                Renderer childRenderer = child.GetComponent<Renderer>();
                childRenderer?.SetAlphaRecursive(val);
            }
        }
    }
    // ********************************************************************
    public static void SetColour(this Renderer renderer, Color _color)
    {
        // NOTE: Will not work for sprites, if needed, write separate function for sprites
        // Avoid checking if this renderer is a SpriteRenderer to avoid unneeded dynamic cast to slow down execution
        foreach (Material mat in renderer.materials)
        {
            mat.color = _color;
        }
    }
    // ********************************************************************
    public static void SetColourRecursive(this Renderer renderer, Color _color)
    {
        // NOTE: Will not work for sprites, if needed, write separate function for sprites
        // Avoid checking if this renderer is a SpriteRenderer to avoid unneeded dynamic cast to slow down execution
        renderer.SetColour(_color);

        if (renderer.transform.childCount > 0)
        {
            foreach (Transform child in renderer.transform)
            {
                Renderer childRenderer = child.GetComponent<Renderer>();
                childRenderer?.SetColourRecursive(_color);
            }
        }
    }
    #endregion
    // ********************************************************************
}
#endregion
// ************************************************************************

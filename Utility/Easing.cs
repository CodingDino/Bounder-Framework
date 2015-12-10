/*
 * QuickStart Unity Pack is Copyright (c) 2015 Jeff Regis
 * See LICENSE.txt or visit http://opensource.org/licenses/MIT for details about the MIT License.
 */

/*
 * This document contains works from http://www.robertpenner.com/easing/ licensed under the BSD
 * 3-Clause License:
 * 
 * Copyright © 2001 Robert Penner
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, are permitted
 * provided that the following conditions are met:
 * 
 * 1) Redistributions of source code must retain the above copyright notice, this list of conditions
 * and the following disclaimer.
 * 2) Redistributions in binary form must reproduce the above copyright notice, this list of
 * conditions and the following disclaimer in the documentation and/or other materials provided with
 * the distribution.
 * 3) Neither the name of the author nor the names of contributors may be used to endorse or promote
 * products derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
 * FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY
 * WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using UnityEngine;

/// <summary>
/// An easing function.
/// </summary>
public enum EasingFunction
{
	Linear,
	QuadEaseIn,
	QuadEaseOut,
	QuadEaseInOut,
	QuadEaseOutIn,
	CubicEaseIn,
	CubicEaseOut,
	CubicEaseInOut,
	CubicEaseOutIn,
	QuartEaseIn,
	QuartEaseOut,
	QuartEaseInOut,
	QuartEaseOutIn,
	QuintEaseIn,
	QuintEaseOut,
	QuintEaseInOut,
	QuintEaseOutIn,
	ExpoEaseIn,
	ExpoEaseOut,
	ExpoEaseInOut,
	ExpoEaseOutIn,
	SineEaseIn,
	SineEaseOut,
	SineEaseInOut,
	SineEaseOutIn,
	CircEaseIn,
	CircEaseOut,
	CircEaseInOut,
	CircEaseOutIn,
	ElasticEaseIn,
	ElasticEaseOut,
	ElasticEaseInOut,
	ElasticEaseOutIn,
	BounceEaseIn,
	BounceEaseOut,
	BounceEaseInOut,
	BounceEaseOutIn,
	BackEaseIn,
	BackEaseOut,
	BackEaseInOut,
	BackEaseOutIn
}

/// <summary>
/// Static functions for interpolating values over a distance over time.
/// </summary>
public static class Easing
{
	
	/// <summary>
	/// Converts an EasingFunction enum to the corresponding function.
	/// </summary>
	/// <param name="function">The EasingFunction enum.</param>
	/// <returns>The actual easing function.</returns>
	public static Func<float, float, float, float, float> GetFunc(EasingFunction function)
	{
		switch (function)
		{
		case EasingFunction.Linear:
			return Easing.Linear;
			
		case EasingFunction.QuadEaseIn:
			return Easing.QuadEaseIn;
		case EasingFunction.QuadEaseOut:
			return Easing.QuadEaseOut;
		case EasingFunction.QuadEaseInOut:
			return Easing.QuadEaseInOut;
		case EasingFunction.QuadEaseOutIn:
			return Easing.QuadEaseOutIn;
			
		case EasingFunction.CubicEaseIn:
			return Easing.CubicEaseIn;
		case EasingFunction.CubicEaseOut:
			return Easing.CubicEaseOut;
		case EasingFunction.CubicEaseInOut:
			return Easing.CubicEaseInOut;
		case EasingFunction.CubicEaseOutIn:
			return Easing.CubicEaseOutIn;
			
		case EasingFunction.QuartEaseIn:
			return Easing.QuartEaseIn;
		case EasingFunction.QuartEaseOut:
			return Easing.QuartEaseOut;
		case EasingFunction.QuartEaseInOut:
			return Easing.QuartEaseInOut;
		case EasingFunction.QuartEaseOutIn:
			return Easing.QuartEaseOutIn;
			
		case EasingFunction.QuintEaseIn:
			return Easing.QuintEaseIn;
		case EasingFunction.QuintEaseOut:
			return Easing.QuintEaseOut;
		case EasingFunction.QuintEaseInOut:
			return Easing.QuintEaseInOut;
		case EasingFunction.QuintEaseOutIn:
			return Easing.QuintEaseOutIn;
			
		case EasingFunction.ExpoEaseIn:
			return Easing.ExpoEaseIn;
		case EasingFunction.ExpoEaseOut:
			return Easing.ExpoEaseOut;
		case EasingFunction.ExpoEaseInOut:
			return Easing.ExpoEaseInOut;
		case EasingFunction.ExpoEaseOutIn:
			return Easing.ExpoEaseOutIn;
			
		case EasingFunction.SineEaseIn:
			return Easing.SineEaseIn;
		case EasingFunction.SineEaseOut:
			return Easing.SineEaseOut;
		case EasingFunction.SineEaseInOut:
			return Easing.SineEaseInOut;
		case EasingFunction.SineEaseOutIn:
			return Easing.SineEaseOutIn;
			
		case EasingFunction.CircEaseIn:
			return Easing.CircEaseIn;
		case EasingFunction.CircEaseOut:
			return Easing.CircEaseOut;
		case EasingFunction.CircEaseInOut:
			return Easing.CircEaseInOut;
		case EasingFunction.CircEaseOutIn:
			return Easing.CircEaseOutIn;
			
		case EasingFunction.ElasticEaseIn:
			return Easing.ElasticEaseIn;
		case EasingFunction.ElasticEaseOut:
			return Easing.ElasticEaseOut;
		case EasingFunction.ElasticEaseInOut:
			return Easing.ElasticEaseInOut;
		case EasingFunction.ElasticEaseOutIn:
			return Easing.ElasticEaseOutIn;
			
		case EasingFunction.BounceEaseIn:
			return Easing.BounceEaseIn;
		case EasingFunction.BounceEaseOut:
			return Easing.BounceEaseOut;
		case EasingFunction.BounceEaseInOut:
			return Easing.BounceEaseInOut;
		case EasingFunction.BounceEaseOutIn:
			return Easing.BounceEaseOutIn;
			
		case EasingFunction.BackEaseIn:
			return Easing.BackEaseIn;
		case EasingFunction.BackEaseOut:
			return Easing.BackEaseOut;
		case EasingFunction.BackEaseInOut:
			return Easing.BackEaseInOut;
		case EasingFunction.BackEaseOutIn:
			return Easing.BackEaseOutIn;
			
		default:
			return Easing.Linear;
		}
	}
	
	#region Linear
	
	/// <summary>
	/// Easing equation function for a simple linear tweening, with no easing.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float Linear(float t, float b, float c, float d)
	{
		return c * t / d + b;
	}
	
	#endregion
	
	#region Expo
	
	/// <summary>
	/// Easing equation function for an exponential (2^t) easing out: 
	/// decelerating from zero velocity.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float ExpoEaseOut(float t, float b, float c, float d)
	{
		return (t == d) ? b + c : c * (-Mathf.Pow(2, -10 * t / d) + 1) + b;
	}
	
	/// <summary>
	/// Easing equation function for an exponential (2^t) easing in: 
	/// accelerating from zero velocity.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float ExpoEaseIn(float t, float b, float c, float d)
	{
		return (t == 0) ? b : c * Mathf.Pow(2, 10 * (t / d - 1)) + b;
	}
	
	/// <summary>
	/// Easing equation function for an exponential (2^t) easing in/out: 
	/// acceleration until halfway, then deceleration.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float ExpoEaseInOut(float t, float b, float c, float d)
	{
		if (t == 0)
			return b;
		
		if (t == d)
			return b + c;
		
		if ((t /= d / 2) < 1)
			return c / 2 * Mathf.Pow(2, 10 * (t - 1)) + b;
		
		return c / 2 * (-Mathf.Pow(2, -10 * --t) + 2) + b;
	}
	
	/// <summary>
	/// Easing equation function for an exponential (2^t) easing out/in: 
	/// deceleration until halfway, then acceleration.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float ExpoEaseOutIn(float t, float b, float c, float d)
	{
		if (t < d / 2)
			return ExpoEaseOut(t * 2, b, c / 2, d);
		
		return ExpoEaseIn((t * 2) - d, b + c / 2, c / 2, d);
	}
	
	#endregion
	
	#region Circular
	
	/// <summary>
	/// Easing equation function for a circular (sqrt(1-t^2)) easing out: 
	/// decelerating from zero velocity.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float CircEaseOut(float t, float b, float c, float d)
	{
		return c * Mathf.Sqrt(1 - (t = t / d - 1) * t) + b;
	}
	
	/// <summary>
	/// Easing equation function for a circular (sqrt(1-t^2)) easing in: 
	/// accelerating from zero velocity.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float CircEaseIn(float t, float b, float c, float d)
	{
		return -c * (Mathf.Sqrt(1 - (t /= d) * t) - 1) + b;
	}
	
	/// <summary>
	/// Easing equation function for a circular (sqrt(1-t^2)) easing in/out: 
	/// acceleration until halfway, then deceleration.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float CircEaseInOut(float t, float b, float c, float d)
	{
		if ((t /= d / 2) < 1)
			return -c / 2 * (Mathf.Sqrt(1 - t * t) - 1) + b;
		
		return c / 2 * (Mathf.Sqrt(1 - (t -= 2) * t) + 1) + b;
	}
	
	/// <summary>
	/// Easing equation function for a circular (sqrt(1-t^2)) easing in/out: 
	/// acceleration until halfway, then deceleration.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float CircEaseOutIn(float t, float b, float c, float d)
	{
		if (t < d / 2)
			return CircEaseOut(t * 2, b, c / 2, d);
		
		return CircEaseIn((t * 2) - d, b + c / 2, c / 2, d);
	}
	
	#endregion
	
	#region Quad
	
	/// <summary>
	/// Easing equation function for a quadratic (t^2) easing out: 
	/// decelerating from zero velocity.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float QuadEaseOut(float t, float b, float c, float d)
	{
		return -c * (t /= d) * (t - 2) + b;
	}
	
	/// <summary>
	/// Easing equation function for a quadratic (t^2) easing in: 
	/// accelerating from zero velocity.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float QuadEaseIn(float t, float b, float c, float d)
	{
		return c * (t /= d) * t + b;
	}
	
	/// <summary>
	/// Easing equation function for a quadratic (t^2) easing in/out: 
	/// acceleration until halfway, then deceleration.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float QuadEaseInOut(float t, float b, float c, float d)
	{
		if ((t /= d / 2) < 1)
			return c / 2 * t * t + b;
		
		return -c / 2 * ((--t) * (t - 2) - 1) + b;
	}
	
	/// <summary>
	/// Easing equation function for a quadratic (t^2) easing out/in: 
	/// deceleration until halfway, then acceleration.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float QuadEaseOutIn(float t, float b, float c, float d)
	{
		if (t < d / 2)
			return QuadEaseOut(t * 2, b, c / 2, d);
		
		return QuadEaseIn((t * 2) - d, b + c / 2, c / 2, d);
	}
	
	#endregion
	
	#region Sine
	
	/// <summary>
	/// Easing equation function for a sinusoidal (sin(t)) easing out: 
	/// decelerating from zero velocity.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float SineEaseOut(float t, float b, float c, float d)
	{
		return c * Mathf.Sin(t / d * (Mathf.PI / 2)) + b;
	}
	
	/// <summary>
	/// Easing equation function for a sinusoidal (sin(t)) easing in: 
	/// accelerating from zero velocity.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float SineEaseIn(float t, float b, float c, float d)
	{
		return -c * Mathf.Cos(t / d * (Mathf.PI / 2)) + c + b;
	}
	
	/// <summary>
	/// Easing equation function for a sinusoidal (sin(t)) easing in/out: 
	/// acceleration until halfway, then deceleration.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float SineEaseInOut(float t, float b, float c, float d)
	{
		if ((t /= d / 2) < 1)
			return c / 2 * (Mathf.Sin(Mathf.PI * t / 2)) + b;
		
		return -c / 2 * (Mathf.Cos(Mathf.PI * --t / 2) - 2) + b;
	}
	
	/// <summary>
	/// Easing equation function for a sinusoidal (sin(t)) easing in/out: 
	/// deceleration until halfway, then acceleration.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float SineEaseOutIn(float t, float b, float c, float d)
	{
		if (t < d / 2)
			return SineEaseOut(t * 2, b, c / 2, d);
		
		return SineEaseIn((t * 2) - d, b + c / 2, c / 2, d);
	}
	
	#endregion
	
	#region Cubic
	
	/// <summary>
	/// Easing equation function for a cubic (t^3) easing out: 
	/// decelerating from zero velocity.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float CubicEaseOut(float t, float b, float c, float d)
	{
		return c * ((t = t / d - 1) * t * t + 1) + b;
	}
	
	/// <summary>
	/// Easing equation function for a cubic (t^3) easing in: 
	/// accelerating from zero velocity.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float CubicEaseIn(float t, float b, float c, float d)
	{
		return c * (t /= d) * t * t + b;
	}
	
	/// <summary>
	/// Easing equation function for a cubic (t^3) easing in/out: 
	/// acceleration until halfway, then deceleration.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float CubicEaseInOut(float t, float b, float c, float d)
	{
		if ((t /= d / 2) < 1)
			return c / 2 * t * t * t + b;
		
		return c / 2 * ((t -= 2) * t * t + 2) + b;
	}
	
	/// <summary>
	/// Easing equation function for a cubic (t^3) easing out/in: 
	/// deceleration until halfway, then acceleration.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float CubicEaseOutIn(float t, float b, float c, float d)
	{
		if (t < d / 2)
			return CubicEaseOut(t * 2, b, c / 2, d);
		
		return CubicEaseIn((t * 2) - d, b + c / 2, c / 2, d);
	}
	
	#endregion
	
	#region Quartic
	
	/// <summary>
	/// Easing equation function for a quartic (t^4) easing out: 
	/// decelerating from zero velocity.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float QuartEaseOut(float t, float b, float c, float d)
	{
		return -c * ((t = t / d - 1) * t * t * t - 1) + b;
	}
	
	/// <summary>
	/// Easing equation function for a quartic (t^4) easing in: 
	/// accelerating from zero velocity.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float QuartEaseIn(float t, float b, float c, float d)
	{
		return c * (t /= d) * t * t * t + b;
	}
	
	/// <summary>
	/// Easing equation function for a quartic (t^4) easing in/out: 
	/// acceleration until halfway, then deceleration.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float QuartEaseInOut(float t, float b, float c, float d)
	{
		if ((t /= d / 2) < 1)
			return c / 2 * t * t * t * t + b;
		
		return -c / 2 * ((t -= 2) * t * t * t - 2) + b;
	}
	
	/// <summary>
	/// Easing equation function for a quartic (t^4) easing out/in: 
	/// deceleration until halfway, then acceleration.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float QuartEaseOutIn(float t, float b, float c, float d)
	{
		if (t < d / 2)
			return QuartEaseOut(t * 2, b, c / 2, d);
		
		return QuartEaseIn((t * 2) - d, b + c / 2, c / 2, d);
	}
	
	#endregion
	
	#region Quintic
	
	/// <summary>
	/// Easing equation function for a quintic (t^5) easing out: 
	/// decelerating from zero velocity.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float QuintEaseOut(float t, float b, float c, float d)
	{
		return c * ((t = t / d - 1) * t * t * t * t + 1) + b;
	}
	
	/// <summary>
	/// Easing equation function for a quintic (t^5) easing in: 
	/// accelerating from zero velocity.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float QuintEaseIn(float t, float b, float c, float d)
	{
		return c * (t /= d) * t * t * t * t + b;
	}
	
	/// <summary>
	/// Easing equation function for a quintic (t^5) easing in/out: 
	/// acceleration until halfway, then deceleration.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float QuintEaseInOut(float t, float b, float c, float d)
	{
		if ((t /= d / 2) < 1)
			return c / 2 * t * t * t * t * t + b;
		return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
	}
	
	/// <summary>
	/// Easing equation function for a quintic (t^5) easing in/out: 
	/// acceleration until halfway, then deceleration.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float QuintEaseOutIn(float t, float b, float c, float d)
	{
		if (t < d / 2)
			return QuintEaseOut(t * 2, b, c / 2, d);
		return QuintEaseIn((t * 2) - d, b + c / 2, c / 2, d);
	}
	
	#endregion
	
	#region Elastic
	
	/// <summary>
	/// Easing equation function for an elastic (exponentially decaying sine wave) easing out: 
	/// decelerating from zero velocity.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float ElasticEaseOut(float t, float b, float c, float d)
	{
		if ((t /= d) == 1)
			return b + c;
		
		float p = d * .3f;
		float s = p / 4;
		
		return (c * Mathf.Pow(2, -10 * t) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p) + c + b);
	}
	
	/// <summary>
	/// Easing equation function for an elastic (exponentially decaying sine wave) easing in: 
	/// accelerating from zero velocity.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float ElasticEaseIn(float t, float b, float c, float d)
	{
		if ((t /= d) == 1)
			return b + c;
		
		float p = d * .3f;
		float s = p / 4;
		
		return -(c * Mathf.Pow(2, 10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p)) + b;
	}
	
	/// <summary>
	/// Easing equation function for an elastic (exponentially decaying sine wave) easing in/out: 
	/// acceleration until halfway, then deceleration.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float ElasticEaseInOut(float t, float b, float c, float d)
	{
		if ((t /= d / 2) == 2)
			return b + c;
		
		float p = d * (.3f * 1.5f);
		float s = p / 4;
		
		if (t < 1)
			return -.5f * (c * Mathf.Pow(2, 10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p)) + b;
		return c * Mathf.Pow(2, -10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p) * .5f + c + b;
	}
	
	/// <summary>
	/// Easing equation function for an elastic (exponentially decaying sine wave) easing out/in: 
	/// deceleration until halfway, then acceleration.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float ElasticEaseOutIn(float t, float b, float c, float d)
	{
		if (t < d / 2)
			return ElasticEaseOut(t * 2, b, c / 2, d);
		return ElasticEaseIn((t * 2) - d, b + c / 2, c / 2, d);
	}
	
	#endregion
	
	#region Bounce
	
	/// <summary>
	/// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out: 
	/// decelerating from zero velocity.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float BounceEaseOut(float t, float b, float c, float d)
	{
		if ((t /= d) < (1 / 2.75))
			return c * (7.5625f * t * t) + b;
		else if (t < (2 / 2.75))
			return c * (7.5625f * (t -= (1.5f / 2.75f)) * t + .75f) + b;
		else if (t < (2.5 / 2.75))
			return c * (7.5625f * (t -= (2.25f / 2.75f)) * t + .9375f) + b;
		else
			return c * (7.5625f * (t -= (2.625f / 2.75f)) * t + .984375f) + b;
	}
	
	/// <summary>
	/// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in: 
	/// accelerating from zero velocity.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float BounceEaseIn(float t, float b, float c, float d)
	{
		return c - BounceEaseOut(d - t, 0, c, d) + b;
	}
	
	/// <summary>
	/// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in/out: 
	/// acceleration until halfway, then deceleration.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float BounceEaseInOut(float t, float b, float c, float d)
	{
		if (t < d / 2)
			return BounceEaseIn(t * 2, 0, c, d) * .5f + b;
		else
			return BounceEaseOut(t * 2 - d, 0, c, d) * .5f + c * .5f + b;
	}
	
	/// <summary>
	/// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out/in: 
	/// deceleration until halfway, then acceleration.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float BounceEaseOutIn(float t, float b, float c, float d)
	{
		if (t < d / 2)
			return BounceEaseOut(t * 2, b, c / 2, d);
		return BounceEaseIn((t * 2) - d, b + c / 2, c / 2, d);
	}
	
	#endregion
	
	#region Back
	
	/// <summary>
	/// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing out: 
	/// decelerating from zero velocity.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float BackEaseOut(float t, float b, float c, float d)
	{
		return c * ((t = t / d - 1) * t * ((1.70158f + 1) * t + 1.70158f) + 1) + b;
	}
	
	/// <summary>
	/// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing in: 
	/// accelerating from zero velocity.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float BackEaseIn(float t, float b, float c, float d)
	{
		return c * (t /= d) * t * ((1.70158f + 1) * t - 1.70158f) + b;
	}
	
	/// <summary>
	/// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing in/out: 
	/// acceleration until halfway, then deceleration.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float BackEaseInOut(float t, float b, float c, float d)
	{
		float s = 1.70158f;
		if ((t /= d / 2) < 1)
			return c / 2 * (t * t * (((s *= (1.525f)) + 1) * t - s)) + b;
		return c / 2 * ((t -= 2) * t * (((s *= (1.525f)) + 1) * t + s) + 2) + b;
	}
	
	/// <summary>
	/// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing out/in: 
	/// deceleration until halfway, then acceleration.
	/// </summary>
	/// <param name="t">Current time in seconds.</param>
	/// <param name="b">Starting value.</param>
	/// <param name="c">Final value.</param>
	/// <param name="d">Duration of animation.</param>
	/// <returns>The correct value.</returns>
	public static float BackEaseOutIn(float t, float b, float c, float d)
	{
		if (t < d / 2)
			return BackEaseOut(t * 2, b, c / 2, d);
		return BackEaseIn((t * 2) - d, b + c / 2, c / 2, d);
	}
	
	#endregion
}
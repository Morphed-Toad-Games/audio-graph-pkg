using System;
using UnityEngine;

public static class CurveHelpers
{
    public static AnimationCurve GenerateLogCurve(float min, float max, int keyframeCount)
    {
        if (min >= max)
            throw new ArgumentException("min cannot be greater than or equal to the max");

        if (keyframeCount <= 0)
            throw new ArgumentException("keyframeCount must be 1 or greater");

        Keyframe[] keyframes = new Keyframe[keyframeCount];

        // Calculate the step size for x-values
        float xStep = (max - min) / (keyframeCount - 1);

        // Generate keyframes with logarithmic y-values normalized and inverted to [1, 0]
        for (int i = 0; i < keyframeCount; i++)
        {
            float x = min + i * xStep;
            // Normalize x within the range [min, max]
            float t = (x - min) / (max - min);
            // Apply logarithmic scaling and normalize to [1, 0]
            float y = 1f - Mathf.Log10(1f + 9f * t) / Mathf.Log10(10f);  // The 1+9*t scales the range to be within [1, 10] logarithmically
            keyframes[i] = new Keyframe(x, y);
        }

        // Create an AnimationCurve and set smooth tangents
        AnimationCurve curve = new AnimationCurve(keyframes);
        for (int i = 0; i < keyframes.Length; i++)
        {
            curve.SmoothTangents(i, 0.1f);
        }

        return curve;
    }
}
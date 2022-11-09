using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUtils
{
    public static bool ScaleBToGetMagAPlusB(Vector3 A, Vector3 B, float mag, out float alpha)
    {
        alpha = 0;

        //Impossible : magnitude positive
        if (mag < 0)
            return false;

        //Impossible de scaler B qui est nul
        if (mag > 0 && B.sqrMagnitude == 0)
            return false;

        float a = B.sqrMagnitude;
        float b = 2 * Vector3.Dot(A, B);
        float c = A.sqrMagnitude - (mag * mag);

        float delta = (b * b) - 4 * a * c;

        //impossible
        if (delta < 0)
            return false;

        float racDelta = delta > 0 ? Mathf.Sqrt(delta) : delta;
        alpha = (-b + racDelta) / (2 * a);

        return true;
    }

    public static void test_ScaleBToGetMagAPlusB()
    {
        for (int i = 0; i < 100; i++)
        {
            Vector3 A = Random.onUnitSphere * Random.Range(-100, 100);
            Vector3 B = Random.onUnitSphere * Random.Range(-100, 100);
            float magSum = Random.Range(0, 100);
            float alpha;
            if (ScaleBToGetMagAPlusB(A, B, magSum, out alpha))
                Debug.Log("A:" + A + " B:" + B + " magSum:" + magSum + " alpha:" + alpha + " res:" + (magSum - (A + alpha * B).magnitude));
        }
    }
}
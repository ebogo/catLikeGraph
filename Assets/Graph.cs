using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Graph : MonoBehaviour
{
    //THIS IS YOUR HEADER SPACEEEEEEE

    //how to: get elapsed time from click or command?

    //vars + delegates
    [Range(10, 100)]
    public int resolution = 50;
    int localResolution;
    public GraphFunctionName functionName;
    static GraphFunction[] functions = { SineFunction, Sine2DFunction, MultiSineFunction, MultiSine2DFunction, Ripple, Cylinder, Sphere, Torus};
    const float pi = Mathf.PI;

    //objs (?)
    public Transform pointPrefab;
    Transform point;
    Transform[] points;



    void Awake()
    {
        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        points = new Transform[resolution * resolution];
        for (int i = 0; i < points.Length; i++)
        {
            Transform point = Instantiate(pointPrefab);
            point.localScale = scale;
            point.SetParent(transform, false);
            points[i] = point;
        }
        /*
         * localResolution = resolution;
         * SetupGraph(localResolution);
         * */

    }

    void Update()
    {

        float t = Time.time;
        GraphFunction f = functions[(int)functionName];

        float step = 2f / resolution;
        for (int i = 0, z = 0; z < resolution; z++)
        {
            float v = (z + 0.5f) * step - 1f;
            for (int x = 0; x < resolution; x++, i++)
            {
                float u = (x + 0.5f) * step - 1f;
                points[i].localPosition = f(u, v, t);
            }
        }
        /*
        if (localResolution != resolution)
        {
            localResolution = resolution;
            SetupGraph(localResolution);
        }

        //f = (function == 0 ? SineFunction : MultiSineFunction); //understand error

        for (int i = 0; i < points.Length; i++)
        {

            Transform point = points[i];
            Vector3 position = point.localPosition;

            position.y = f(position.x, position.z, t);
            point.localPosition = position;
        }
        */
    }

    static Vector3 SineFunction(float x, float z, float t) {

        Vector3 p;
        p.x = x;
        p.y = Mathf.Sin(pi * (x + t));
        p.z = z;
        return p;
    }

    static Vector3 Sine2DFunction(float x, float z, float t) {
        /*
         * code for that grid life
         * //return Mathf.Sin(pi * (x + z + t)); //pointy ends on xz diag
        //below: ( sin(pi(x+t)) + sin(pi(z+t)) ) / 2 ;
        float y = Mathf.Sin(pi * (x + t));
        y += Mathf.Sin(pi * (z + t));
        y *= 0.5f; //mult is faster than div, k
        return y; //float!
        */
        Vector3 p;
        p.x = x;
        p.y = Mathf.Sin(pi *(x + t));
        p.y += Mathf.Sin(pi * (z + t));
        p.y *= 0.5f;
        p.z = z;
        return p;

    }


    static Vector3 MultiSineFunction(float x, float z, float t) {

        /*
        float y = Mathf.Sin(pi * (x + t));
        y += Mathf.Sin(2f * pi * (x + 2f * t)) / 2f;
        y *= 2f / 3f;
        return y; //float!
        */

        Vector3 p;
        p.x = x;
        p.y = Mathf.Sin(pi * (x + t));
        p.y += Mathf.Sin(2f * pi * (x + 2f * t)) / 2f;
        p.y *= 2f / 3f;
        p.z = z;
        return p;
    }

    static Vector3 MultiSine2DFunction (float x, float z, float t){
        //f(x,z,t)=M + S_x + S_z //main + secondary in x, secnd z
        //M = 4 * sin (pi (x+z+t/2))
        //S_x = sin (pi(x+t))
        //S_z = sin (2pi (z+2t)) /2  //double freq half amplitude
        //f(x,z,t) = 4M + S_x + S_z/2 (divided by 5.5 to normalize to -1,1

        /*
         float y = 4f * Mathf.Sin(pi * (x + z + t * 0.5f));
        y += Mathf.Sin(pi * (x + t));
        y += Mathf.Sin(2f * pi * (z + 2f * t)) * 0.5f;
        y *= 1f / 5.5f; //note!
        return y; //float!
        */

        Vector3 p;
        p.x = x;
        p.y = 4f * Mathf.Sin(pi * (x + z + t / 2f));
        p.y += Mathf.Sin(pi * (x + t));
        p.y += Mathf.Sin(2f * pi * (z + 2f * t)) * 0.5f;
        p.y *= 1f / 5.5f;
        p.z = z;
        return p;
    }

    static Vector3 Ripple (float x, float z, float t){
        /*
         float d = Mathf.Sqrt(x * x + z * z);
        float y = Mathf.Sin (4f* pi * d -t); //4x freq //-t moves outward like irl
        y *= (1 / (1f + 10f * d));
        return y; //float!!
        */
        Vector3 p;
        float d = Mathf.Sqrt(x * x + z * z);
        p.x = x;
        p.y = Mathf.Sin(pi * (4f * d - t));
        p.y /= 1f + 10f * d;
        p.z = z;
        return p;
    }

    static Vector3 Cylinder(float u, float v, float t)
    {
        Vector3 p;
        //f(u)=[[sin(pi*u)],[0],[cos(pi*u)]] //circle
        //f(u,v)=[[r* sin(pi*u)],[v],[r*cos(pi*v)]] radius r

        float r = 0.8f + Mathf.Sin(pi * (6f * u + 2f * v +t)) * 0.2f; //*t, ukno
        p.x = r * Mathf.Sin(pi * u );
        //p.y = u; //ribbon vibey
        p.y = v;
        p.z = r * Mathf.Cos(pi * u);
        return p;
    }
    static Vector3 Sphere(float u, float v, float t)
    {

        //f(u, v) =[[Ssin(piu)],[Rsin((piv) / 2)],[Scos(piu)]]
        Vector3 p;
        //UV sphere
        float r = 0.8f + Mathf.Sin(pi * (6f * u + t)) * 0.1f;
        r += Mathf.Sin(pi * (4f * v + t)) * 0.1f;
        float s = r * Mathf.Cos(pi * 0.5f * v);
        p.x = s * Mathf.Sin(pi * u);
        p.y = r * Mathf.Sin(pi * 0.5f * v);
        p.z = s * Mathf.Cos(pi * u);
        return p;
    }

    static Vector3 Torus(float u, float v, float t)
    {
        Vector3 p;
        //v1
        // float s = Mathf.Cos(pi * 0.5f * v) + 0.5f;
        // p.y = Mathf.Sin(pi * 0.5f * v);
        //v2
        /*
        float s = Mathf.Cos(pi * v) + 0.5f;
        p.x = s * Mathf.Sin(pi * u);
        p.y = Mathf.Sin(pi * v);
        p.z = s * Mathf.Cos(pi * u);
        */
        //v ring flat
        //float r1 = 1f;
        //float r2 = 0.5f;

        //v ring waviiy
        float r1 = 0.65f + Mathf.Sin(pi * (6f * u + t)) * 0.1f;
        float r2 = 0.2f + Mathf.Sin(pi * (4f * v + t)) * 0.05f;
        float s = r2 * Mathf.Cos(pi * v) + r1;
        p.x = s * Mathf.Sin(pi * u);
        p.y = r2 * Mathf.Sin(pi * v);
        p.z = s * Mathf.Cos(pi * u);

        return p;

    }

    /*
     * void SetupGraph(int res)
     {
         points = new Transform[res * res];
         float step = 2f / res;

         Vector3 scale = Vector3.one * step;

         Vector3 position; //if you set it to .zero it sets the first one at the origin
         position.z = 0f;

         float t = Time.time;
         for (int i = 0, z = 0; z < res; z++)
         {
             position.z = (z + 0.5f) * step - 1f;

             for (int x = 0; x < res; x++, i++) //ye!
             {
                 point = Instantiate(pointPrefab);
                 //shift them over, bring them back together to fill gaps
                 //adjust for the size of the cubes to fit snug in the -1,1 range
                 position.x = (x + 0.5f) * step - 1f;
                 position.y = MultiSineFunction(position.x, position.z, t);
                 point.localPosition = position;
                 point.localScale = scale;
                 point.SetParent(transform, false); //attacing: Transform pt = Graph.transform
                 points[i] = point;
             }

         }
     }
     */
}
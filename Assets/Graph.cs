using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Graph : MonoBehaviour {
    //THIS IS YOUR HEADER SPACEEEEEEE

    //vars + delegates
    [Range (10,100)]
    public int resolution = 50;
    int localResolution;
    public GraphFunctionName functionName;
    static GraphFunction[] functions = {SineFunction, MultiSineFunction};


    //objs (?)
    public Transform pointPrefab;
    Transform point;
    Transform[] points;



    void Awake(){
        localResolution = resolution;
        SetupGraph(localResolution);

    }
    void Start () {
		
	}
	
	void Update () {

        float t = Time.time;
        GraphFunction f = functions[(int)functionName];

        if (localResolution != resolution){
            localResolution = resolution;
            SetupGraph(localResolution);
        }

        //f = (function == 0 ? SineFunction : MultiSineFunction); //understand error

        for (int i = 0; i < points.Length; i++){

            Transform point = points[i];
            Vector3 position = point.localPosition;

            position.y = f(position.x, position.z, t);
            point.localPosition = position;
        }
	}

   static  float SineFunction(float x, float z, float t){

        return Mathf.Sin(Mathf.PI * (x+t));
    }

   static float MultiSineFunction(float x, float z, float t){

        float y = Mathf.Sin(Mathf.PI * (x + t));
        y += Mathf.Sin(2f * Mathf.PI * (x + 2f * t))/2f;
        y *= 2f / 3f;
        return y;
    }


    void SetupGraph(int res)
    {
        points = new Transform[res];
        float step = 2f / res;

        Vector3 scale = Vector3.one * step;

        Vector3 position; //if you set it to .zero it sets the first one at the origin
        position.z = 0f;

        float t = Time.time;
        for (int i = 0; i < points.Length; i++)
        {
            point = Instantiate(pointPrefab);
            //shift them over, bring them back together to fill gaps
            //adjust for the size of the cubes to fit snug in the -1,1 range
            position.x = (i + 0.5f) * step - 1f;
            position.y = MultiSineFunction(position.x, position.z, t);
            point.localPosition = position;
            point.localScale = scale;
            point.SetParent(transform, false); //attacing: Transform pt = Graph.transform
            points[i] = point;
        }

    }
}

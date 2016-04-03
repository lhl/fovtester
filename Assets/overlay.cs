using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class overlay : MonoBehaviour {
    public GameObject cursor;

    // private Vector3 mouse_center;

    private Vector3 last_pos;

    private List<Vector2> vertices2D;


    // Use this for initialization
    void Start () {
        // mouse_center = Input.mousePosition;
        vertices2D = new List<Vector2>();
        last_pos = new Vector3(0.0f, 0.0f, 0.0f);
    }

// Update is called once per frame
void Update () {
        float delta_x = 0.0f;
        float delta_y = 0.0f;

        if(OVRInput.Get(OVRInput.Button.DpadUp))
        {
            delta_y = 0.0002f;
        }
        if (OVRInput.Get(OVRInput.Button.DpadDown))
        {
            delta_y = -0.0002f;
        }
        if (OVRInput.Get(OVRInput.Button.DpadLeft))
        {
            delta_x = -0.0002f;
        }
        if (OVRInput.Get(OVRInput.Button.DpadRight))
        {
            delta_x = 0.0002f;
        }

        // Set Location
        last_pos = cursor.transform.localPosition;
        cursor.transform.localPosition = new Vector3(
            cursor.transform.localPosition.x + delta_x,
            cursor.transform.localPosition.y + delta_y,
            -1.2f
        );


        if (OVRInput.Get(OVRInput.Button.One) && last_pos != cursor.transform.localPosition)
        {
            last_pos = cursor.transform.localPosition;

            Debug.Log(cursor.transform.localPosition);

            vertices2D.Add(new Vector2(
                cursor.transform.localPosition.x,
                cursor.transform.localPosition.y
            ));
        }


        /*** Mouse Cursor ***/
        /*
        // Debug.Log(Input.mousePosition);
        cursor.transform.localPosition = new Vector3(
            (Input.mousePosition.x - mouse_center.x) * 0.001f,
            (Input.mousePosition.y - mouse_center.y) * 0.001f,
            -1.2f
        );

        /*
          Extents are +/- 173.2051 (346.4102/2)
        */
        // Debug.Log(cursor.transform.position);
        // Debug.Log(cursor.transform.localPosition);


        /*** Add Vertices ***/
        /*
        if (Input.GetButtonDown("Fire1"))
        {
            vertices2D.Add(new Vector2(
                cursor.transform.localPosition.x,
                cursor.transform.localPosition.y
            ));
        }
        */

        /*** Create Polygon Overlay ***/

        /*
        Basic dynamic mesh creation:
        http://docs.unity3d.com/ScriptReference/Mesh.html

        Tutorial of grid creation of vertices, triangles
        http://catlikecoding.com/unity/tutorials/procedural-grid/
        */



        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        /*
        mesh.vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0) };
        mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1) };
        mesh.triangles = new int[] { 0, 1, 2 };
        */

        // Create Vector2 vertices
        Vector2[] vertices2DA = vertices2D.ToArray();

        // Use the triangulator to get indices for creating triangles
        Triangulator tr = new Triangulator(vertices2DA);
        int[] indices = tr.Triangulate();

        // Create the Vector3 vertices
        Vector3[] vertices = new Vector3[vertices2DA.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
        }

        // Create the mesh
        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();





        // Oculus
        /*
          Remote: Button.Dpad* and Button.One
          XBox: Button.Dpad* or Axis2D.PrimaryThumbstick (Vector2) or Axis2D.SecondaryThumbstick (Vector2) and Button.One
        */

        /*
        if(OVRInput.Get(OVRInput.Button.One))
        {
            Debug.Log("Button One!");
        }

        if (OVRInput.Get(OVRInput.Button.DpadUp))
        {
            Debug.Log("DPadUp");
        }

        if (OVRInput.Get(OVRInput.Button.DpadDown))
        {
            Debug.Log("DPadDown");
        }
        */

        /* 
        Mesh Rendering
        http://docs.unity3d.com/ScriptReference/Mesh.html

        Line Rendering
        http://www.everyday3d.com/blog/index.php/2010/03/15/3-ways-to-draw-3d-lines-in-unity3d/
        http://www.theappguruz.com/blog/draw-line-mouse-move-detect-line-collision-unity2d-unity3d
        http://answers.unity3d.com/questions/184442/drawing-lines-from-mouse-position.html
        
        Asset Painting
        https://www.assetstore.unity3d.com/en/#!/content/33506
        https://www.assetstore.unity3d.com/en/#!/content/15433
        */
    }
}


public class Triangulator
{
    private List<Vector2> m_points = new List<Vector2>();

    public Triangulator(Vector2[] points)
    {
        m_points = new List<Vector2>(points);
    }

    public int[] Triangulate()
    {
        List<int> indices = new List<int>();

        int n = m_points.Count;
        if (n < 3)
            return indices.ToArray();

        int[] V = new int[n];
        if (Area() > 0)
        {
            for (int v = 0; v < n; v++)
                V[v] = v;
        }
        else {
            for (int v = 0; v < n; v++)
                V[v] = (n - 1) - v;
        }

        int nv = n;
        int count = 2 * nv;
        for (int m = 0, v = nv - 1; nv > 2;)
        {
            if ((count--) <= 0)
                return indices.ToArray();

            int u = v;
            if (nv <= u)
                u = 0;
            v = u + 1;
            if (nv <= v)
                v = 0;
            int w = v + 1;
            if (nv <= w)
                w = 0;

            if (Snip(u, v, w, nv, V))
            {
                int a, b, c, s, t;
                a = V[u];
                b = V[v];
                c = V[w];
                indices.Add(a);
                indices.Add(b);
                indices.Add(c);
                m++;
                for (s = v, t = v + 1; t < nv; s++, t++)
                    V[s] = V[t];
                nv--;
                count = 2 * nv;
            }
        }

        indices.Reverse();
        return indices.ToArray();
    }

    private float Area()
    {
        int n = m_points.Count;
        float A = 0.0f;
        for (int p = n - 1, q = 0; q < n; p = q++)
        {
            Vector2 pval = m_points[p];
            Vector2 qval = m_points[q];
            A += pval.x * qval.y - qval.x * pval.y;
        }
        return (A * 0.5f);
    }

    private bool Snip(int u, int v, int w, int n, int[] V)
    {
        int p;
        Vector2 A = m_points[V[u]];
        Vector2 B = m_points[V[v]];
        Vector2 C = m_points[V[w]];
        if (Mathf.Epsilon > (((B.x - A.x) * (C.y - A.y)) - ((B.y - A.y) * (C.x - A.x))))
            return false;
        for (p = 0; p < n; p++)
        {
            if ((p == u) || (p == v) || (p == w))
                continue;
            Vector2 P = m_points[V[p]];
            if (InsideTriangle(A, B, C, P))
                return false;
        }
        return true;
    }

    private bool InsideTriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
    {
        float ax, ay, bx, by, cx, cy, apx, apy, bpx, bpy, cpx, cpy;
        float cCROSSap, bCROSScp, aCROSSbp;

        ax = C.x - B.x; ay = C.y - B.y;
        bx = A.x - C.x; by = A.y - C.y;
        cx = B.x - A.x; cy = B.y - A.y;
        apx = P.x - A.x; apy = P.y - A.y;
        bpx = P.x - B.x; bpy = P.y - B.y;
        cpx = P.x - C.x; cpy = P.y - C.y;

        aCROSSbp = ax * bpy - ay * bpx;
        cCROSSap = cx * apy - cy * apx;
        bCROSScp = bx * cpy - by * cpx;

        return ((aCROSSbp >= 0.0f) && (bCROSScp >= 0.0f) && (cCROSSap >= 0.0f));
    }
}
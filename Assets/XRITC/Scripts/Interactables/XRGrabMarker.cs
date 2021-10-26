using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabMarker : MonoBehaviour
{
    private XRGrabInteractable m_InteractableBase;

    public Material DrawMaterial;
    public Color DrawColor = Color.red;
    public float LineWidth = 0.02f;

    public Transform RaycastStart;
    public LayerMask DrawingLayers;
    private Rigidbody _rigidbody;

    public float RaycastLength = 0.01f;

    /// <summary>
    /// Minimum distance required from points to place drawing down
    /// </summary>
    public float MinDrawDistance = 0.02f;
    public float ReuseTolerance = 0.001f;

    //Used for changing color of material
    public int materialColorToChangeIndex = 1;
    public MeshRenderer meshRenderer;
    bool IsNewDraw = false;
    Vector3 lastDrawPoint;
    LineRenderer LineRenderer;

    // Use this to store our Marker's LineRenderers
    Transform root;
    Transform lastTransform;
    Coroutine drawRoutine = null;
    float lastLineWidth = 0;
    int renderLifeTime = 0;
    public bool isDrawing;

    //These events only run once, so dont use them for continous stuff like update
    public UnityEvent onDrawStart;
    public UnityEvent onDrawStop;

    void Awake()
    {
        m_InteractableBase = GetComponent<XRGrabInteractable>();
    }

    [System.Obsolete]
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        //We have to set the color of the tip to the color selected
        ChangeColorDrawColorRPC(DrawColor);
        m_InteractableBase.onSelectEntered.AddListener(StartDrawing);
        m_InteractableBase.onSelectExited.AddListener(StopDrawing);
    }

    private void LateUpdate()
    {
        if (isDrawing)
        {
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        }
        else
        {
            _rigidbody.constraints = RigidbodyConstraints.None;
        }

    }

    void StartDrawing(XRBaseInteractor obj)
    {
        if (drawRoutine == null)
        {
            drawRoutine = StartCoroutine(WriteRoutine());
        }
    }

    void StopDrawing(XRBaseInteractor obj)
    {
        if (drawRoutine != null)
        {
            StopCoroutine(drawRoutine);
            drawRoutine = null;
        }
    }


    IEnumerator WriteRoutine()
    {
        while (true)
        {
            if (Physics.Raycast(RaycastStart.position, RaycastStart.up, out RaycastHit hit, RaycastLength, DrawingLayers, QueryTriggerInteraction.Ignore))
            {
                float tipDistance = Vector3.Distance(hit.point, RaycastStart.transform.position);
                float tipDercentage = tipDistance / RaycastLength;
                Vector3 drawStart = hit.point + (-RaycastStart.up * 0.0005f);
                Quaternion drawRotation = Quaternion.FromToRotation(Vector3.back, hit.normal);
                float lineWidth = LineWidth * (1 - tipDercentage);
                InitDraw(drawStart, drawRotation, lineWidth, DrawColor);
                if (!isDrawing)
                {
                    isDrawing = true;
                    onDrawStart.Invoke();
                    Debug.Log("started drawing");
                }
            }
            else
            {

                if (isDrawing)
                {
                    isDrawing = false;


                    onDrawStop.Invoke();
                    Debug.Log("stopped drawing");

                }

                IsNewDraw = true;
            }
            yield return new WaitForFixedUpdate();
        }
    }


    void InitDraw(Vector3 position, Quaternion rotation, float lineWidth, Color lineColor)
    {
        if (IsNewDraw)
        {
            lastDrawPoint = position;

            DrawPoint(lastDrawPoint, position, lineWidth, lineColor.r, lineColor.g, lineColor.b, lineColor.a, rotation);

            IsNewDraw = false;
        }
        else
        {
            float dist = Vector3.Distance(lastDrawPoint, position);
            if (dist > MinDrawDistance)
            {
                //  lastDrawPoint = DrawPoint(lastDrawPoint, position, lineWidth, DrawColor, rotation);
                DrawPoint(lastDrawPoint, position, lineWidth, lineColor.r, lineColor.g, lineColor.b, lineColor.a, rotation);

                lastDrawPoint = workDrawPoint;


            }
        }
    }


    private Vector3 workDrawPoint;

    Vector3 DrawPoint(Vector3 lastDrawPoint, Vector3 endPosition, float lineWidth, float r, float g, float b, float a, Quaternion rotation)
    {
        Color lineColor = new Color(r, g, b, a);
        var dif = Mathf.Abs(lastLineWidth - lineWidth);
        lastLineWidth = lineWidth;
        if (dif > ReuseTolerance || renderLifeTime >= 98)
        {
            LineRenderer = null;
            renderLifeTime = 0;
        }
        else
        {
            renderLifeTime += 1;
        }
        if (IsNewDraw || LineRenderer == null)
        {
            lastTransform = new GameObject().transform;
            lastTransform.name = "DrawLine";
            lastTransform.tag = "2DLine";
            if (root == null)
            {
                root = new GameObject().transform;
                root.name = "MarkerLineHolder";

            }
            lastTransform.parent = root;
            lastTransform.position = endPosition;
            lastTransform.rotation = rotation;
            LineRenderer = lastTransform.gameObject.AddComponent<LineRenderer>();

            LineRenderer.startColor = lineColor;
            LineRenderer.endColor = lineColor;
            LineRenderer.startWidth = lineWidth;
            LineRenderer.endWidth = lineWidth;
            var curve = new AnimationCurve();
            curve.AddKey(0, lineWidth);
            //curve.AddKey(1, lineWidth);
            LineRenderer.widthCurve = curve;
            if (DrawMaterial)
            {
                LineRenderer.material = DrawMaterial;
            }
            LineRenderer.numCapVertices = 5;
            LineRenderer.alignment = LineAlignment.TransformZ;
            LineRenderer.useWorldSpace = true;
            LineRenderer.SetPosition(0, lastDrawPoint);
            LineRenderer.SetPosition(1, endPosition);
        }
        else
        {
            if (LineRenderer != null)
            {
                LineRenderer.widthMultiplier = 1;
                LineRenderer.positionCount += 1;
                var curve = LineRenderer.widthCurve;
                curve.AddKey((LineRenderer.positionCount - 1) / 100, lineWidth);
                LineRenderer.widthCurve = curve;
                LineRenderer.SetPosition(LineRenderer.positionCount - 1, endPosition);
            }
        }
        workDrawPoint = endPosition;
        return endPosition;
    }

    public void ChangeColorDrawColorRPC(Color newColor)
    {
        ChangeDrawColor(newColor.r, newColor.g, newColor.b, newColor.a);
    }

    public void ChangeDrawColor(float r, float g, float b, float a)
    {
        Color color = new Color(r, g, b, a);
        DrawColor = color;
        meshRenderer.materials[materialColorToChangeIndex].color = color;
        if (meshRenderer.materials[materialColorToChangeIndex].GetColor("_Color") != null)
        {
            meshRenderer.materials[materialColorToChangeIndex].SetColor("_Color", color);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Show Grip Point
        Gizmos.color = Color.green;
        Gizmos.DrawLine(RaycastStart.position, RaycastStart.position + RaycastStart.up * RaycastLength);
    }

    void freezeLocalRotation()
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }
}

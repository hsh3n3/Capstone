using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractOutline : MonoBehaviour
{
    public Material outlineMat;
    GameObject child;
    public GameObject EmptyPrefabForSomeReason;
    private void OnEnable()
    {
        outlineMat = FindObjectOfType<RayInteract>().outlineMaterial;
        child = new GameObject();
        child.transform.parent = this.gameObject.transform;
        child.transform.localPosition = Vector3.zero;
        child.name = "outline";
        //get mesh
        child.AddComponent<MeshFilter>();
        child.GetComponent<MeshFilter>().mesh = this.GetComponent<MeshFilter>().mesh;
        //"Give me your face." - the immortal words of Optimus Prime
        child.AddComponent<MeshRenderer>();
        MeshRenderer mr = child.GetComponent<MeshRenderer>();
        mr.material = outlineMat;
        mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        mr.receiveShadows = false;

        child.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }
    void LateUpdate()
    {
        Ray cRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        if (!Physics.Raycast(cRay, out hit, 5f, 1 << 9, QueryTriggerInteraction.Ignore))
        {
            Destroy(child);
            Destroy(this);
        }
    }
}

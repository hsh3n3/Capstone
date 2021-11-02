using UnityEngine;
using System.Collections;

public class MoveTexture : MonoBehaviour {

	public Material mat;

    [System.Serializable]
    public class property
    {
        public string PropertyName;
        public Vector2 speed;
    }
    public property[] properties;

    void FixedUpdate () {
        foreach (property prop in properties)
        {
            string p = prop.PropertyName;

            mat.SetTextureOffset(p, new Vector2(
                ((mat.GetTextureOffset(p).x + (prop.speed.x * Time.deltaTime)) % mat.GetTextureScale(p).x),
                (mat.GetTextureOffset(p).y + (prop.speed.y * Time.deltaTime)) % mat.GetTextureScale(p).y));
        }
	}
}

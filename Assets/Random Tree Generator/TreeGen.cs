using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGen : MonoBehaviour
{
    public GameObject LeafPrimitive;
    new public Color light;
    public Color medium, dark;
    public float treeRadius;
    public int numberOfTreesGen;
    public float leafDensity;
    public float height;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < leafDensity * treeRadius * treeRadius; i++)
        {
            GameObject leafObject = Instantiate(LeafPrimitive, transform);
            leafObject.transform.localPosition = Random.insideUnitSphere * treeRadius;
            leafObject.transform.localScale = leafObject.transform.localScale * (1 + Random.value / 2);
            leafObject.GetComponent<SpriteRenderer>().color = dark;
        }
        for (int i = 0; i < leafDensity * treeRadius * treeRadius; i++)
        {
            GameObject leafObject = Instantiate(LeafPrimitive, transform);
            leafObject.transform.localPosition = Random.insideUnitSphere * treeRadius + Vector3.up * height / 2;
            leafObject.transform.localScale = leafObject.transform.localScale * (1 + Random.value / 2);
            leafObject.GetComponent<SpriteRenderer>().color = medium;
        }
        for (int i = 0; i < leafDensity * treeRadius * treeRadius; i++)
        {
            GameObject leafObject = Instantiate(LeafPrimitive, transform);
            leafObject.transform.localPosition = Random.insideUnitSphere * treeRadius + 2 * Vector3.up * height;
            leafObject.transform.localScale = leafObject.transform.localScale * (1 + Random.value / 2);
            leafObject.GetComponent<SpriteRenderer>().color = light;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenTree()
    {

    }
}

using UnityEngine;

public class Outline : MonoBehaviour
{
    public Material mat;
    public int indexOfMat = 1;

    private void Awake()
    {
        //access the renderer component of the material, and the outline matieral should be set at index 1
        mat = GetComponent<Renderer>().materials[indexOfMat];
    }

    public void OutlineBoolFunc(bool show)
    {
        //set float bool, show = 1 and don't show = 0
        mat.SetFloat("_ShowOutline", show ? 1 : 0);
    }
}

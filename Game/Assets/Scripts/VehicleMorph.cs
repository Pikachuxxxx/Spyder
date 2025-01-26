using UnityEngine;

public class VehicleMorph : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("RockAI")) {
            SkinnedMeshRenderer skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
            StartCoroutine(AnimateBlendShape(skinnedMeshRenderer));
        }
    }

    private System.Collections.IEnumerator AnimateBlendShape(SkinnedMeshRenderer skinnedMeshRenderer)
    {
        float blendValue = 0f;
        float currBlendValue = skinnedMeshRenderer.GetBlendShapeWeight(0);
        while (blendValue < 100f && currBlendValue < 100f)
        {
            blendValue += Time.deltaTime * 250f;
            skinnedMeshRenderer.SetBlendShapeWeight(0, blendValue);
            yield return null;
        }
    }
}

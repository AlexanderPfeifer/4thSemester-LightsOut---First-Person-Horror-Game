using UnityEngine;

public class Block : MonoBehaviour
{
    //When enters the destroy zone, this block is destroyed
    private void OnTriggerEnter(Collider col)
    {
        if ((1 << col.gameObject.layer) == FindObjectOfType<SparkleSpelunk>().deleteZone)
        {
            FindAnyObjectByType<SparkleSpelunk>().allBlocks.Remove(transform);
            Destroy(gameObject);
        }
    }
}

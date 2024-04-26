using UnityEngine;

public class Console : PlayerInputs
{
    private void Awake()
    {
        selectedObject = transform;
        var position = selectedObject.position;
        putAwayPosition =  new Vector3(position.x + 3, position.y - 3, position.z + 3);
        holdPosition = position;
    }

    public void Selected()
    {
        putAwayPosition = transform.position;
        consoleHoldVolume.weight = 1;
    }
}

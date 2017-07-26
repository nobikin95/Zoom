using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CameraController : MonoBehaviour {
    public List<Transform> targets;

    private void LateUpdate()
    {
        transform.position = new Vector3(
            targets.Average(
                (target) => target.transform.position.x
            ),
            targets.Average(
                (target) => target.transform.position.y
            ),
            transform.position.z
        );
    }

    /*
     * float Average(){
     *   float sum = 0;
     *   foreach(target in targets){
     *      sum += target.transform.position.x;
     *   }
     *   float average = sum/targets.Count;
     * }
     */
}

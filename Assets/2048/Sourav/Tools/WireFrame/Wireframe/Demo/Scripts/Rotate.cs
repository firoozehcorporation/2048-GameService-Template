using UnityEngine;
using System.Collections;

namespace Sourav.Utilities.WireFrame
{
    public class Rotate : MonoBehaviour
    {
        public float speed = -5;
        private void Update()
        {
            transform.rotation *= Quaternion.AngleAxis(speed * Mathf.PI * Time.deltaTime, Vector3.up);
        }
    }

}

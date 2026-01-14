using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Imagine.WebAR.Demo
{
    public class HotspotButtonDemo : MonoBehaviour
    {
        protected static HotspotButtonDemo activeHotspot;
        [SerializeField] Transform cam;
        [SerializeField] Transform container;

        Coroutine lerpRoutine;

        public void OnMouseDown()
        {
            Debug.Log("clicked " + name);
            if (activeHotspot != null)
            {
                activeHotspot.StopLerp();
            }
            activeHotspot = this;
            LerpToHotspot();
        }

        public void StopLerp()
        {
            StopCoroutine(lerpRoutine);
        }

        public void LerpToHotspot()
        {
            lerpRoutine = StartCoroutine(LerpToHotspotInternal());
        }
        
        IEnumerator LerpToHotspotInternal()
        {
            var deltaPos = transform.position - cam.transform.position;
            deltaPos.y = 0;
            var targetContainerPos = container.position - deltaPos;
            while (Vector3.Distance(cam.position, transform.position) > 0.01f)
            {
                //lerping
                var lerpSpeed = 5;
                container.position = Vector3.Lerp(container.position, targetContainerPos, Time.deltaTime * lerpSpeed);
                yield return new WaitForEndOfFrame();
            }
        }

    }
}

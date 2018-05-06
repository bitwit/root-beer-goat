 using UnityEngine;
 using System.Collections;
 
 public class BlinkingLight : MonoBehaviour
 {
 
     public float totalSeconds;
     public float maxIntensity;
     private Light myLight;
	 private bool isBlinkComplete = true;

	 void Start () {
		 myLight = GetComponent<Light>();
	 }

	 void Update() {
		 if (isBlinkComplete) {
		 	StartCoroutine(flashNow());
		 }
	 }
 
     public IEnumerator flashNow ()
     {
		 isBlinkComplete = false;
         
         while (myLight.intensity < maxIntensity) {
             myLight.intensity += 1;
             yield return null;
         }
         while (myLight.intensity > 0) {
             myLight.intensity -= 1;
             yield return null;
         }
		 isBlinkComplete = true;
         yield return null;
     }
 }
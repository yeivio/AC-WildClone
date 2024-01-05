using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Daylight_Manager : MonoBehaviour
{
    [SerializeField] private Vector3 dayStartRotation;
    [SerializeField] private Vector3 dayEndRotation;
    private Light lightComponent;
    [SerializeField] private float timer;
    [SerializeField] private int DURACION_DIA;
    [SerializeField] private int DURACION_NOCHE;

    private void Start()
    {
        lightComponent = GetComponent<Light>();
        lightComponent.intensity = 0;
        StartCoroutine(RotarSol());
    }

    private IEnumerator RotarSol()
    {
        while (true) { 
            this.gameObject.transform.rotation = Quaternion.Euler(dayStartRotation);
            timer = 0f;
            while (timer <= DURACION_DIA)
            {
                this.lightComponent.intensity = Mathf.Sin((timer * 360/ DURACION_DIA) * Mathf.PI/360);
                this.gameObject.transform.rotation = Quaternion.Lerp(Quaternion.Euler(dayStartRotation), Quaternion.Euler(dayEndRotation), timer/DURACION_DIA);
                timer += Time.deltaTime;
                yield return null;
            }
            this.gameObject.transform.rotation = Quaternion.Euler(dayEndRotation);
            this.lightComponent.intensity = 0;
            yield return new WaitForSeconds(DURACION_NOCHE);
        }
    }
}

using System;
using System.Collections;
using UnityEngine;

public class Daylight_Manager : MonoBehaviour
{

    public static Daylight_Manager current;

    [SerializeField] private Vector3 dayStartRotation;
    [SerializeField] private Vector3 dayEndRotation;
    private Light lightComponent;
    private float timer;
    [SerializeField] private int DURACION_DIA;
    [SerializeField] private int DURACION_NOCHE;

    public DateTime currentTime;

    private void Awake()
    {
        lightComponent = GetComponent<Light>();
        lightComponent.intensity = 0;
        StartCoroutine(RotarSol(0));
        this.currentTime = new DateTime(DateTime.Now.Year, 1, 1);
        current = this; // Patron Singleton
    }

    public void setTime(int hours)
    {
        if (hours >= 24 || hours < 0)
            hours = 0;
        StopAllCoroutines();
        StartCoroutine(RotarSol(hours* (DURACION_DIA + DURACION_NOCHE) / 24));
    }

    private IEnumerator RotarSol(float hour)
    {
        int calHoras;
        
        while (true) {
            this.timer = hour;
            this.gameObject.transform.rotation = Quaternion.Euler(dayStartRotation);
            while (timer <= DURACION_DIA)
            {
                this.lightComponent.intensity = Mathf.Sin((timer * 360 / DURACION_DIA) * Mathf.PI / 360);
                this.gameObject.transform.rotation = Quaternion.Lerp(Quaternion.Euler(dayStartRotation), Quaternion.Euler(dayEndRotation), timer / DURACION_DIA);

                timer += Time.deltaTime;
                calHoras = (int)Math.Floor(timer * 24 / (DURACION_DIA + DURACION_NOCHE));
                currentTime = new DateTime(1, 1, 1, calHoras, 0,0);
                yield return null;
            }
            this.gameObject.transform.rotation = Quaternion.Euler(dayEndRotation);
            this.lightComponent.intensity = 0;

            while (timer <= DURACION_DIA + DURACION_NOCHE)
            {
                timer += Time.deltaTime;
                calHoras = (int)Math.Floor(timer * 24 / (DURACION_DIA + DURACION_NOCHE));
                if (calHoras >= 24)
                    calHoras = 0;
                currentTime = new DateTime(1, 1, 1, calHoras, 0, 0);
                yield return null;
            }
        }
    }
}

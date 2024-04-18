using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicaPersonaje2 : MonoBehaviour
{
    public float velocidadMovimiento = 5.0f;
    public float velocidadRotacion = 200.0f;
    private Animator anim;

    // Definir los límites del área de movimiento
    public float areaRadius = 30f;
    public float tiempoDeCaminata = 5f; // Tiempo en segundos que el personaje caminará antes de detenerse
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private float tiempoCaminando;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        // Guardar la posición inicial del personaje
        initialPosition = transform.position;

        // Calcular una nueva posición de destino dentro del área
        CalculateNewTargetPosition();

        // Inicializar el tiempo caminando
        tiempoCaminando = 0f;
    }

    private void FixedUpdate()
    {
        // Movimiento hacia la posición de destino
        Vector3 direction = (targetPosition - transform.position).normalized;
        Vector3 movement = direction * velocidadMovimiento;
        transform.Translate(movement * Time.deltaTime, Space.World);

        // Rotación hacia la dirección del movimiento
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, velocidadRotacion * Time.deltaTime);

            // Activar la animación de caminar
            anim.SetBool("Caminando", true);

            // Actualizar el tiempo caminando
            tiempoCaminando += Time.deltaTime;

            // Si el tiempo caminando supera el tiempo de caminata, detener el movimiento
            if (tiempoCaminando >= tiempoDeCaminata)
            {
                StopWalking();
            }
        }
        else
        {
            // Desactivar la animación de caminar
            anim.SetBool("Caminando", false);
        }

        // Si el personaje está cerca de la posición de destino, calcular una nueva posición
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Calcular una nueva posición de destino aleatoria dentro del área
            CalculateNewTargetPosition();
        }
    }

    // Calcular una nueva posición de destino aleatoria dentro del área
    private void CalculateNewTargetPosition()
    {
        float randomAngle = Random.Range(0f, Mathf.PI * 2f);
        float randomX = Mathf.Sin(randomAngle) * areaRadius;
        float randomZ = Mathf.Cos(randomAngle) * areaRadius;
        targetPosition = initialPosition + new Vector3(randomX, 0f, randomZ);

        // Reiniciar el tiempo caminando
        tiempoCaminando = 0f;
    }

    // Detener el movimiento del personaje
    private void StopWalking()
    {
        tiempoCaminando = 0f; // Reiniciar el tiempo caminando
        anim.SetBool("Caminando", false); // Desactivar la animación de caminar
    }
}

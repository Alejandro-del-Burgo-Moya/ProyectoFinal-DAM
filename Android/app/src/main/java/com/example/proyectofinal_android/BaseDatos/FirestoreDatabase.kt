package com.example.proyectofinal_android.BaseDatos

import android.os.Build
import androidx.annotation.RequiresApi
import com.example.proyectofinal_android.Modelos.Incidencia
import com.example.proyectofinal_android.Modelos.Persona
import com.example.proyectofinal_android.Modelos.Estado
import com.example.proyectofinal_android.Modelos.Prioridad
import com.google.firebase.firestore.FirebaseFirestore
import java.time.LocalDate


class FirestoreDatabase {
    private val db: FirebaseFirestore = FirebaseFirestore.getInstance()
    private val coleccionIncidencias = "Incidencias"
    private val coleccionPersonas = "Personas"

    fun guardarIncidencia(incidencia: Incidencia) {
        val nuevoId = obtenerUltimoId(incidencia) + 1
        val dato = hashMapOf(
            "Nombre" to incidencia.nombre,
            "Descripcion" to incidencia.descripcion,
            "Prioridad" to incidencia.prioridad.toString(),
            "Estado" to incidencia.estado.toString(),
            "FechaCreacion" to incidencia.fechaCreacion.toString(),
            "FechaAsignacion" to incidencia.fechaAsignacion.toString(),
            "FechaResolucion" to incidencia.fechaResolucion.toString(),
            "CreadaPor" to incidencia.creadaPor.toString(),
            "AsignadaA" to incidencia.asignadaA.toString(),
            "ResueltaPor" to incidencia.resueltaPor.toString(),
        )
        db.collection(coleccionIncidencias).document(nuevoId.toString()).set(dato)
    }

    fun guardarPersona(persona: Persona) {
        val nuevoId = obtenerUltimoId(persona) + 1
        val dato = hashMapOf(
            "Nombre" to persona.nombre,
            "Contraseña" to persona.contrasena,
            "Rol" to persona.rol,
        )
        db.collection(coleccionIncidencias).document(nuevoId.toString()).set(dato)
    }

    @RequiresApi(Build.VERSION_CODES.O)
    fun obtenerIncidencias(): MutableList<Incidencia> {
        var listaIncidencias: MutableList<Incidencia> = arrayListOf()
        var incidencia: Incidencia
        db.collection(coleccionIncidencias).get().addOnSuccessListener { documentos ->
            for (doc in documentos) {
                incidencia = Incidencia(
                    doc.get("Nombre").toString(),
                    doc.get("Descripcion").toString(),

                    when (doc.get("Prioridad")) {
                        "Alta" -> Prioridad.Alta
                        "Media" -> Prioridad.Media
                        else -> Prioridad.Baja
                    },

                    when (doc.get("Estado")) {
                        "Abierta" -> Estado.Abierta
                        "Asignada" -> Estado.Asignada
                        "En progreso" -> Estado.En_progreso
                        else -> Estado.Resuelta
                    },

                    LocalDate.now(),
                    LocalDate.now(),
                    LocalDate.now(),
//                    doc.get("FechaCreacion"),
//                    doc.get("FechaAsignacion"),
//                    doc.get("FechaResolucion"),

                    Persona(doc.get("CreadaPor").toString()),
                    Persona(doc.get("AsigandaA").toString()),
                    Persona(doc.get("ResueltaPor").toString()),
                )
                listaIncidencias.add(incidencia)
            }
        }
        return listaIncidencias
    }

    private fun obtenerUltimoId(objeto: Any): Int {
        var ultimoId = "-1"
        when (objeto) {
            is Incidencia ->
                db.collection(coleccionIncidencias).get()
                    .addOnSuccessListener { documentos ->
                        ultimoId = documentos.last().id
                    }
                    .addOnFailureListener { _ ->
                        throw Exception()
                    }

            is Persona ->
                db.collection(coleccionPersonas).get()
                    .addOnSuccessListener { documentos ->
                        ultimoId = documentos.last().id
                    }
                    .addOnFailureListener { _ ->
                        throw Exception()
                    }
        }

        if (ultimoId != "-1")
            return ultimoId.toInt()
        else
            return 0
        //throw ErrorObtenerUltimoIdExcepcion()
    }

}
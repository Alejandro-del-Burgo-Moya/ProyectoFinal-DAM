package com.example.proyectofinal_android.Modelos

import android.os.Build
import androidx.annotation.RequiresApi
import java.time.LocalDate

class Incidencia() {
    private lateinit var _nombre: String
    private lateinit var _descripcion: String
    private lateinit var _prioridad: Prioridad
    private lateinit var _estado: Estado
    private lateinit var _fechaCreacion: LocalDate
    private lateinit var _fechaAsignacion: LocalDate
    private lateinit var _fechaResolucion: LocalDate
    private lateinit var _creadaPor: Persona
    private lateinit var _asignadaA: Persona
    private lateinit var _resueltaPor: Persona
    var nombre: String
        get() = _nombre
        set(value) {
            _nombre = value
        }
    var descripcion: String
        get() = _descripcion
        set(value) {
            _descripcion = value
        }
    var prioridad: Prioridad
        get() = _prioridad
        set(value) {
            _prioridad = value
        }
    var estado: Estado
        get() = _estado
        set(value) {
            _estado = value
        }
    var fechaCreacion: LocalDate
        get() = _fechaCreacion
        set(value) {
            _fechaCreacion = value
        }
    var fechaAsignacion: LocalDate
        get() = _fechaAsignacion
        set(value) {
            _fechaAsignacion = value
        }
    var fechaResolucion: LocalDate
        get() = _fechaResolucion
        set(value) {
            _fechaResolucion = value
        }
    var creadaPor: Persona
        get() = _creadaPor
        set(value) {
            _creadaPor = value
        }
    var asignadaA: Persona
        get() = _asignadaA
        set(value) {
            _asignadaA = value
        }
    var resueltaPor: Persona
        get() = _resueltaPor
        set(value) {
            _resueltaPor = value
        }

    constructor(
        nombre: String, descripcion: String,
        prioridad: Prioridad, estado: Estado,
        fechaCreacion: LocalDate, fechaAsignacion: LocalDate, fechaResolucion: LocalDate,
        creadaPor: Persona, asignadaA: Persona, resultaPor: Persona
    ) : this() {
        this.nombre = nombre
        this.descripcion = descripcion
        this.prioridad = prioridad
        this.estado = estado
        this.fechaCreacion = fechaCreacion
        this.fechaAsignacion = fechaAsignacion
        this.fechaResolucion = fechaResolucion
        this.creadaPor = creadaPor
        this.asignadaA = asignadaA
        this.resueltaPor = resultaPor
    }

    @RequiresApi(Build.VERSION_CODES.O)
    fun crearIncidencia(
        persona: Persona, n: String, d: String, p: Prioridad, es: Estado
    ) {
        creadaPor = persona
        fechaCreacion = LocalDate.now()

        nombre = n
        descripcion = d
        prioridad = p
        estado = es
    }

    @RequiresApi(Build.VERSION_CODES.O)
    fun asignarIncidencia(persona: Persona) {
        asignadaA = persona
        fechaAsignacion = LocalDate.now()
    }

    @RequiresApi(Build.VERSION_CODES.O)
    fun resolverIncidencia(persona: Persona) {
        resueltaPor = persona
        fechaResolucion = LocalDate.now()
    }

    fun reasignarTecnico(persona: Persona) {
        asignadaA = persona
    }

    fun reasignarEstado(e: Estado) {
        estado = e
    }

    fun reasignarPrioridad(p: Prioridad) {
        prioridad = p
    }

    fun actualizarDescripcion(d: String) {
        descripcion = d
    }
}
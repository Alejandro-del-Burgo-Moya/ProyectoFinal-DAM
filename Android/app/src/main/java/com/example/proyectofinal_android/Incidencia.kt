package com.example.proyectofinal_android

import java.time.LocalDate

class Incidencia() {
    private lateinit var _nombre: String
    private lateinit var _descripcion: String
    private lateinit var _elemento: Elemento
    private lateinit var _tipo: Tipo
    private lateinit var _prioridad: Prioridad
    private lateinit var _estado: Estado
    private lateinit var _fechaCreacion: LocalDate
    private lateinit var _fechaAsignacion: LocalDate
    private lateinit var _fechaResolucion: LocalDate
    private lateinit var _creadaPor: Persona
    private lateinit var _asignadaA: Persona
    private lateinit var _resueltaPor: Persona
    private var nombre: String
        get() = _nombre
        set(value) {
            _nombre = value
        }
    private var descripcion: String
        get() = _descripcion
        set(value) {
            _descripcion = value
        }
    private var elemento: Elemento
        get() = _elemento
        set(value) {
            _elemento = value
        }
    private var tipo: Tipo
        get() = _tipo
        set(value) {
            _tipo = value
        }
    private var prioridad: Prioridad
        get() = _prioridad
        set(value) {
            _prioridad = value
        }
    private var estado: Estado
        get() = _estado
        set(value) {
            _estado = value
        }
    private var fechaCreacion: LocalDate
        get() = _fechaCreacion
        set(value) {
            _fechaCreacion = value
        }
    private var fechaAsignacion: LocalDate
        get() = _fechaAsignacion
        set(value) {
            _fechaAsignacion = value
        }
    private var fechaResolucion: LocalDate
        get() = _fechaResolucion
        set(value) {
            _fechaResolucion = value
        }
    private var creadaPor: Persona
        get() = _creadaPor
        set(value) {
            _creadaPor = value
        }
    private var asignadaA: Persona
        get() = _asignadaA
        set(value) {
            _asignadaA = value
        }
    private var resueltaPor: Persona
        get() = _resueltaPor
        set(value) {
            _resueltaPor = value
        }

    constructor(
        nombre: String, descripcion: String,
        elemento: Elemento, tipo: Tipo,
        prioridad: Prioridad, estado: Estado,
        fechaCreacion: LocalDate, fechaAsignacion: LocalDate, fechaResolucion: LocalDate,
        creadaPor: Persona, asignadaA: Persona, resultaPor: Persona
    ) : this() {
        this.nombre = nombre
        this.descripcion = descripcion
        this.elemento = elemento
        this.tipo = tipo
        this.prioridad = prioridad
        this.estado = estado
        this.fechaCreacion = fechaCreacion
        this.fechaAsignacion = fechaAsignacion
        this.fechaResolucion = fechaResolucion
        this.creadaPor = creadaPor
        this.asignadaA = asignadaA
        this.resueltaPor = resultaPor
    }

    fun crearIncidencia(
        persona: Persona, n: String, d: String, el: Elemento, t: Tipo, p: Prioridad, es: Estado
    ) {
        creadaPor = persona
        fechaCreacion = LocalDate.now()

        nombre = n
        descripcion = d
        elemento = el
        tipo = t
        prioridad = p
        estado = es
    }

    fun asignarIncidencia(persona: Persona) {
        asignadaA = persona
        fechaAsignacion = LocalDate.now()
    }

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
package com.example.proyectofinal_android.Modelos

/**
 * Representa un usuario de la aplicación
 */
//0-admin 1-tecnico 2-normal
class Persona() {
    private lateinit var _nombre: String
    private lateinit var _contrasena: String
    private lateinit var _rol: Rol

    var nombre: String
        get() = _nombre
        set(value) {
            _nombre = value
        }
    var contrasena: String
        get() = _contrasena
        set(value) {
            _contrasena = value
        }
    var rol: Rol
        get() = _rol
        set(value) {
            _rol = value
        }

    constructor(nombre: String) : this() {
        this.nombre = nombre
    }

    constructor(nombre: String, contrasena: String, rol: Rol) : this() {
        this.nombre = nombre
        this.contrasena = contrasena
        this.rol = rol
    }

    override fun toString(): String {
        return nombre
    }
}
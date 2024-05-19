package com.example.proyectofinal_android

import android.annotation.SuppressLint
import android.os.Build
import android.os.Bundle
import android.view.MenuItem
import android.widget.ImageButton
import android.widget.Toast
import androidx.annotation.RequiresApi
import androidx.appcompat.app.ActionBarDrawerToggle
import androidx.appcompat.app.AppCompatActivity
import androidx.appcompat.widget.Toolbar
import androidx.core.view.GravityCompat
import androidx.drawerlayout.widget.DrawerLayout
import com.example.proyectofinal_android.BaseDatos.FirestoreDatabase
import com.example.proyectofinal_android.Modelos.Estado
import com.example.proyectofinal_android.Modelos.Incidencia
import com.example.proyectofinal_android.Modelos.Persona
import com.example.proyectofinal_android.Modelos.Prioridad
import com.example.proyectofinal_android.Nav_Fragment.FragmentAcercaDe
import com.example.proyectofinal_android.Nav_Fragment.FragmentAdmin
import com.example.proyectofinal_android.Nav_Fragment.FragmentIncidencias
import com.example.proyectofinal_android.Nav_Fragment.FragmentInicio
import com.example.proyectofinal_android.Nav_Fragment.FragmentMisIncidencias
import com.example.proyectofinal_android.Nav_Fragment.FragmentTiposIncidencia
import com.google.android.material.floatingactionbutton.FloatingActionButton
import com.google.android.material.navigation.NavigationView
import java.time.LocalDate

//TODO: enlaces
// https://www.youtube.com/watch?v=6mgTJdy_di4
// https://androidknowledge.com/navigation-drawer-android-studio/

class MainActivity : AppCompatActivity(), NavigationView.OnNavigationItemSelectedListener {

    private lateinit var drawerLayout: DrawerLayout
    private lateinit var btnAgregarIncidencia: ImageButton

    @SuppressLint("MissingInflatedId")
    @RequiresApi(Build.VERSION_CODES.O)
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        val toolbar = findViewById<Toolbar>(R.id.toolbar)
        setSupportActionBar(toolbar)

        drawerLayout = findViewById(R.id.drawer_layout)
        val navigationView: NavigationView = findViewById(R.id.nav_view)
        navigationView.setNavigationItemSelectedListener(this)

        var db = FirestoreDatabase()
        guardar(db)
        var lista = db.obtenerIncidencias()

        val toggle = ActionBarDrawerToggle(
            this,
            drawerLayout,
            toolbar,
            R.string.abrir_nav,
            R.string.cerrar_nav
        )
        drawerLayout.addDrawerListener(toggle)
        toggle.syncState()

        if (savedInstanceState == null) {
            supportFragmentManager.beginTransaction()
                .replace(R.id.fragment_container, FragmentInicio())
                .commit()
            navigationView.setCheckedItem(R.id.nav_incidencias)
        }

        btnAgregarIncidencia = findViewById<ImageButton>(R.id.btnAgregarIncidencia)
    }

    override fun onNavigationItemSelected(item: MenuItem): Boolean {
        when (item.itemId) {
            R.id.nav_incidencias -> {
                supportFragmentManager.beginTransaction()
                    .replace(R.id.fragment_container, FragmentIncidencias()).commit()
                btnAgregarIncidencia.setOnClickListener { prueba() }
            }

            R.id.nav_mis_incidencias -> supportFragmentManager.beginTransaction()
                .replace(R.id.fragment_container, FragmentMisIncidencias()).commit()

            R.id.nav_tipos -> supportFragmentManager.beginTransaction()
                .replace(R.id.fragment_container, FragmentTiposIncidencia()).commit()

            R.id.nav_admin -> supportFragmentManager.beginTransaction()
                .replace(R.id.fragment_container, FragmentAdmin()).commit()

            R.id.nav_acerca_de -> supportFragmentManager.beginTransaction()
                .replace(R.id.fragment_container, FragmentAcercaDe()).commit()

            R.id.nav_cerrar_sesion -> {
                supportFragmentManager.beginTransaction()
                    .replace(R.id.fragment_container, FragmentInicio()).commit()
                Toast.makeText(this, "Cerrando sesión", Toast.LENGTH_SHORT)
                    .show()
            }
        }
        drawerLayout.closeDrawer(GravityCompat.START)
        return true
    }


    @RequiresApi(Build.VERSION_CODES.O)
    private fun guardar(db: FirestoreDatabase) {
        db.guardarIncidencia(
            Incidencia(
                "nombre1",
                "desc1",
                Prioridad.Baja,
                Estado.Abierta,
                LocalDate.now(),
                LocalDate.now(),
                LocalDate.now(),
                Persona("p1"),
                Persona("p1"),
                Persona("p1")
            )
        )
        db.guardarIncidencia(
            Incidencia(
                "nombre2222",
                "desc2",
                Prioridad.Baja,
                Estado.Abierta,
                LocalDate.now(),
                LocalDate.now(),
                LocalDate.now(),
                Persona("p2"),
                Persona("p3"),
                Persona("p4")
            )
        )
    }

    private fun prueba() {
        Toast.makeText(this, "prueba", Toast.LENGTH_SHORT).show()
    }

}
package com.example.proyectofinal_android

import android.os.Bundle
import android.view.MenuItem
import android.widget.Toast
import androidx.appcompat.app.ActionBarDrawerToggle
import androidx.appcompat.app.AppCompatActivity
import androidx.appcompat.widget.Toolbar
import androidx.core.view.GravityCompat
import androidx.drawerlayout.widget.DrawerLayout
import com.example.proyectofinal_android.Nav_Fragment.FragmentAcercaDe
import com.example.proyectofinal_android.Nav_Fragment.FragmentAdmin
import com.example.proyectofinal_android.Nav_Fragment.FragmentIncidencias
import com.example.proyectofinal_android.Nav_Fragment.FragmentInicio
import com.example.proyectofinal_android.Nav_Fragment.FragmentMisIncidencias
import com.example.proyectofinal_android.Nav_Fragment.FragmentTiposIncidencia
import com.google.android.material.navigation.NavigationView

//TODO: enlaces
// https://www.youtube.com/watch?v=6mgTJdy_di4
// https://androidknowledge.com/navigation-drawer-android-studio/

class MainActivity : AppCompatActivity(), NavigationView.OnNavigationItemSelectedListener {

    private lateinit var drawerLayout: DrawerLayout

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        val toolbar = findViewById<Toolbar>(R.id.toolbar)
        setSupportActionBar(toolbar)

        drawerLayout = findViewById(R.id.drawer_layout)
        val navigationView: NavigationView = findViewById(R.id.nav_view)
        navigationView.setNavigationItemSelectedListener(this)

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
    }

    override fun onNavigationItemSelected(item: MenuItem): Boolean {
        when (item.itemId) {
            R.id.nav_incidencias -> supportFragmentManager.beginTransaction()
                .replace(R.id.fragment_container, FragmentIncidencias()).commit()

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

}
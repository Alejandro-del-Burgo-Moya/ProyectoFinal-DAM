package com.example.proyectofinal_android.Nav_Fragment

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.lifecycle.findViewTreeViewModelStoreOwner
import com.example.proyectofinal_android.R
import com.google.android.material.floatingactionbutton.FloatingActionButton

class FragmentIncidencias() : Fragment() {
    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        return inflater.inflate(R.layout.fragment_incidencias, container, false)
    }
}
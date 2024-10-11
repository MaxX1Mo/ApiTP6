Para probar la api, use un celular fisico android, en net maui.
Para que esto funcione necesite instalar devtunnel (túneles de desarrollo en Visual Studio)
La configuracion que necesite fue crear un tunel desde visual studio y ponerlo publico. 
En las salidas del proyecto de la api, busque el tipo de salida dev tunnel y busque y copie la url que me genero: ===
===>    Salida
        Mostrar salida de: Dev Tunnel
        Se configuraron correctamente las siguientes direcciones URL en el túnel de desarrollo "TunelApi": 
             http://localhost:5280  ->  https://fvgzx1gd-5280.brs.devtunnels.ms/    
                                              esta es la url que necesito
entonces me quedaria para el endpoint login por ejemplo  https://fvgzx1gd-5280.brs.devtunnels.ms/api/Login/acceso

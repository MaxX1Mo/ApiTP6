Para probar la api, use un celular fisico android, en net maui.

Para que esto funcione necesite instalar devtunnel (túneles de desarrollo en Visual Studio)

La configuracion que necesite fue crear un tunel desde visual studio, ponerlo publico y como tipo de tunel persistente para que no cambie el url. 

En las salidas del proyecto de la api, busque el tipo de salida dev tunnel y busque y copie la url que me genero: 


        Salida
        Mostrar salida de: Dev Tunnel
        Se configuraron correctamente las siguientes direcciones URL en el túnel de desarrollo "TunelApi": 
             http://localhost:5280  -> https://9s77np9h-5280.brs.devtunnels.ms/ 
                                              esta es la url que necesito

Entonces me quedaria para el endpoint login por ejemplo  https://9s77np9h-5280.brs.devtunnels.ms/api/Login/acceso

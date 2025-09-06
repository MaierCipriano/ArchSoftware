EXPLICACIÓN DE CADA PRINCIPIO SOLID:

1.	SINGLE RESPONSIBILITY (SRP):
⦁	Libro: solo maneja datos del libro
⦁	Usuario: solo maneja datos del usuario
⦁	Prestamo: solo maneja datos del préstamo
2.	OPEN/CLOSED (OCP):
⦁	CalculadoraMulta es extensible (podemos agregar MultaVIP)
⦁	Sin modificar el código existente
3.	LISKOV SUBSTITUTION (LSP):
⦁	NotificadorEmail y NotificadorSMS pueden intercambiarse
⦁	Ambos cumplen el contrato de Notificador
4.	INTERFACE SEGREGATION (ISP):
⦁	Interfaces pequeñas y específicas
⦁	Cada clase implementa solo lo que necesita
5.	DEPENDENCY INVERSION (DIP):
⦁	GestorPrestamos depende de abstracciones
⦁	No de implementaciones concretas
⦁	Fácil intercambiar comportamientos

Ejecución
Crear una carpeta y abrir una terminal y ejecutar lo siguiente:
git clone https://github.com/MaierCipriano/ArchSoftware.git
cd ArchSoftware
cd solid-c#
dotnet restore
dotnet run

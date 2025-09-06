// SISTEMA DE BIBLIOTECA - EJEMPLOS PRINCIPIOS SOLID
// ================================================

using System;
using System.Collections.Generic;

// ========================================
// 1. SINGLE RESPONSIBILITY PRINCIPLE (SRP)
// ========================================
// Una clase debe tener una sola responsabilidad

public class Libro
{
    public string Titulo { get; }
    public string Autor { get; }
    public string Isbn { get; }
    public bool Disponible { get; set; }

    public Libro(string titulo, string autor, string isbn)
    {
        Titulo = titulo;
        Autor = autor;
        Isbn = isbn;
        Disponible = true;
    
    }
}

public class Usuario
{
    public string Nombre { get; }
    public string IdUsuario { get; }

    public Usuario(string nombre, string idUsuario)
    {
        Nombre = nombre;
        IdUsuario = idUsuario;
    }
}

public class Prestamo
{
    public Libro Libro { get; }
    public Usuario Usuario { get; }
    public DateTime FechaPrestamo { get; }
    public DateTime FechaDevolucion { get; set; } // <-- Cambiado a set público
    public bool Devuelto { get; set; }

    public Prestamo(Libro libro, Usuario usuario)
    {
        Libro = libro;
        Usuario = usuario;
        FechaPrestamo = DateTime.Now;
        FechaDevolucion = DateTime.Now.AddDays(14);
        Devuelto = false;
    }
}

// ========================================
// 2. OPEN/CLOSED PRINCIPLE (OCP)
// ========================================
// Abierto para extensión, cerrado para modificación

public abstract class CalculadoraMulta
{
    public abstract int Calcular(int diasRetraso);
}

public class MultaEstandar : CalculadoraMulta
{
    public override int Calcular(int diasRetraso)
    {
        return diasRetraso * 10;
    }
}

public class MultaEstudiante : CalculadoraMulta
{
    public override int Calcular(int diasRetraso)
    {
        return diasRetraso * 5;
    }
}

public class MultaVIP : CalculadoraMulta
{
    public override int Calcular(int diasRetraso)
    {
        return 0;
    }
}

// ========================================
// 3. LISKOV SUBSTITUTION PRINCIPLE (LSP)
// ========================================
// Los objetos derivados deben poder sustituir a la clase base

public abstract class Notificador
{
    public abstract bool Enviar(string mensaje, string destinatario);
}

public class NotificadorEmail : Notificador
{
    public override bool Enviar(string mensaje, string destinatario)
    {
        Console.WriteLine($"📧 Email a {destinatario}: {mensaje}");
        return true;
    }
}

public class NotificadorSMS : Notificador
{
    public override bool Enviar(string mensaje, string destinatario)
    {
        Console.WriteLine($"📱 SMS a {destinatario}: {mensaje}");
        return true;
    }
}

// ========================================
// 4. INTERFACE SEGREGATION PRINCIPLE (ISP)
// ========================================
// Los clientes no deben depender de interfaces que no usan

public interface IReservable
{
    void Reservar(Usuario usuario);
}

public interface IPrestable
{
    void Prestar(Usuario usuario);
}

public interface IRenovable
{
    void Renovar(Prestamo prestamo);
}

// ========================================
// 5. DEPENDENCY INVERSION PRINCIPLE (DIP)
// ========================================
// Depender de abstracciones, no de implementaciones concretas

public class GestorPrestamos
{
    private readonly CalculadoraMulta _calculadoraMulta;
    private readonly Notificador _notificador;
    private readonly List<Prestamo> _prestamos;

    public GestorPrestamos(CalculadoraMulta calculadoraMulta, Notificador notificador)
    {
        _calculadoraMulta = calculadoraMulta;
        _notificador = notificador;
        _prestamos = new List<Prestamo>();
    }

    public Prestamo RealizarPrestamo(Libro libro, Usuario usuario)
    {
        if (!libro.Disponible)
        {
            Console.WriteLine($"❌ El libro '{libro.Titulo}' no está disponible");
            return null;
        }

        libro.Disponible = false;
        var prestamo = new Prestamo(libro, usuario);
        _prestamos.Add(prestamo);

        string mensaje = $"Has prestado '{libro.Titulo}'. Fecha de devolución: {prestamo.FechaDevolucion:yyyy-MM-dd}";
        _notificador.Enviar(mensaje, usuario.Nombre);

        Console.WriteLine($"✅ Préstamo realizado: '{libro.Titulo}' para {usuario.Nombre}");
        return prestamo;
    }

    public void DevolverLibro(Prestamo prestamo)
    {
        if (prestamo.Devuelto)
        {
            Console.WriteLine("❌ Este libro ya fue devuelto");
            return;
        }

        DateTime fechaActual = DateTime.Now;
        if (fechaActual > prestamo.FechaDevolucion)
        {
            int diasRetraso = (fechaActual - prestamo.FechaDevolucion).Days;
            int multa = _calculadoraMulta.Calcular(diasRetraso);
            Console.WriteLine($"⚠️  Retraso de {diasRetraso} días. Multa: ${multa}");
        }
        else
        {
            Console.WriteLine("✅ Libro devuelto a tiempo");
        }

        prestamo.Devuelto = true;
        prestamo.Libro.Disponible = true;
        Console.WriteLine($"📚 '{prestamo.Libro.Titulo}' devuelto por {prestamo.Usuario.Nombre}");
    }
}

// ========================================
// EJEMPLO DE USO
// ========================================

public class Program
{
    public static void Main()
    {
        Console.WriteLine("🏛️  SISTEMA DE BIBLIOTECA - PRINCIPIOS SOLID");
        Console.WriteLine(new string('=', 50));

        // Crear libros
        var libro1 = new Libro("1984", "George Orwell", "978-0-452-28423-4");
        var libro2 = new Libro("Cien años de soledad", "Gabriel García Márquez", "978-84-376-0494-7");

        // Crear usuarios
        var usuario1 = new Usuario("Ana García", "U001");
        var usuario2 = new Usuario("Carlos López", "U002");

        // Crear diferentes gestores con distintas configuraciones
        Console.WriteLine("\n📋 GESTOR PARA USUARIOS REGULARES:");
        var gestorRegular = new GestorPrestamos(
            new MultaEstandar(),
            new NotificadorEmail()
        );

        Console.WriteLine("\n📋 GESTOR PARA ESTUDIANTES:");
        var gestorEstudiante = new GestorPrestamos(
            new MultaEstudiante(),
            new NotificadorSMS()
        );

        // Realizar préstamos
        Console.WriteLine("\n🔄 REALIZANDO PRÉSTAMOS:");
        var prestamo1 = gestorRegular.RealizarPrestamo(libro1, usuario1);
        var prestamo2 = gestorEstudiante.RealizarPrestamo(libro2, usuario2);

        // Simular devolución tardía
        Console.WriteLine("\n📅 SIMULANDO DEVOLUCIÓN TARDÍA:");
        if (prestamo1 != null)
        {
            prestamo1.FechaDevolucion = DateTime.Now.AddDays(-3);
            gestorRegular.DevolverLibro(prestamo1);
        }

        // Devolución a tiempo
        Console.WriteLine("\n📅 DEVOLUCIÓN A TIEMPO:");
        if (prestamo2 != null)
        {
            gestorEstudiante.DevolverLibro(prestamo2);
        }
    }
}
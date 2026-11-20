-- Crear la base de datos si no existe
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'CarteleraDB')
BEGIN
    CREATE DATABASE CarteleraDB;
END
GO

-- Usar la base de datos recién creada
USE CarteleraDB;
GO

-- Crear la tabla Funciones si no existe
IF OBJECT_ID('dbo.Funciones', 'U') IS NULL
BEGIN
    CREATE TABLE Funciones (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Pelicula NVARCHAR(100) NOT NULL,
        Sala NVARCHAR(50) NOT NULL,
        Precio DECIMAL(10,2) NOT NULL
    );
END
GO

-- Insertar los dos registros iniciales solicitados
INSERT INTO Funciones (Pelicula, Sala, Precio)
VALUES 
    ('Coco', 'Sala 1', 40.00),
    ('Moana', 'Sala 2', 45.00);
GO
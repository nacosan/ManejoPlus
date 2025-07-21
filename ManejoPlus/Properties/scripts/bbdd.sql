-- ⚙ Crear base de datos
CREATE DATABASE ManejoPlus;
GO

USE ManejoPlus;
GO

-- 1. Tabla Plataformas
CREATE TABLE Plataformas (
    PlataformaID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255),
    EsPersonalizada BIT NOT NULL
);

-- 2. Tabla Planes
CREATE TABLE Planes (
    PlanID INT IDENTITY(1,1) PRIMARY KEY,
    PlataformaID INT NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    Precio DECIMAL(10, 2) NOT NULL,
    Periodicidad NVARCHAR(50),
    Descripcion NVARCHAR(255),
    FOREIGN KEY (PlataformaID) REFERENCES Plataformas(PlataformaID)
);

-- 3. Tabla Suscripciones
CREATE TABLE Suscripciones (
    SubscriptionID INT IDENTITY(1,1) PRIMARY KEY,
    PlataformaID INT NOT NULL,
    PlanID INT NOT NULL,
    ApplicationUserId NVARCHAR(450) NOT NULL,
    NombrePersonalizado NVARCHAR(100),
    Tipo NVARCHAR(50),
    Descripcion NVARCHAR(255),
    FechaInicio DATE NOT NULL,
    FechaFin DATE NULL,
    Estado NVARCHAR(50),
    FOREIGN KEY (PlataformaID) REFERENCES Plataformas(PlataformaID),
    FOREIGN KEY (PlanID) REFERENCES Planes(PlanID)
    -- UserID no tiene FK formal a AspNetUsers por simplicidad en desarrollo
);

-- 4. Tabla Miembros
CREATE TABLE Miembros (
    MiembroID INT IDENTITY(1,1) PRIMARY KEY,
    SubscriptionID INT NOT NULL,
    NombreMiembro NVARCHAR(100) NOT NULL,
    EmailOpcional NVARCHAR(100),
    Rol NVARCHAR(50) NOT NULL,
    MontoAportado DECIMAL(10,2),
    ApplicationUserId NVARCHAR(450),
    FOREIGN KEY (SubscriptionID) REFERENCES Suscripciones(SubscriptionID)
);

-- 5. Tabla HistorialPagos
CREATE TABLE HistorialPagos (
    PagoID INT IDENTITY(1,1) PRIMARY KEY,
    SubscriptionID INT NOT NULL,
    FechaPago DATE NOT NULL,
    Monto DECIMAL(10,2) NOT NULL,
    Detalle NVARCHAR(255),
    FOREIGN KEY (SubscriptionID) REFERENCES Suscripciones(SubscriptionID)
);
GO


-- Insertar Plataformas
INSERT INTO Plataformas (Nombre, Descripcion, EsPersonalizada)
VALUES 
('Netflix', 'Streaming de películas y series', 0),
('Spotify', 'Música en streaming', 0),
('Disney+', 'Contenido exclusivo de Disney', 0),
('CustomTV', 'Plataforma personalizada', 1);

-- Insertar Planes
INSERT INTO Planes (PlataformaID, Nombre, Precio, Periodicidad, Descripcion)
VALUES 
(1, 'Básico', 7.99, 'mensual', '1 pantalla'),
(1, 'Estándar', 11.99, 'mensual', 'HD y 2 pantallas'),
(1, 'Premium', 15.99, 'mensual', '4 pantallas UHD'),
(2, 'Individual', 9.99, 'mensual', 'Cuenta individual'),
(2, 'Familiar', 14.99, 'mensual', 'Hasta 6 cuentas'),
(3, 'Anual', 89.99, 'anual', 'Pago único por 12 meses'),
(4, 'Plan flexible', 5.00, 'mensual', 'Definido por el usuario');

-- Insertar Suscripciones basadas en los planes anteriores
INSERT INTO Suscripciones 
(PlataformaID, PlanID, ApplicationUserId, NombrePersonalizado, Tipo, Descripcion, FechaInicio, FechaFin, Estado)
VALUES 
(1, 3, '88f7cab4-824a-48b4-896d-752d78dc7ad9', 'Netflix Premium', 'compartida', 'Compartida con amigos', '2025-01-01', NULL, 'activa'),
(2, 4, '88f7cab4-824a-48b4-896d-752d78dc7ad9', 'Spotify Personal', 'individual', 'Uso personal', '2025-02-01', NULL, 'activa'),
(3, 6, '88f7cab4-824a-48b4-896d-752d78dc7ad9', 'Disney Año completo', 'individual', 'Prueba anual', '2025-03-01', NULL, 'activa'),
(4, 7, '88f7cab4-824a-48b4-896d-752d78dc7ad9', 'CustomTV Prueba', 'individual', 'Plan personalizado', '2025-04-01', NULL, 'activa');

-- Insertar Miembros (solo 1 con datos completos, los demás con valores null)
INSERT INTO Miembros (
    SubscriptionID, NombreMiembro, EmailOpcional, Rol, MontoAportado, ApplicationUserId)
VALUES
(1, 'Juan Martín', 'juan@correo.com', 'admin', 15.99, '88f7cab4-824a-48b4-896d-752d78dc7ad9'),
(1, 'Participante 2', NULL, 'participante', NULL, NULL),
(2, 'Participante 3', NULL, 'admin', NULL, NULL),
(3, 'Participante 4', NULL, 'admin', NULL, NULL),
(4, 'Participante 5', NULL, 'admin', NULL, NULL);

-- Insertar Historial de Pagos
INSERT INTO HistorialPagos (SubscriptionID, FechaPago, Monto, Detalle)
VALUES
(1, '2025-01-01', 15.99, 'Pago inicial Netflix'),
(2, '2025-02-01', 9.99, 'Spotify activado'),
(3, '2025-03-01', 89.99, 'Pago Disney por 12 meses'),
(4, '2025-04-01', 5.00, 'Pago CustomTV 1er mes');

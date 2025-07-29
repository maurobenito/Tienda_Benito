-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 29-07-2025 a las 15:35:00
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `proyecto_tienda2`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `cliente`
--

CREATE TABLE `cliente` (
  `ClienteId` int(11) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `Apellido` varchar(100) NOT NULL,
  `Email` varchar(150) DEFAULT NULL,
  `Telefono` varchar(50) DEFAULT NULL,
  `Direccion` varchar(255) DEFAULT NULL,
  `CondFiscal` varchar(50) DEFAULT NULL,
  `CUIT` varchar(50) DEFAULT NULL,
  `UsuarioId` int(11) DEFAULT NULL,
  `FotoPerfil` varchar(255) DEFAULT NULL,
  `ArchivoAdjunto` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `cliente`
--

INSERT INTO `cliente` (`ClienteId`, `Nombre`, `Apellido`, `Email`, `Telefono`, `Direccion`, `CondFiscal`, `CUIT`, `UsuarioId`, `FotoPerfil`, `ArchivoAdjunto`) VALUES
(1, 'María', 'López', 'emp@g.com', '1111-2222', 'asd 45', 'Responsable Inscripto', '30-587465216-5', NULL, NULL, NULL),
(2, 'Carlos', 'López', 'emp@g.com', '1111-2222', 'asd 45', 'Responsable Inscripto', '30-58456987-9', NULL, NULL, NULL),
(3, 'sancho', 'panza', 'sancho@g.com', '26648578459', 'saavedra 500', 'responsable inscripto', '30-58456987-1', NULL, 'c2784d28-0c92-4728-abd0-d616c9c51a33.jpg', 'a34de05e-1323-4762-8624-b2e6d0b893f1.png'),
(4, 'soledad', 'paez', 'sp@g.com', '|2664879854', 'las heras 45', 'consumidor final', NULL, NULL, NULL, NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `producto`
--

CREATE TABLE `producto` (
  `ProductoId` int(11) NOT NULL,
  `Nombre` varchar(150) NOT NULL,
  `Descripcion` text DEFAULT NULL,
  `PrecioCosto` decimal(12,2) NOT NULL,
  `PrecioVenta` decimal(12,2) NOT NULL,
  `UnidadMedida` enum('unidad','metro','pack','bobina','combo') DEFAULT NULL,
  `RubroId` int(11) DEFAULT NULL,
  `ProveedorId` int(11) DEFAULT NULL,
  `stock` decimal(10,2) NOT NULL DEFAULT 0.00,
  `ProductoPadreId` int(11) DEFAULT NULL,
  `EquivalenciaEnPadre` decimal(10,8) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `producto`
--

INSERT INTO `producto` (`ProductoId`, `Nombre`, `Descripcion`, `PrecioCosto`, `PrecioVenta`, `UnidadMedida`, `RubroId`, `ProveedorId`, `stock`, `ProductoPadreId`, `EquivalenciaEnPadre`) VALUES
(1, 'Cable UTP EXT', 'Cable UTP Furukawa exterior bobina 305 mtrs', 158000.00, 230000.00, 'bobina', NULL, NULL, 54.00, NULL, NULL),
(2, 'Ficha rj45', 'ficha utp cat 6', 585.00, 1100.00, 'unidad', NULL, NULL, 51.00, NULL, NULL),
(3, 'Patchcord', 'patchcord rj45', 585.00, 1100.00, 'unidad', 2, NULL, 192.00, NULL, NULL),
(4, 'Cable UTP EXT x metro', 'furukawa exterior doble vaina x METRO', 485.00, 895.00, 'metro', 3, 1, 0.00, NULL, NULL),
(5, 'Rollo Cable 305m', 'Bobina completa de cable UTP', 12000.00, 18000.00, 'bobina', 3, 1, 2.00, NULL, NULL),
(6, 'Rollo Cable utp 305m', 'Bobina completa de cable UTP', 12000.00, 18000.00, 'bobina', 1, 1, 0.30, NULL, NULL),
(7, 'Cable x metro', 'Cable de red UTP por metro', 508.00, 1050.00, 'metro', 1, 1, 92.00, 6, 0.00327860),
(8, 'Pack 100 fichas RJ45 CAT6', 'Bolsa cerrada con 100 fichas de red CAT6', 20000.00, 28000.00, 'pack', 3, 1, 188.80, NULL, NULL),
(9, 'Ficha RJ45 CAT6 por unidad', 'Ficha de red CAT6 suelta', 200.00, 300.00, 'unidad', 3, 1, 18880.00, 8, 0.01000000),
(10, 'Cable CCTV interior', 'Cable  CCTV interior dos pares', 587.00, 897.00, 'metro', 3, 1, 36.53, NULL, NULL),
(11, 'Cable x metro CCTV interior', 'Cable x metro CCTV interior 2 pares', 483.00, 987.00, 'metro', 3, 1, 1217.00, 10, 0.03000000),
(13, 'Precinto bolsa x 100', 'precinto negro corto bolsa 100 unidades', 1500.00, 2800.00, 'pack', 3, 8, 6.04, NULL, NULL),
(14, 'prencito negro corto unidad', 'prencito negro corto x 1', 150.00, 320.00, 'unidad', 3, 8, 604.00, 13, 0.01000000);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `proveedor`
--

CREATE TABLE `proveedor` (
  `ProveedorId` int(11) NOT NULL,
  `Nombre` varchar(150) NOT NULL,
  `Email` varchar(150) DEFAULT NULL,
  `Telefono` varchar(50) DEFAULT NULL,
  `Direccion` varchar(255) DEFAULT NULL,
  `CUIT` varchar(50) DEFAULT NULL,
  `RazonSocial` varchar(150) DEFAULT NULL,
  `DatosBancarios` varchar(255) DEFAULT NULL,
  `Archivo1` varchar(255) DEFAULT NULL,
  `Archivo2` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `proveedor`
--

INSERT INTO `proveedor` (`ProveedorId`, `Nombre`, `Email`, `Telefono`, `Direccion`, `CUIT`, `RazonSocial`, `DatosBancarios`, `Archivo1`, `Archivo2`) VALUES
(3, 'Infoandina', 'info@d.com', '1111-2222', 'ccrfg 2587', '30-587465216-5', 'Infoandina sa', 'cbu 00198754125785211', NULL, NULL),
(4, 'air', 'air@g.com', '26648578459', 'carg 258', '30-58456987-9', 'air sa', 'cbu 001574265485', NULL, NULL),
(5, 'digital store', 'dgs@g.com', '52487142', 'saavedra 345', '23-4566744-4', 'dgs sa', NULL, NULL, NULL),
(6, 'elit', 'elit@gcom', '52487142', 'las heras 457', '25-85487-9', 'elit sa', NULL, NULL, NULL),
(7, 'stilus sa', NULL, NULL, NULL, '30-58456987-9', 'stilus sa', NULL, '7055267d-9597-4ae4-80d2-0690d5b1dbd6.jpg', '196863e8-bf17-48fb-a583-7a95a3ab3f20.jpg'),
(8, 'pc store', 'pc@gmail.com', '2614587985', 'Bayahibe.org, Parqueo Principal, Bayahíbe, República Dominicana', '25-85487-9', 'pc sa', NULL, '8272910b-ff82-4b3e-865d-a65acad7d1fe.png', '09cfb2ee-af29-4b1a-a321-4eb16598bc1c.jpg');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `rubro`
--

CREATE TABLE `rubro` (
  `RubroId` int(11) NOT NULL,
  `Nombre` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `rubro`
--

INSERT INTO `rubro` (`RubroId`, `Nombre`) VALUES
(2, 'Redes'),
(3, 'Cables');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE `usuario` (
  `UsuarioId` int(11) NOT NULL,
  `Email` varchar(150) NOT NULL,
  `PasswordHash` varchar(255) NOT NULL,
  `Rol` varchar(50) NOT NULL,
  `Avatar` varchar(255) DEFAULT NULL,
  `nombre` varchar(100) NOT NULL DEFAULT '',
  `apellido` varchar(100) NOT NULL DEFAULT ''
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuario`
--

INSERT INTO `usuario` (`UsuarioId`, `Email`, `PasswordHash`, `Rol`, `Avatar`, `nombre`, `apellido`) VALUES
(1, 'mauro@g.com', '$2a$11$lS48YXy7yjoTldMMefZ6TOI3s0xhv.waQ2WETO1e0e6cq7DK1ISI.', 'Admin', 'fdf0abd0-7420-4b65-bfd0-ddd36c64282a.jpg', 'Mauro', 'Benito'),
(4, 'ss@g.com', '$2a$11$0MB/Da0L4DH3FxBClGdemOpVMYVYg5YzRjzMWWG0rqhZwziqCDoTO', 'Empleado', 'e6a4a4d0-802b-4e78-ba6b-66b52ef95fb7.jpg', 'sofia', 'serri');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `venta`
--

CREATE TABLE `venta` (
  `VentaId` int(11) NOT NULL,
  `Fecha` datetime NOT NULL,
  `Total` decimal(12,2) NOT NULL,
  `TipoVenta` enum('Contado','Tarjeta Débito','Tarjeta Crédito','Transferencia') DEFAULT 'Contado',
  `UsuarioId` int(11) DEFAULT NULL,
  `ClienteId` int(11) DEFAULT NULL,
  `Anulada` bit(1) NOT NULL DEFAULT b'0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `venta`
--

INSERT INTO `venta` (`VentaId`, `Fecha`, `Total`, `TipoVenta`, `UsuarioId`, `ClienteId`, `Anulada`) VALUES
(1, '2025-07-01 17:30:00', 1252000.00, 'Contado', NULL, 1, b'1'),
(2, '2025-07-02 09:49:00', 1182000.00, 'Contado', NULL, 1, b'1'),
(8, '2025-07-06 11:55:15', 395000.00, NULL, 1, 2, b'1'),
(9, '2025-07-06 17:29:21', 11000.00, NULL, 1, 1, b'0'),
(10, '2025-07-07 09:44:02', 11000.00, NULL, 1, 1, b'0'),
(11, '2025-07-07 10:00:36', 230000.00, NULL, 1, 1, b'0'),
(12, '2025-07-07 10:07:52', 230000.00, NULL, 1, 2, b'1'),
(13, '2025-07-07 11:47:10', 460000.00, NULL, 1, 1, b'0'),
(14, '2025-07-07 12:19:34', 16500.00, NULL, 1, 1, b'0'),
(15, '2025-07-07 18:52:58', 1151100.00, NULL, 1, 2, b'0'),
(16, '2025-07-07 19:16:02', 231100.00, NULL, 1, 1, b'0'),
(17, '2025-07-07 19:22:05', 468800.00, NULL, 1, 1, b'0'),
(18, '2025-07-07 19:33:53', 230000.00, NULL, 1, 1, b'0'),
(19, '2025-07-07 19:41:37', 1100.00, NULL, 1, 2, b'0'),
(20, '2025-07-07 19:51:13', 4400.00, NULL, 1, 1, b'0'),
(21, '2025-07-07 21:29:59', 230000.00, NULL, 1, 2, b'0'),
(22, '2025-07-07 21:38:59', 231100.00, NULL, 1, 2, b'0'),
(23, '2025-07-08 09:26:27', 232200.00, NULL, 1, 1, b'0'),
(24, '2025-07-08 09:46:36', 11000.00, NULL, 1, 1, b'0'),
(25, '2025-07-08 09:47:57', 4400.00, NULL, 1, 1, b'0'),
(26, '2025-07-08 09:50:54', 1100.00, NULL, 1, 2, b'1'),
(27, '2025-07-08 09:51:50', 1100.00, NULL, 1, 2, b'1'),
(28, '2025-07-08 09:53:17', 1100.00, NULL, 1, 2, b'1'),
(29, '2025-07-08 16:05:07', 2200.00, NULL, 1, 2, b'1'),
(30, '2025-07-09 10:37:44', 11000.00, NULL, 1, 1, b'1'),
(31, '2025-07-09 11:10:25', 0.00, NULL, 1, 1, b'1'),
(32, '2025-07-09 11:10:50', 11000.00, NULL, 1, 2, b'1'),
(33, '2025-07-09 13:21:20', 16500.00, NULL, 1, 1, b'1'),
(34, '2025-07-09 13:37:01', 3300.00, NULL, 1, 1, b'1'),
(35, '2025-07-09 13:49:19', 16500.00, NULL, 1, 1, b'1'),
(36, '2025-07-09 16:58:16', 11000.00, NULL, 1, 1, b'0'),
(37, '2025-07-09 18:46:31', 236600.00, NULL, 1, 1, b'0'),
(38, '2025-07-10 11:43:21', 232200.00, NULL, 1, 2, b'1'),
(39, '2025-07-10 13:41:00', 1100.00, NULL, 1, 2, b'0'),
(40, '2025-07-10 15:48:47', 1100.00, NULL, 1, 1, b'0'),
(41, '2025-07-10 15:52:58', 1100.00, NULL, 1, 1, b'0'),
(42, '2025-07-10 15:56:25', 1100.00, NULL, 1, 2, b'0'),
(43, '2025-07-10 15:57:09', 230000.00, NULL, 1, 2, b'0'),
(44, '2025-07-10 16:04:57', 1100.00, NULL, 1, 2, b'0'),
(45, '2025-07-10 16:06:13', 0.00, NULL, 1, 2, b'0'),
(46, '2025-07-10 16:06:18', 0.00, NULL, 1, 2, b'0'),
(47, '2025-07-10 16:07:16', 230000.00, NULL, 1, 2, b'0'),
(48, '2025-07-10 16:33:16', 0.00, 'Tarjeta Débito', 1, 2, b'0'),
(49, '2025-07-10 16:33:21', 0.00, 'Contado', 1, 2, b'0'),
(50, '2025-07-10 16:36:12', 0.00, 'Contado', 1, 2, b'0'),
(51, '2025-07-10 16:36:24', 1100.00, 'Contado', 1, 1, b'0'),
(52, '2025-07-10 18:34:41', 2300000.00, 'Tarjeta Crédito', 1, 1, b'1'),
(53, '2025-07-10 20:15:18', 89500.00, 'Transferencia', 1, 1, b'0'),
(54, '2025-07-10 20:16:04', 0.00, 'Contado', 1, 2, b'0'),
(55, '2025-07-10 20:16:13', 183475.00, 'Contado', 1, 2, b'0'),
(56, '2025-07-11 11:42:39', 321300.00, 'Contado', 1, 2, b'0'),
(57, '2025-07-11 11:43:38', 0.00, 'Contado', 1, 1, b'0'),
(58, '2025-07-11 12:00:00', 321300.00, 'Tarjeta Crédito', 1, 2, b'0'),
(59, '2025-07-11 12:01:45', 322350.00, 'Contado', 1, 1, b'0'),
(60, '2025-07-11 12:02:34', 0.00, 'Contado', 1, 1, b'0'),
(61, '2025-07-11 12:03:00', 90000.00, 'Contado', 1, 1, b'0'),
(62, '2025-07-11 12:16:12', 0.00, 'Contado', 1, 2, b'0'),
(63, '2025-07-11 12:17:41', 323400.00, 'Contado', 1, 1, b'0'),
(64, '2025-07-11 17:58:21', 33000.00, 'Tarjeta Débito', 1, 1, b'0'),
(65, '2025-07-11 17:58:55', 315000.00, 'Contado', 1, 2, b'0'),
(66, '2025-07-11 17:59:54', 267000.00, 'Contado', 1, 1, b'0'),
(67, '2025-07-11 18:00:42', 0.00, 'Contado', 1, 1, b'0'),
(68, '2025-07-11 18:05:17', 315000.00, 'Contado', 1, 1, b'0'),
(69, '2025-07-11 18:13:32', 157500.00, 'Contado', 1, 2, b'0'),
(70, '2025-07-11 18:20:29', 210000.00, 'Contado', 1, 2, b'0'),
(71, '2025-07-11 18:20:53', 170100.00, 'Contado', 1, 2, b'0'),
(72, '2025-07-11 18:23:47', 126000.00, 'Transferencia', 1, 2, b'0'),
(73, '2025-07-11 18:24:15', 105000.00, 'Contado', 1, 1, b'0'),
(74, '2025-07-12 11:41:03', 3897.00, 'Tarjeta Débito', 1, 1, b'0'),
(75, '2025-07-12 11:42:57', 531035.00, 'Tarjeta Crédito', 1, 1, b'0'),
(76, '2025-07-14 12:23:15', 0.00, 'Contado', 1, 1, b'0'),
(77, '2025-07-14 12:23:17', 0.00, 'Contado', 1, 1, b'0'),
(78, '2025-07-14 12:59:56', 0.00, 'Contado', 1, 2, b'0'),
(79, '2025-07-14 13:00:19', 28000.00, 'Contado', 1, 2, b'1'),
(80, '2025-07-14 13:53:29', 0.00, 'Contado', 1, 2, b'0'),
(81, '2025-07-14 13:53:57', 18000.00, 'Contado', 1, 2, b'0'),
(82, '2025-07-14 16:51:40', 248000.00, 'Contado', 1, 2, b'0'),
(83, '2025-07-14 16:52:42', 0.00, NULL, 1, 1, b'1'),
(84, '2025-07-15 16:37:05', 0.00, 'Contado', 1, 2, b'0'),
(85, '2025-07-15 19:07:12', 18000.00, 'Contado', 1, 2, b'0'),
(86, '2025-07-16 09:25:35', 690000.00, 'Tarjeta Débito', 1, 1, b'0'),
(87, '2025-07-16 09:30:08', 0.00, 'Contado', 1, 2, b'0'),
(88, '2025-07-16 11:24:35', 1050.00, 'Tarjeta Débito', 1, 2, b'0'),
(89, '2025-07-16 16:44:28', 0.00, 'Contado', 1, 1, b'0'),
(90, '2025-07-16 16:44:35', 0.00, 'Contado', 1, 1, b'0'),
(91, '2025-07-16 17:44:21', 0.00, 'Contado', 1, 2, b'0'),
(92, '2025-07-16 17:44:26', 0.00, 'Contado', 1, 2, b'1'),
(93, '2025-07-16 17:44:29', 2310500.00, 'Contado', 1, 2, b'1'),
(94, '2025-07-17 09:43:01', 105000.00, 'Contado', 1, 1, b'1'),
(95, '2025-07-17 12:32:14', 2300000.00, 'Contado', 1, 2, b'0'),
(96, '2025-07-17 16:36:28', 125370.00, 'Tarjeta Crédito', 1, 1, b'0'),
(97, '2025-07-17 16:38:57', 897.00, 'Contado', 1, 2, b'1'),
(98, '2025-07-21 08:21:07', 2300000.00, 'Contado', 1, 3, b'0'),
(99, '2025-07-21 17:24:56', 48000.00, 'Contado', 1, 3, b'0'),
(100, '2025-07-21 17:27:40', 25600.00, 'Contado', 1, 3, b'1'),
(101, '2025-07-21 17:38:31', 0.00, 'Contado', 1, 3, b'0'),
(102, '2025-07-21 17:38:36', 252000.00, 'Contado', 1, 3, b'0'),
(103, '2025-07-21 17:39:05', 48000.00, 'Contado', 1, 3, b'1'),
(104, '2025-07-21 17:40:52', 48000.00, 'Contado', 1, 3, b'0'),
(105, '2025-07-21 17:42:30', 48000.00, 'Contado', 1, 3, b'0'),
(106, '2025-07-21 20:17:33', 0.00, 'Tarjeta Débito', 1, 4, b'0'),
(107, '2025-07-21 20:18:01', 1050.00, 'Tarjeta Débito', 1, 4, b'0'),
(108, '2025-07-22 16:34:47', 3697.00, 'Contado', 1, 4, b'0'),
(109, '2025-07-22 18:43:37', 320.00, 'Contado', 1, 3, b'1'),
(110, '2025-07-22 18:46:32', 157500.00, 'Contado', 1, 2, b'0'),
(111, '2025-07-22 18:47:19', 139650.00, 'Tarjeta Crédito', 1, 2, b'0'),
(112, '2025-07-24 16:47:41', 28000.00, 'Contado', 1, 4, b'0'),
(113, '2025-07-25 12:35:12', 55100.00, 'Contado', 1, 4, b'0'),
(114, '2025-07-25 15:59:08', 37200.00, 'Transferencia', 1, 3, b'0'),
(115, '2025-07-25 16:07:36', 3120.00, 'Contado', 1, 1, b'1'),
(116, '2025-07-25 16:09:35', 320.00, 'Contado', 1, 3, b'0'),
(117, '2025-07-25 16:14:03', 6000.00, 'Contado', 1, 4, b'1'),
(118, '2025-07-25 16:28:28', 3200.00, 'Contado', 1, 4, b'0'),
(119, '2025-07-25 16:29:10', 6000.00, 'Contado', 1, 4, b'1'),
(120, '2025-07-25 16:30:39', 4080.00, 'Contado', 1, 4, b'1'),
(122, '2025-07-27 13:52:04', 400.00, 'Contado', 1, 3, b'0'),
(123, '2025-07-27 15:06:17', 1794.00, 'Transferencia', 1, 1, b'0'),
(124, '2025-07-27 15:08:22', 897.00, 'Contado', 1, 1, b'0'),
(125, '2025-07-27 15:09:37', 0.00, 'Contado', 1, 3, b'0'),
(126, '2025-07-27 15:10:07', 0.00, 'Contado', 1, 3, b'0'),
(127, '2025-07-27 15:11:17', 0.00, 'Contado', 1, 3, b'0'),
(128, '2025-07-27 15:11:36', 400.00, 'Contado', 1, 3, b'0'),
(129, '2025-07-27 15:12:15', 230000.00, 'Tarjeta Débito', 1, 1, b'0'),
(130, '2025-07-27 15:18:09', 897.00, 'Contado', 1, 1, b'1'),
(131, '2025-07-27 15:25:42', 897.00, 'Tarjeta Débito', 1, 1, b'0'),
(132, '2025-07-27 19:12:08', 8970.00, 'Contado', 1, 4, b'0'),
(133, '2025-07-27 19:13:12', 1050.00, 'Tarjeta Crédito', 1, 4, b'0'),
(134, '2025-07-27 19:24:12', 897.00, 'Contado', 4, 3, b'0'),
(135, '2025-07-28 08:58:54', 9867.00, 'Contado', 1, 4, b'0'),
(136, '2025-07-28 10:37:29', 897.00, 'Contado', 1, 1, b'0'),
(137, '2025-07-28 10:38:00', 897.00, 'Tarjeta Crédito', 4, 1, b'0'),
(138, '2025-07-28 10:54:42', 897.00, 'Contado', 1, 1, b'0'),
(139, '2025-07-28 10:55:19', 230000.00, 'Transferencia', 4, 4, b'0'),
(140, '2025-07-28 10:59:44', 1050.00, 'Contado', 4, 1, b'0'),
(141, '2025-07-28 11:05:09', 897.00, 'Contado', 1, 1, b'0'),
(142, '2025-07-28 11:11:56', 897.00, 'Contado', 4, 1, b'0'),
(143, '2025-07-28 11:18:19', 897.00, 'Contado', 1, 1, b'0'),
(144, '2025-07-28 11:19:07', 28000.00, 'Contado', 4, 3, b'0'),
(145, '2025-07-28 11:43:29', 897.00, 'Contado', 4, 1, b'0'),
(146, '2025-07-28 11:46:12', 897.00, 'Contado', 1, 1, b'0'),
(147, '2025-07-28 11:50:47', 897.00, 'Contado', 4, 1, b'0'),
(148, '2025-07-28 11:51:09', 897.00, 'Contado', 1, 1, b'0'),
(149, '2025-07-28 11:51:49', 897.00, 'Tarjeta Crédito', 1, 1, b'0'),
(150, '2025-07-28 11:55:06', 897.00, 'Contado', 1, 1, b'0'),
(151, '2025-07-28 11:55:47', 56000.00, 'Tarjeta Crédito', 1, 3, b'0'),
(152, '2025-07-28 11:58:12', 2100.00, 'Tarjeta Débito', 1, 1, b'0'),
(153, '2025-07-28 11:58:52', 230000.00, 'Contado', 1, 1, b'0'),
(154, '2025-07-28 12:02:49', 897.00, 'Tarjeta Débito', 1, 2, b'0'),
(155, '2025-07-28 12:10:27', 6909.00, 'Tarjeta Débito', 1, 1, b'0'),
(156, '2025-07-28 12:10:32', 6909.00, 'Tarjeta Débito', 1, 1, b'0'),
(157, '2025-07-28 12:10:34', 6909.00, 'Tarjeta Débito', 1, 1, b'0'),
(158, '2025-07-28 12:10:54', 6909.00, 'Tarjeta Débito', 1, 1, b'0'),
(159, '2025-07-28 12:10:54', 6909.00, 'Tarjeta Débito', 1, 1, b'0'),
(160, '2025-07-28 12:10:55', 6909.00, 'Tarjeta Débito', 1, 1, b'0'),
(161, '2025-07-28 12:11:45', 897.00, 'Tarjeta Débito', 1, 2, b'0'),
(162, '2025-07-28 12:26:27', 1800.00, 'Tarjeta Débito', 1, 4, b'0'),
(163, '2025-07-28 12:28:32', 5500.00, 'Tarjeta Crédito', 1, 3, b'0'),
(164, '2025-07-28 12:49:42', 84000.00, 'Tarjeta Crédito', 1, 3, b'1'),
(165, '2025-07-28 16:19:39', 897.00, 'Contado', 4, 1, b'0'),
(166, '2025-07-28 17:06:52', 1974.00, 'Tarjeta Débito', 1, 4, b'0'),
(167, '2025-07-28 17:07:03', 400.00, 'Contado', 1, 3, b'0'),
(168, '2025-07-28 17:08:38', 9400.00, 'Tarjeta Crédito', 1, 4, b'1'),
(169, '2025-07-28 17:16:12', 2800.00, 'Contado', 1, 4, b'1'),
(170, '2025-07-28 17:17:26', 400.00, 'Contado', 1, 3, b'0'),
(171, '2025-07-28 17:17:54', 400.00, 'Contado', 1, 3, b'0'),
(172, '2025-07-28 18:35:21', 56000.00, 'Tarjeta Débito', 1, 1, b'0');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `ventadetalle`
--

CREATE TABLE `ventadetalle` (
  `VentaDetalleId` int(11) NOT NULL,
  `VentaId` int(11) NOT NULL,
  `ProductoId` int(11) NOT NULL,
  `Cantidad` int(11) NOT NULL,
  `PrecioUnitario` decimal(12,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `ventadetalle`
--

INSERT INTO `ventadetalle` (`VentaDetalleId`, `VentaId`, `ProductoId`, `Cantidad`, `PrecioUnitario`) VALUES
(1, 8, 3, 100, 1100.00),
(2, 8, 2, 50, 1100.00),
(3, 8, 1, 1, 230000.00),
(4, 9, 2, 10, 1100.00),
(5, 10, 2, 10, 1100.00),
(6, 11, 1, 1, 230000.00),
(7, 12, 1, 1, 230000.00),
(8, 13, 1, 2, 230000.00),
(9, 14, 2, 15, 1100.00),
(10, 15, 2, 1, 1100.00),
(11, 15, 1, 5, 230000.00),
(12, 16, 1, 1, 230000.00),
(13, 16, 2, 1, 1100.00),
(14, 17, 3, 8, 1100.00),
(15, 17, 1, 2, 230000.00),
(16, 18, 1, 1, 230000.00),
(17, 19, 3, 1, 1100.00),
(18, 20, 3, 4, 1100.00),
(19, 21, 1, 1, 230000.00),
(20, 22, 3, 1, 1100.00),
(21, 22, 1, 1, 230000.00),
(22, 23, 3, 2, 1100.00),
(23, 23, 1, 1, 230000.00),
(24, 24, 3, 10, 1100.00),
(25, 25, 3, 4, 1100.00),
(26, 26, 3, 1, 1100.00),
(27, 27, 3, 1, 1100.00),
(28, 28, 3, 1, 1100.00),
(29, 29, 3, 1, 1100.00),
(30, 29, 2, 1, 1100.00),
(31, 30, 3, 10, 1100.00),
(32, 32, 3, 10, 1100.00),
(33, 33, 3, 15, 1100.00),
(34, 34, 2, 3, 1100.00),
(35, 35, 3, 15, 1100.00),
(36, 36, 3, 10, 1100.00),
(37, 37, 3, 5, 1100.00),
(38, 37, 2, 1, 1100.00),
(39, 37, 1, 1, 230000.00),
(40, 38, 1, 1, 230000.00),
(41, 38, 2, 1, 1100.00),
(42, 38, 3, 1, 1100.00),
(43, 39, 3, 1, 1100.00),
(44, 40, 3, 1, 1100.00),
(45, 41, 3, 1, 1100.00),
(46, 42, 3, 1, 1100.00),
(47, 43, 1, 1, 230000.00),
(48, 44, 3, 1, 1100.00),
(49, 47, 1, 1, 230000.00),
(50, 51, 2, 1, 1100.00),
(51, 52, 1, 10, 230000.00),
(52, 53, 4, 100, 895.00),
(53, 55, 4, 205, 895.00),
(54, 56, 7, 306, 1050.00),
(55, 58, 7, 306, 1050.00),
(56, 59, 7, 307, 1050.00),
(57, 61, 6, 5, 18000.00),
(58, 63, 7, 308, 1050.00),
(59, 64, 9, 110, 300.00),
(60, 65, 7, 300, 1050.00),
(61, 66, 9, 890, 300.00),
(62, 68, 7, 300, 1050.00),
(63, 69, 7, 150, 1050.00),
(64, 70, 7, 200, 1050.00),
(65, 71, 7, 162, 1050.00),
(66, 72, 7, 120, 1050.00),
(67, 73, 7, 100, 1050.00),
(68, 74, 10, 1, 897.00),
(69, 74, 9, 10, 300.00),
(70, 75, 1, 1, 230000.00),
(71, 75, 11, 305, 987.00),
(72, 79, 8, 1, 28000.00),
(73, 81, 5, 1, 18000.00),
(74, 82, 5, 1, 18000.00),
(75, 82, 1, 1, 230000.00),
(76, 85, 5, 1, 18000.00),
(77, 86, 1, 3, 230000.00),
(78, 88, 7, 1, 1050.00),
(79, 93, 1, 10, 230000.00),
(80, 93, 7, 10, 1050.00),
(81, 94, 7, 100, 1050.00),
(82, 95, 1, 10, 230000.00),
(83, 96, 10, 10, 897.00),
(84, 96, 2, 4, 1100.00),
(85, 96, 8, 4, 28000.00),
(86, 97, 10, 1, 897.00),
(87, 98, 1, 10, 230000.00),
(88, 99, 14, 150, 320.00),
(89, 100, 14, 80, 320.00),
(90, 102, 13, 90, 2800.00),
(91, 103, 14, 150, 320.00),
(92, 104, 14, 150, 320.00),
(93, 105, 14, 150, 320.00),
(94, 107, 7, 1, 1050.00),
(95, 108, 13, 1, 2800.00),
(96, 108, 10, 1, 897.00),
(97, 109, 14, 1, 320.00),
(98, 110, 7, 150, 1050.00),
(99, 111, 7, 133, 1050.00),
(100, 112, 8, 1, 28000.00),
(101, 113, 5, 3, 18000.00),
(102, 113, 2, 1, 1100.00),
(103, 114, 14, 20, 320.00),
(104, 114, 13, 1, 2800.00),
(105, 114, 8, 1, 28000.00),
(106, 115, 14, 1, 320.00),
(107, 115, 13, 1, 2800.00),
(108, 116, 14, 1, 320.00),
(109, 117, 14, 10, 320.00),
(110, 117, 13, 1, 2800.00),
(111, 118, 14, 10, 320.00),
(112, 119, 13, 1, 2800.00),
(113, 119, 14, 10, 320.00),
(114, 120, 13, 1, 2800.00),
(115, 120, 14, 4, 320.00),
(116, 122, 5, 2, 150.00),
(117, 122, 6, 1, 100.00),
(118, 123, 10, 2, 897.00),
(119, 124, 10, 1, 897.00),
(120, 128, 3, 2, 150.00),
(121, 128, 9, 1, 100.00),
(122, 129, 1, 1, 230000.00),
(123, 130, 10, 1, 897.00),
(124, 131, 10, 1, 897.00),
(125, 132, 10, 10, 897.00),
(126, 133, 7, 1, 1050.00),
(127, 134, 10, 1, 897.00),
(128, 135, 10, 11, 897.00),
(129, 136, 10, 1, 897.00),
(130, 137, 10, 1, 897.00),
(131, 138, 10, 1, 897.00),
(132, 139, 1, 1, 230000.00),
(133, 140, 7, 1, 1050.00),
(134, 141, 10, 1, 897.00),
(135, 142, 10, 1, 897.00),
(136, 143, 10, 1, 897.00),
(137, 144, 8, 1, 28000.00),
(138, 145, 10, 1, 897.00),
(139, 146, 10, 1, 897.00),
(140, 147, 10, 1, 897.00),
(141, 148, 10, 1, 897.00),
(142, 149, 10, 1, 897.00),
(143, 150, 10, 1, 897.00),
(144, 151, 8, 2, 28000.00),
(145, 152, 7, 2, 1050.00),
(146, 153, 1, 1, 230000.00),
(147, 154, 10, 1, 897.00),
(148, 155, 11, 7, 987.00),
(149, 156, 11, 7, 987.00),
(150, 157, 11, 7, 987.00),
(151, 158, 11, 7, 987.00),
(152, 159, 11, 7, 987.00),
(153, 160, 11, 7, 987.00),
(154, 161, 10, 1, 897.00),
(155, 162, 9, 6, 300.00),
(156, 163, 2, 5, 1100.00),
(157, 164, 8, 3, 28000.00),
(158, 165, 10, 1, 897.00),
(159, 166, 11, 2, 987.00),
(160, 167, 3, 2, 150.00),
(161, 167, 9, 1, 100.00),
(162, 168, 3, 6, 1100.00),
(163, 168, 13, 1, 2800.00),
(164, 169, 13, 1, 2800.00),
(165, 170, 3, 2, 150.00),
(166, 170, 9, 1, 100.00),
(167, 171, 3, 2, 150.00),
(168, 171, 9, 1, 100.00),
(169, 172, 8, 2, 28000.00);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `cliente`
--
ALTER TABLE `cliente`
  ADD PRIMARY KEY (`ClienteId`);

--
-- Indices de la tabla `producto`
--
ALTER TABLE `producto`
  ADD PRIMARY KEY (`ProductoId`),
  ADD KEY `ProductoPadreId` (`ProductoPadreId`);

--
-- Indices de la tabla `proveedor`
--
ALTER TABLE `proveedor`
  ADD PRIMARY KEY (`ProveedorId`);

--
-- Indices de la tabla `rubro`
--
ALTER TABLE `rubro`
  ADD PRIMARY KEY (`RubroId`);

--
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`UsuarioId`);

--
-- Indices de la tabla `venta`
--
ALTER TABLE `venta`
  ADD PRIMARY KEY (`VentaId`),
  ADD KEY `UsuarioId` (`UsuarioId`),
  ADD KEY `ClienteId` (`ClienteId`);

--
-- Indices de la tabla `ventadetalle`
--
ALTER TABLE `ventadetalle`
  ADD PRIMARY KEY (`VentaDetalleId`),
  ADD KEY `VentaId` (`VentaId`),
  ADD KEY `ProductoId` (`ProductoId`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `cliente`
--
ALTER TABLE `cliente`
  MODIFY `ClienteId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `producto`
--
ALTER TABLE `producto`
  MODIFY `ProductoId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT de la tabla `proveedor`
--
ALTER TABLE `proveedor`
  MODIFY `ProveedorId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `rubro`
--
ALTER TABLE `rubro`
  MODIFY `RubroId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `UsuarioId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `venta`
--
ALTER TABLE `venta`
  MODIFY `VentaId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=173;

--
-- AUTO_INCREMENT de la tabla `ventadetalle`
--
ALTER TABLE `ventadetalle`
  MODIFY `VentaDetalleId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=170;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `producto`
--
ALTER TABLE `producto`
  ADD CONSTRAINT `producto_ibfk_1` FOREIGN KEY (`ProductoPadreId`) REFERENCES `producto` (`ProductoId`);

--
-- Filtros para la tabla `venta`
--
ALTER TABLE `venta`
  ADD CONSTRAINT `venta_ibfk_1` FOREIGN KEY (`UsuarioId`) REFERENCES `usuario` (`UsuarioId`),
  ADD CONSTRAINT `venta_ibfk_2` FOREIGN KEY (`ClienteId`) REFERENCES `cliente` (`ClienteId`);

--
-- Filtros para la tabla `ventadetalle`
--
ALTER TABLE `ventadetalle`
  ADD CONSTRAINT `ventadetalle_ibfk_1` FOREIGN KEY (`VentaId`) REFERENCES `venta` (`VentaId`),
  ADD CONSTRAINT `ventadetalle_ibfk_2` FOREIGN KEY (`ProductoId`) REFERENCES `producto` (`ProductoId`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2024. Feb 06. 13:54
-- Kiszolgáló verziója: 10.4.28-MariaDB
-- PHP verzió: 8.2.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `kkfestmenyek`
--

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `festmenyek`
--

CREATE TABLE `festmenyek` (
  `kk_id` int(11) NOT NULL,
  `kk_nev` varchar(512) CHARACTER SET utf8 COLLATE utf8_hungarian_ci NOT NULL,
  `kk_festo` varchar(256) CHARACTER SET utf8 COLLATE utf8_hungarian_ci NOT NULL,
  `kk_evszam` int(11) NOT NULL,
  `kk_szelesseg` double NOT NULL,
  `kk_magassag` double NOT NULL,
  `kk_kep` varchar(100) CHARACTER SET utf8 COLLATE utf8_hungarian_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `festmenyek`
--

INSERT INTO `festmenyek` (`kk_id`, `kk_nev`, `kk_festo`, `kk_evszam`, `kk_szelesseg`, `kk_magassag`, `kk_kep`) VALUES
(1, 'Vénusz születése', 'Sandro Botticeli', 1485, 1.72, 2.78, 'venusz_szuletese.jpg'),
(2, 'Éjjeli őrjárat', 'Rembrandt Vermeer', 1642, 3.63, 4.37, 'ejjeli_orjarat.jpg'),
(3, 'A Sikoly', 'Edvard Munch', 1893, 0.91, 0.735, 'a_sikoly.jpg'),
(4, 'Lány gyöngy fülbevalóval', 'Johannes Vermeer', 1665, 0.44, 0.39, 'lany_gyongy_fulbevaloval.jpg'),
(5, 'festmeny', 'festo', 1, 2, 3, ''),
(6, 'festoooo', 'festoooo', 123, 2, 3, 'kep_csevego.PNG');

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `festmenyek`
--
ALTER TABLE `festmenyek`
  ADD PRIMARY KEY (`kk_id`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `festmenyek`
--
ALTER TABLE `festmenyek`
  MODIFY `kk_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

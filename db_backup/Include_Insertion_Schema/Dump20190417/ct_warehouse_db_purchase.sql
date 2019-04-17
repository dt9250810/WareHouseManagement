-- MySQL dump 10.13  Distrib 8.0.12, for Win64 (x86_64)
--
-- Host: localhost    Database: ct_warehouse_db
-- ------------------------------------------------------
-- Server version	8.0.12

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
 SET NAMES utf8 ;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `purchase`
--

DROP TABLE IF EXISTS `purchase`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `purchase` (
  `purchaseID` bigint(20) NOT NULL AUTO_INCREMENT,
  `goodsID` int(11) NOT NULL,
  `quantity` int(11) NOT NULL,
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `saveGoods` tinyint(4) NOT NULL DEFAULT '0',
  PRIMARY KEY (`purchaseID`),
  KEY `goods_FK_2_idx` (`goodsID`),
  CONSTRAINT `goods_FK_2` FOREIGN KEY (`goodsID`) REFERENCES `goods` (`goodsid`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `purchase`
--

LOCK TABLES `purchase` WRITE;
/*!40000 ALTER TABLE `purchase` DISABLE KEYS */;
INSERT INTO `purchase` (`purchaseID`, `goodsID`, `quantity`, `created_date`, `saveGoods`) VALUES (1,4,5,'2018-10-09 01:52:54',1),(2,5,2,'2018-10-09 01:52:54',1),(3,4,5,'2018-11-02 02:27:42',1),(4,5,8,'2018-11-02 02:27:42',1),(5,9,10,'2018-11-02 02:29:30',1),(6,4,3,'2018-11-02 02:30:36',1),(7,4,3,'2018-11-02 02:30:36',1),(8,4,1,'2018-11-02 02:32:26',1),(9,4,1,'2018-11-02 02:32:26',1),(10,3,10,'2018-11-02 02:33:34',1),(11,4,5,'2018-11-02 15:52:37',1),(12,5,5,'2018-11-02 15:52:37',1),(13,8,10,'2018-11-02 17:03:54',1),(14,7,10,'2018-11-02 17:03:54',1);
/*!40000 ALTER TABLE `purchase` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-04-17 22:12:08

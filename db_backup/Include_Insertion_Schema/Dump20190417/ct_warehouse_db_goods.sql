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
-- Table structure for table `goods`
--

DROP TABLE IF EXISTS `goods`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `goods` (
  `goodsID` int(11) NOT NULL AUTO_INCREMENT,
  `type` varchar(50) NOT NULL,
  `categoryID` int(11) NOT NULL,
  `spec` varchar(150) DEFAULT NULL,
  `brandID` int(11) NOT NULL,
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `updated_date` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `remark` varchar(50) DEFAULT NULL,
  `quantity` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`goodsID`),
  KEY `brandID_FK_idx` (`brandID`),
  KEY `categoryID_FK_idx` (`categoryID`),
  CONSTRAINT `brandID_FK` FOREIGN KEY (`brandID`) REFERENCES `brand` (`brandid`),
  CONSTRAINT `categoryID_FK` FOREIGN KEY (`categoryID`) REFERENCES `category` (`categoryid`)
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `goods`
--

LOCK TABLES `goods` WRITE;
/*!40000 ALTER TABLE `goods` DISABLE KEYS */;
INSERT INTO `goods` (`goodsID`, `type`, `categoryID`, `spec`, `brandID`, `created_date`, `updated_date`, `remark`, `quantity`) VALUES (1,'TEST-1000',1,'220V',1,'2018-09-24 18:14:19','2018-10-12 23:36:08','',100),(2,'TEST-1001',7,'白光',2,'2018-09-24 19:26:24','2018-10-12 23:36:11','absdd',100),(3,'TESTTTT-151',8,'',2,'2018-09-24 19:26:24','2018-11-02 04:20:38','',100),(4,'TEST-515156',2,'110V轉220V',3,'2018-09-24 19:26:24','2018-11-02 15:52:39','',105),(5,'EP-15151',2,'110V',1,'2018-09-24 19:27:36','2018-11-02 15:52:39','',105),(7,'AA-1234455',4,'測試敘述規格的字元數量',3,'2018-09-24 20:24:14','2018-11-02 17:03:55','',110),(8,'TEST-d51516',5,'fff',4,'2018-09-24 20:42:03','2018-11-02 17:03:55','fff',110),(9,'SASD-151',6,'測試敘述規格的字元',5,'2018-09-24 20:51:34','2018-11-02 04:20:38','',100),(10,'FS-1025151',10,'',5,'2018-09-24 21:12:04','2018-10-12 23:36:33','kkkk',100),(16,'HH-sdsd555',8,'this is blank...',4,'2018-09-24 21:16:04','2018-10-12 23:36:35','',100),(22,'gddhfg',2,'',2,'2018-10-05 06:22:49','2018-10-12 23:36:37','',100),(23,'sadsad',4,'',4,'2018-10-06 16:56:19','2018-10-12 23:36:39','',100),(27,'TEST',12,'',6,'2018-10-07 14:49:19','2018-10-12 23:36:41','',100);
/*!40000 ALTER TABLE `goods` ENABLE KEYS */;
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

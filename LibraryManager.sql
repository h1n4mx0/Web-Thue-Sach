/*!999999\- enable the sandbox mode */ 
-- MariaDB dump 10.19  Distrib 10.11.8-MariaDB, for debian-linux-gnu (x86_64)
--
-- Host: localhost    Database: LibraryManager
-- ------------------------------------------------------
-- Server version	10.11.8-MariaDB-1

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `Authors`
--

DROP TABLE IF EXISTS `Authors`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Authors` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `Bio` text DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Authors`
--

LOCK TABLES `Authors` WRITE;
/*!40000 ALTER TABLE `Authors` DISABLE KEYS */;
INSERT INTO `Authors` VALUES
(1,'J.K. Rowling','Tác giả người Anh, nổi tiếng với loạt sách Harry Potter.'),
(2,'George R.R. Martin','Tiểu thuyết gia và nhà viết truyện ngắn người Mỹ, nổi tiếng với loạt sách A Song of Ice and Fire.'),
(3,'J.R.R. Tolkien','Nhà văn, nhà thơ và giáo sư người Anh, nổi tiếng với bộ sách Chúa tể những chiếc nhẫn.'),
(4,'Agatha Christie','Nhà văn người Anh nổi tiếng với các tiểu thuyết trinh thám, đặc biệt là các nhân vật Hercule Poirot và Miss Marple.'),
(5,'Stephen King','Tác giả người Mỹ nổi tiếng với các tiểu thuyết kinh dị, siêu nhiên và hồi hộp.'),
(6,'Isaac Asimov','Nhà văn và giáo sư sinh hóa người Nga-Mỹ, nổi tiếng với các tác phẩm khoa học viễn tưởng như loạt Foundation.'),
(7,'Arthur Conan Doyle','Nhà văn người Anh, nổi tiếng với các tác phẩm trinh thám về nhân vật Sherlock Holmes.'),
(8,'Mark Twain','Nhà văn, nhà hài hước và giảng viên người Mỹ, nổi tiếng với Những cuộc phiêu lưu của Tom Sawyer và Huckleberry Finn.'),
(9,'Jane Austen','Tiểu thuyết gia người Anh, nổi tiếng với các tác phẩm như Kiêu hãnh và Định kiến và Lý trí và Tình cảm.'),
(10,'Ernest Hemingway','Tiểu thuyết gia, nhà văn truyện ngắn và nhà báo người Mỹ, nổi tiếng với tác phẩm Ông già và biển cả.');
/*!40000 ALTER TABLE `Authors` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `BookContents`
--

DROP TABLE IF EXISTS `BookContents`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `BookContents` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `BookId` int(11) NOT NULL,
  `Chapter` varchar(50) NOT NULL,
  `Title` varchar(255) NOT NULL,
  `Content` longtext NOT NULL,
  `PageStart` int(11) DEFAULT NULL,
  `PageEnd` int(11) DEFAULT NULL,
  `Status` enum('Draft','Published') DEFAULT 'Draft',
  `CreatedAt` datetime DEFAULT current_timestamp(),
  `UpdatedAt` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`Id`),
  KEY `FK_BookContents_Books` (`BookId`),
  CONSTRAINT `FK_BookContents_Books` FOREIGN KEY (`BookId`) REFERENCES `Books` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `BookContents`
--

LOCK TABLES `BookContents` WRITE;
/*!40000 ALTER TABLE `BookContents` DISABLE KEYS */;
INSERT INTO `BookContents` VALUES
(1,2,'Chapter 1','Khởi đầu ngày mới với niềm cảm hứng','Nội dung về cách khởi đầu ngày mới đầy cảm hứng...',1,15,'Published','2024-12-29 14:51:20','2024-12-29 14:51:20'),
(2,2,'Chapter 2','Chinh phục nỗi sợ hãi','Học cách vượt qua những nỗi sợ hãi trong cuộc sống...',16,30,'Published','2024-12-29 14:51:20','2024-12-29 14:51:20'),
(3,2,'Chapter 3','Tầm quan trọng của sự tập trung','Làm thế nào để giữ sự tập trung và hoàn thành mục tiêu...',31,45,'Published','2024-12-29 14:51:20','2024-12-29 14:51:20'),
(4,2,'Chapter 4','Khai phá tiềm năng bản thân','Khám phá khả năng tiềm ẩn và tạo ra cuộc sống ý nghĩa...',46,60,'Draft','2024-12-29 14:51:20','2024-12-29 14:51:20');
/*!40000 ALTER TABLE `BookContents` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Books`
--

DROP TABLE IF EXISTS `Books`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Books` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Title` varchar(255) NOT NULL,
  `CategoryId` int(11) DEFAULT NULL,
  `AuthorId` int(11) DEFAULT NULL,
  `ISBN` varchar(20) DEFAULT NULL,
  `PublishedYear` int(11) DEFAULT NULL,
  `Summary` text DEFAULT NULL,
  `CoverImage` varchar(255) DEFAULT NULL,
  `RentalPrice` varchar(50) DEFAULT NULL,
  `ReadingDuration` varchar(50) DEFAULT NULL,
  `Views` int(11) DEFAULT 0,
  `Status` enum('Đã hoàn thành','Đang cập nhật') DEFAULT 'Đã hoàn thành',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `ISBN` (`ISBN`),
  KEY `CategoryId` (`CategoryId`),
  KEY `AuthorId` (`AuthorId`),
  CONSTRAINT `Books_ibfk_1` FOREIGN KEY (`CategoryId`) REFERENCES `Categories` (`Id`),
  CONSTRAINT `Books_ibfk_2` FOREIGN KEY (`AuthorId`) REFERENCES `Authors` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=37 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Books`
--

LOCK TABLES `Books` WRITE;
/*!40000 ALTER TABLE `Books` DISABLE KEYS */;
INSERT INTO `Books` VALUES
(1,'Flow – Mihaly Csikszentmihalyi ',1,1,'9781234567890',2020,'Khái niệm, lý thuyết Flow được xem như trụ cột trong tâm lý học tích cực, vẫn đang được tiếp tục nghiên cứu và áp dụng trong hầu hết các ngành Khoa học Xã hội, Kinh tế, thậm chí trong nghiên cứu sự tiến hóa của nhân loại, và các lĩnh vực đời sống khác nhau, nhằm cải thiện chất lượng cuộc sống, bao gồm cả tâm lý trị liệu, cải tạo tội phạm vị thành niên, giáo dục, âm nhạc,…','/bookImages/72c8f6b5-6882-4699-9d52-fd576e2ce31e.png','2,222 VNĐ','2',100,'Đã hoàn thành'),
(2,'Đời Ngắn Đừng Ngủ Dài – Robin Sharma',1,2,'9780987654321',2019,'Đời ngắn đừng ngủ dài, khi đọc tựa đề của cuốn sách này mình thầm nghĩ chắc là tác giả nói đến vấn đề của những người mê ngủ. Nhưng khi đọc nội dung thì tác giả chẳng hề nói gì đến việc ngủ, mà tác giả dùng từ “ngủ dài” ở đây là để ám chỉ cho những người còn vô định trong cuộc sống, những người chưa trả lời được câu hỏi “tôi là ai”.','~/bookImages/doi-ngan-dung-ngu-dai-robin-sharma.jpg','7,000 VNĐ','2h15m',250,'Đang cập nhật'),
(3,'Vị Tu Sĩ Bán Chiếc Ferrari – Robin Sharma',1,5,'9788276374824',2014,'Trong vô vàn những cuốn sách trên kệ sách của nhà sách, tôi ấn tượng với cuốn sách trên bởi màu sắc bìa sách nhẹ nhàng, tên cuốn sách đặt luôn cho tôi câu hỏi “Vị tu sĩ BÁN…?” Tu sĩ đi bán một cái gì đó à? sao lạ quá, tôi tiện google tra luôn Ferrari – chiếc xe khá đẹp và thuộc hàng đắt của xịn. Vậy là tôi cầm lên đọc thử.','~/bookImages/vi-tu-si-ban-chiec-ferrari-robin-sharma.jpg','3,000 VNĐ','1h35m',424,'Đã hoàn thành'),
(11,'Khuyến Học – Fukuzawa Yukichi ',2,4,'9780987654836',2016,'Một quyển sách mà bất cứ thanh niên nào cũng nên đọc một lần. Tựa đề quyển sách là Khuyến học, hiểu đơn giản là khuyến khích học tập, nhưng, học tập ở đây là học cái gì, học như thế nào và học để làm gì?\r\n\r\n“Kiếm kế sinh nhai cũng là học vấn. Lập sổ sách thu chi trong buôn bán cũng là học vấn. Nắm bắt thời cuộc cũng là học vấn. Nếu chỉ đơn thuần đọc vào là sách Tây, sách Tàu, sách Nhật thì không thể coi là có học vấn.','~/bookImages/dee5f0b0-e3d3-4755-8b6c-3f2ce17376a9.jpg','3,000 VNĐ','2h30m',0,'Đang cập nhật'),
(12,'Chó Sủa Nhầm Cây – Eric Barker',2,8,'9872536253412',2021,'Quyển sách này có tên tiếng Anh nguyên bản là “Barking up the wrong tree”, cái tên khá đặc biệt mà nếu mọi người nghe qua sẽ khó hình dung được chủ đề sách viết về điều gì. Tuy nhiên quyển này là quyển được cộng đồng thế giới rất ưa chuộng, xếp hạng #2 sách bán chạy của Wall Street Journal, sách được đánh giá 4.2/5 với hơn 7.5 ngàn reviews trên goodreads và 4.5/5 với gần 30 bình chọn trên Tiki. Mình biết đến quyển này do một lần tình cờ thấy được giới thiệu trên mạng internet.','~/bookImages/c70811b7-0976-424b-b19a-0541f4f36852.jpg','2,000 VNĐ','1h15m',0,'Đã hoàn thành'),
(13,' Lẽ Phải Của Phi Lý Trí – Dan Ariely',2,9,'9735241534232',2021,'Series này gồm 4 quyển: phi lý trí, lẽ phải của phi lý trí, phi lý một cách hợp lý, bản chất của dối trá được alphabook phát hành.\r\nXuất phát từ trải nghiệm cá nhân tác giả bị bỏng độ 3 trên khoảng 70% cơ thể vào năm 18 tuổi. Trong khoảng thời gian nằm viện gần 3 năm, ông có nhiều thời gian quan sát các thói quen và hành động của mọi người xung quanh. Ông đã đặt ra câu hỏi tại sao chúng ta lại hành động theo cách mà mình vẫn thường hay làm? Mặc dù những cách mang lại hiệu quả kém và vô lý. Điều đó thúc đẩy ông trở thành một nhà nghiên cứu xã hội.','~/bookImages/bebc9a2c-4726-4a08-86ab-ce6b9b2aca52.jpg','10,000 VNĐ','2h0m',0,'Đã hoàn thành'),
(14,' Sổ Tay Nhà Thôi Miên 1 – Cao Minh',3,10,'9874653726543',2024,'“Thế giới này vẫn luôn bình thường. Nó sẽ không uất ức, không áp lực, chẳng khác mấy trăm triệu năm trước là bao. Chính chúng ta mới có vấn đề. Chúng ta uất ức, chịu đựng áp lực, thậm chí suy sụp. Chúng ta nghi ngờ tất cả, bởi vậy chúng ta mới bất an.”\r\n\r\nVà chúng ta, cứ vùng vẫy giữa trái hoặc phải, quên mất điểm cân bằng. Thôi miên không ru ta vào cơn say ngủ, thôi miên đánh thức chúng ta khỏi cơn ngủ say.\r\n\r\nMột cuốn sách rất thú vị cho những ai đang quan tâm đến chủ đề thôi miên, và thôi miên theo cách hiểu của mình sau khi đọc cuốn sách này đó chính là một phương thức để khơi gợi tâm trí, điều trị nhận thức hành vi. Thôi miên nói chung và cuốn sách này nói riêng là một trong những đề tài còn mới mẻ trong cuộc sống con người Việt Nam, tuy nhiên đây lại là đề tài đang được rất đông cộng đồng tâm lý quan tâm, giúp chúng ta nhìn nhận được vấn đề sâu thẳm trong tiềm thức.','~/bookImages/813da12e-24ae-4c89-ab59-be778cef3196.jpg','12,000 VNĐ','2h5m',0,'Đang cập nhật');
/*!40000 ALTER TABLE `Books` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Categories`
--

DROP TABLE IF EXISTS `Categories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Categories` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Categories`
--

LOCK TABLES `Categories` WRITE;
/*!40000 ALTER TABLE `Categories` DISABLE KEYS */;
INSERT INTO `Categories` VALUES
(1,'Sách phong cách sống'),
(2,'Sách tư duy'),
(3,'Sách tâm lý học'),
(4,'Sách nghệ thuật sống'),
(5,'Sách kỹ năng làm việc'),
(6,'Sách làm cha mẹ'),
(7,'Văn học kinh điển'),
(8,'Sách Tiểu sử – Hồi ký'),
(9,'Tiểu thuyết'),
(10,'Truyện ngắn – Tản văn – Tạp văn'),
(11,'Văn học tuổi mới lớn'),
(12,'Truyện trinh thám'),
(13,'Sách lịch sử, văn minh thế giới'),
(14,'Sách địa lý, chính trị'),
(15,'Sách danh nhân, doanh nhân'),
(16,'Sách tôn giáo, tâm linh'),
(17,'Sách kinh tế học'),
(18,'Sách y học, dinh dưỡng');
/*!40000 ALTER TABLE `Categories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Chapters`
--

DROP TABLE IF EXISTS `Chapters`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Chapters` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `BookId` int(11) NOT NULL,
  `ChapterNumber` int(11) NOT NULL,
  `Title` varchar(255) DEFAULT NULL,
  `FilePath` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `BookId` (`BookId`),
  CONSTRAINT `Chapters_ibfk_1` FOREIGN KEY (`BookId`) REFERENCES `Books` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Chapters`
--

LOCK TABLES `Chapters` WRITE;
/*!40000 ALTER TABLE `Chapters` DISABLE KEYS */;
/*!40000 ALTER TABLE `Chapters` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Permissions`
--

DROP TABLE IF EXISTS `Permissions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Permissions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PermissionName` varchar(100) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `PermissionName` (`PermissionName`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Permissions`
--

LOCK TABLES `Permissions` WRITE;
/*!40000 ALTER TABLE `Permissions` DISABLE KEYS */;
INSERT INTO `Permissions` VALUES
(2,'Manage Books'),
(1,'Manage Users'),
(4,'Rent Books'),
(5,'View Books'),
(3,'View Reports');
/*!40000 ALTER TABLE `Permissions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Rentals`
--

DROP TABLE IF EXISTS `Rentals`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Rentals` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserAccountId` int(11) NOT NULL,
  `ISBN` varchar(20) NOT NULL,
  `RentalDate` datetime NOT NULL,
  `ReturnDate` datetime NOT NULL,
  `RentalStatus` enum('Đang thuê','Hết hạn') DEFAULT 'Đang thuê',
  PRIMARY KEY (`Id`),
  KEY `UserAccountId` (`UserAccountId`),
  KEY `ISBN` (`ISBN`),
  CONSTRAINT `Rentals_ibfk_1` FOREIGN KEY (`UserAccountId`) REFERENCES `UserAccounts` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `Rentals_ibfk_2` FOREIGN KEY (`ISBN`) REFERENCES `Books` (`ISBN`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Rentals`
--

LOCK TABLES `Rentals` WRITE;
/*!40000 ALTER TABLE `Rentals` DISABLE KEYS */;
INSERT INTO `Rentals` VALUES
(1,1,'9781234567890','2024-11-05 07:52:34','2024-11-12 07:52:34','Hết hạn'),
(2,2,'9781234567890','2024-11-05 08:09:42','2024-11-05 08:10:42','Hết hạn'),
(3,2,'9781234567890','2024-11-05 08:14:24','2024-11-05 08:15:24','Hết hạn'),
(4,1,'9788276374824','2024-11-06 10:31:01','2024-11-13 10:31:01','Hết hạn'),
(5,1,'9780987654321','2024-11-06 10:32:08','2024-11-06 10:33:08','Hết hạn'),
(6,1,'9780987654321','2024-11-06 10:52:34','2024-11-06 10:53:34','Hết hạn'),
(7,1,'9874653726543','2024-12-12 13:36:36','2024-12-12 13:37:36','Hết hạn'),
(8,1,'9781234567890','2024-12-25 13:57:26','2024-12-25 13:58:26','Hết hạn'),
(9,5,'9780987654321','2024-12-26 23:30:49','2024-12-26 23:31:49','Hết hạn'),
(10,5,'9781234567890','2024-12-27 20:07:55','2024-12-27 20:08:55','Hết hạn'),
(11,5,'9788276374824','2024-12-27 21:38:17','2024-12-27 21:39:17','Hết hạn'),
(12,5,'9788276374824','2024-12-27 21:40:39','2024-12-27 22:10:39','Hết hạn'),
(13,5,'9872536253412','2024-12-27 22:33:04','2024-12-27 23:03:04','Hết hạn'),
(15,5,'9872536253412','2024-12-29 08:08:49','2024-12-29 08:38:49','Hết hạn'),
(16,5,'9780987654321','2024-12-29 08:16:51','2024-12-29 08:46:51','Hết hạn'),
(17,5,'9788276374824','2024-12-29 08:18:34','2024-12-29 08:48:34','Hết hạn'),
(18,5,'9781234567890','2024-12-29 08:19:43','2024-12-29 08:49:43','Hết hạn'),
(19,5,'9780987654836','2024-12-29 08:22:22','2024-12-29 08:52:22','Hết hạn'),
(20,5,'9735241534232','2024-12-29 08:22:36','2024-12-29 08:52:36','Hết hạn'),
(22,5,'9780987654321','2024-12-29 08:55:22','2024-12-29 08:56:22','Hết hạn'),
(23,5,'9780987654321','2024-12-29 09:14:57','2024-12-29 09:44:57','Hết hạn'),
(24,5,'9780987654321','2024-12-29 09:45:23','2024-12-29 10:15:23','Hết hạn'),
(25,5,'9780987654321','2024-12-29 14:57:10','2024-12-29 15:27:10','Hết hạn'),
(26,1,'9788276374824','2024-12-29 15:29:52','2024-12-29 15:59:52','Hết hạn'),
(27,1,'9781234567890','2024-12-29 17:58:52','2024-12-29 18:28:52','Hết hạn'),
(28,5,'9780987654321','2024-12-30 00:40:07','2024-12-30 01:10:07','Hết hạn'),
(29,5,'9780987654321','2024-12-30 01:11:25','2024-12-30 01:16:25','Hết hạn'),
(30,1,'9781234567890','2024-12-30 08:07:47','2024-12-30 08:12:47','Đang thuê'),
(31,1,'9780987654321','2024-12-30 08:08:07','2024-12-30 08:13:07','Đang thuê');
/*!40000 ALTER TABLE `Rentals` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `RolePermissions`
--

DROP TABLE IF EXISTS `RolePermissions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `RolePermissions` (
  `RoleId` int(11) NOT NULL,
  `PermissionId` int(11) NOT NULL,
  PRIMARY KEY (`RoleId`,`PermissionId`),
  KEY `PermissionId` (`PermissionId`),
  CONSTRAINT `RolePermissions_ibfk_1` FOREIGN KEY (`RoleId`) REFERENCES `Roles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `RolePermissions_ibfk_2` FOREIGN KEY (`PermissionId`) REFERENCES `Permissions` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `RolePermissions`
--

LOCK TABLES `RolePermissions` WRITE;
/*!40000 ALTER TABLE `RolePermissions` DISABLE KEYS */;
INSERT INTO `RolePermissions` VALUES
(1,1),
(1,2),
(1,3),
(1,4),
(1,5),
(2,2),
(2,3),
(2,4),
(2,5),
(3,4),
(3,5);
/*!40000 ALTER TABLE `RolePermissions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Roles`
--

DROP TABLE IF EXISTS `Roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Roles` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `RoleName` varchar(50) NOT NULL,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RoleName` (`RoleName`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Roles`
--

LOCK TABLES `Roles` WRITE;
/*!40000 ALTER TABLE `Roles` DISABLE KEYS */;
INSERT INTO `Roles` VALUES
(1,'Admin',''),
(2,'Manager',''),
(3,'User','');
/*!40000 ALTER TABLE `Roles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `UserAccounts`
--

DROP TABLE IF EXISTS `UserAccounts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `UserAccounts` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` int(11) NOT NULL,
  `Username` varchar(50) NOT NULL,
  `Password` varchar(255) NOT NULL,
  `Email` varchar(255) DEFAULT NULL,
  `CreatedAt` timestamp NULL DEFAULT current_timestamp(),
  `RoleId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Username` (`Username`),
  UNIQUE KEY `Email` (`Email`),
  KEY `UserId` (`UserId`),
  KEY `FK_UserAccounts_Role` (`RoleId`),
  CONSTRAINT `FK_UserAccounts_Role` FOREIGN KEY (`RoleId`) REFERENCES `Roles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `UserAccounts_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `UserAccounts`
--

LOCK TABLES `UserAccounts` WRITE;
/*!40000 ALTER TABLE `UserAccounts` DISABLE KEYS */;
INSERT INTO `UserAccounts` VALUES
(1,1,'admin','0192023a7bbd73250516f069df18b500','admin@gmail.com','2024-11-01 00:42:24',1),
(2,2,'test','16d7a4fca7442dda3ad93c9a726597e4','test@gmail.com','2024-11-01 01:02:40',3),
(4,4,'manager','1d0258c2440a8d19e716292b231e3190','manager@gmail.com','2024-11-07 03:16:39',2),
(5,5,'h1n4m','7b75514c73c9dcff05266c37f77ea7a3','h1n4m@gmail.com','2024-12-26 16:30:22',3);
/*!40000 ALTER TABLE `UserAccounts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Users`
--

DROP TABLE IF EXISTS `Users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Users` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FullName` varchar(255) NOT NULL,
  `DateOfBirth` date DEFAULT NULL,
  `Address` varchar(255) DEFAULT NULL,
  `Phone` varchar(20) DEFAULT NULL,
  `CreatedAt` timestamp NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Users`
--

LOCK TABLES `Users` WRITE;
/*!40000 ALTER TABLE `Users` DISABLE KEYS */;
INSERT INTO `Users` VALUES
(1,'admin','2003-06-11','ha noi','08472387482','2024-11-01 00:42:23'),
(2,'test','2003-06-11','hanoi','09999999999','2024-11-01 01:02:40'),
(4,'Hai Nam Manaager','2003-06-11','236 Hoang Quoc Viet','02387289347','2024-11-07 03:16:39'),
(5,'Nguyen Cong Hai Nam','2003-11-06','236 Hoang Quoc Viet','0834331416','2024-12-26 16:30:21');
/*!40000 ALTER TABLE `Users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `__EFMigrationsHistory`
--

DROP TABLE IF EXISTS `__EFMigrationsHistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__EFMigrationsHistory`
--

LOCK TABLES `__EFMigrationsHistory` WRITE;
/*!40000 ALTER TABLE `__EFMigrationsHistory` DISABLE KEYS */;
/*!40000 ALTER TABLE `__EFMigrationsHistory` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-12-30 13:49:51

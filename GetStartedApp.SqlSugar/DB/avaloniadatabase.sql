/*
 Navicat Premium Dump SQL

 Source Server         : MySQL
 Source Server Type    : MySQL
 Source Server Version : 50719 (5.7.19-log)
 Source Host           : localhost:3306
 Source Schema         : avaloniadatabase

 Target Server Type    : MySQL
 Target Server Version : 50719 (5.7.19-log)
 File Encoding         : 65001

 Date: 18/07/2025 14:49:58
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for base_model_factory_config
-- ----------------------------
DROP TABLE IF EXISTS `base_model_factory_config`;
CREATE TABLE `base_model_factory_config`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Code` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Name` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of base_model_factory_config
-- ----------------------------

-- ----------------------------
-- Table structure for base_model_line_config
-- ----------------------------
DROP TABLE IF EXISTS `base_model_line_config`;
CREATE TABLE `base_model_line_config`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Code` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Name` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `WorkcenterId` int(11) NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of base_model_line_config
-- ----------------------------

-- ----------------------------
-- Table structure for base_model_unit_config
-- ----------------------------
DROP TABLE IF EXISTS `base_model_unit_config`;
CREATE TABLE `base_model_unit_config`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Code` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Name` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `LineId` int(11) NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of base_model_unit_config
-- ----------------------------

-- ----------------------------
-- Table structure for base_model_workcenter_config
-- ----------------------------
DROP TABLE IF EXISTS `base_model_workcenter_config`;
CREATE TABLE `base_model_workcenter_config`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Code` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Name` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `FactoryId` int(11) NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of base_model_workcenter_config
-- ----------------------------

-- ----------------------------
-- Table structure for base_process_config
-- ----------------------------
DROP TABLE IF EXISTS `base_process_config`;
CREATE TABLE `base_process_config`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Code` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Name` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Index` int(11) NOT NULL,
  `RouteId` int(11) NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of base_process_config
-- ----------------------------
INSERT INTO `base_process_config` VALUES (1, 'OP10', '发火体组装工序', 1, 1, NULL, '2022-08-17 16:48:04', '2022-09-24 16:27:43', 'developer', 'developer', NULL);
INSERT INTO `base_process_config` VALUES (2, 'OP20', '产品组装工序', 2, 1, NULL, '2022-08-17 16:49:02', '2022-09-24 16:27:48', 'developer', 'developer', NULL);
INSERT INTO `base_process_config` VALUES (3, 'OP30', '产品检测', 3, 1, NULL, '2022-08-17 16:49:21', '2022-09-24 16:27:51', 'developer', 'developer', NULL);

-- ----------------------------
-- Table structure for base_process_step_config
-- ----------------------------
DROP TABLE IF EXISTS `base_process_step_config`;
CREATE TABLE `base_process_step_config`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Code` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Name` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `UnitCode` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Index` int(11) NOT NULL,
  `StepType` int(11) NOT NULL,
  `IsInPLC` int(11) NOT NULL COMMENT '是否属于PLC数据上报',
  `ProcessId` int(11) NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 46 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of base_process_step_config
-- ----------------------------
INSERT INTO `base_process_step_config` VALUES (1, 'OP10-01', '底座上料', NULL, 1, 1, 1, 1, 'ST1-底座上料', '2022-08-17 16:59:59', '2022-08-17 19:31:36', 'developer', 'developer', NULL);
INSERT INTO `base_process_step_config` VALUES (2, 'OP10-02', '内O型圈上料', NULL, 2, 1, 1, 1, 'ST2-内O型圈上料', '2022-08-17 17:00:43', '2022-08-17 19:31:43', 'developer', 'developer', NULL);
INSERT INTO `base_process_step_config` VALUES (3, 'OP10-03', '内O型圈检测', NULL, 3, 1, 1, 1, 'ST3-内O型圈检测', '2022-08-17 17:01:14', '2022-08-17 19:31:49', 'developer', 'developer', NULL);
INSERT INTO `base_process_step_config` VALUES (4, 'OP10-04', '点火管上料', NULL, 4, 1, 1, 1, 'ST4-点火管上料', '2022-08-17 17:01:43', '2022-08-17 19:31:55', 'developer', 'developer', NULL);
INSERT INTO `base_process_step_config` VALUES (5, 'OP10-06', '发火体收口', NULL, 6, 1, 1, 1, 'ST6-发火体收口', '2022-08-17 17:04:06', '2023-05-09 14:26:59', 'developer', 'developer', NULL);
INSERT INTO `base_process_step_config` VALUES (6, 'OP10-07', '清洗铝屑', NULL, 7, 1, 1, 1, 'ST7-清洗铝屑', '2022-08-17 17:04:35', '2022-08-17 19:32:16', 'developer', 'developer', NULL);
INSERT INTO `base_process_step_config` VALUES (7, 'OP10-08', '循环线', NULL, 8, 1, 1, 1, 'ST8-循环线', '2022-08-17 17:05:01', '2022-08-17 19:32:23', 'developer', 'developer', NULL);
INSERT INTO `base_process_step_config` VALUES (8, 'OP10-09', '收口高度检测', NULL, 9, 1, 1, 1, 'ST9-收口高度检测', '2022-08-17 17:05:33', '2022-08-17 19:32:30', 'developer', 'developer', NULL);
INSERT INTO `base_process_step_config` VALUES (9, 'OP10-10', '外O型圈上料', NULL, 10, 1, 1, 1, 'ST10-外O型圈上料', '2022-08-17 17:05:54', '2022-08-17 19:32:36', 'developer', 'developer', NULL);
INSERT INTO `base_process_step_config` VALUES (10, 'OP10-11', '外O圈检测', NULL, 11, 1, 1, 1, 'ST11-外O圈检测', '2022-08-17 17:06:36', '2022-08-17 19:32:44', 'developer', 'developer', NULL);
INSERT INTO `base_process_step_config` VALUES (11, 'OP10-12', '双工站外观检测', NULL, 12, 1, 1, 1, 'ST12-双工站外观检测', '2022-08-17 17:06:57', '2022-08-17 19:32:51', 'developer', 'developer', NULL);
INSERT INTO `base_process_step_config` VALUES (12, 'OP10-15', '发火体下料', NULL, 15, 1, 1, 1, 'ST15-发火体下料', '2022-08-17 17:07:47', '2022-08-17 19:32:58', 'developer', 'developer', NULL);
INSERT INTO `base_process_step_config` VALUES (13, 'OP20-01', '管壳上料', NULL, 1, 1, 1, 2, 'ST1-管壳上料', '2022-08-17 17:22:30', '2022-08-17 19:33:04', 'developer', 'developer', NULL);
INSERT INTO `base_process_step_config` VALUES (14, 'OP20-02', '管壳称重', NULL, 2, 1, 1, 2, 'ST2-管壳称重', '2022-08-17 17:25:53', '2023-05-09 14:56:23', 'developer', 'developer', NULL);
INSERT INTO `base_process_step_config` VALUES (15, 'OP20-05', '一次加药', NULL, 5, 1, 1, 2, 'ST5-一次加药', '2022-08-17 17:27:11', '2023-05-09 14:56:28', 'developer', 'developer', NULL);
INSERT INTO `base_process_step_config` VALUES (16, 'OP20-07', '一次称重', NULL, 7, 1, 1, 2, 'ST7-一次称重', '2022-08-17 17:28:00', '2023-05-09 14:57:52', 'developer', 'developer', NULL);
INSERT INTO `base_process_step_config` VALUES (17, 'OP20-10', '二次加药', NULL, 10, 1, 1, 2, 'ST10-二次加药', '2022-08-17 17:28:44', '2023-05-09 14:56:42', 'developer', 'developer', NULL);
INSERT INTO `base_process_step_config` VALUES (18, 'OP20-12', '二次称重', NULL, 12, 1, 1, 2, 'ST12-二次称重', '2022-08-17 17:31:12', '2023-05-09 14:57:03', 'developer', 'developer', NULL);
INSERT INTO `base_process_step_config` VALUES (19, 'OP20-13', '输送线下料', NULL, 13, 1, 1, 2, 'ST13-输送线下料', '2022-08-17 17:32:44', '2023-05-09 14:57:11', 'developer', 'developer', NULL);
INSERT INTO `base_process_step_config` VALUES (20, 'OP20-14', '循环线', NULL, 14, 1, 1, 2, 'ST14-循环线', '2022-08-17 17:33:18', '2022-08-17 19:38:19', 'developer', 'developer', NULL);
INSERT INTO `base_process_step_config` VALUES (21, 'OP20-15', '回流线上料', NULL, 15, 1, 1, 2, 'ST15-回流线上料', '2022-08-17 17:35:37', '2023-05-09 14:58:01', 'developer', 'developer', NULL);
INSERT INTO `base_process_step_config` VALUES (22, 'OP20-18', '发火体上料', NULL, 18, 1, 1, 2, 'ST18-发火体上料', '2022-08-17 17:43:05', '2022-08-17 19:41:53', 'developer', 'developer', NULL);
INSERT INTO `base_process_step_config` VALUES (23, 'OP20-19', '内O外漏检测', NULL, 19, 1, 1, 2, 'ST19-内O外漏检测', '2022-08-17 19:42:40', NULL, 'developer', NULL, NULL);
INSERT INTO `base_process_step_config` VALUES (24, 'OP20-20', '管壳与电极针矫正', NULL, 20, 1, 1, 2, 'ST20-管壳与电极针矫正', '2022-08-17 19:44:31', NULL, 'developer', NULL, NULL);
INSERT INTO `base_process_step_config` VALUES (25, 'OP20-21', 'MGG收口', NULL, 21, 1, 1, 2, 'ST21-MGG收口', '2022-08-17 19:45:11', NULL, 'developer', NULL, NULL);
INSERT INTO `base_process_step_config` VALUES (26, 'OP20-22', '去铝屑及上料', NULL, 22, 1, 1, 2, 'ST22-去铝屑及上料', '2022-08-17 19:47:13', '2022-08-17 19:51:51', 'developer', 'developer', NULL);
INSERT INTO `base_process_step_config` VALUES (28, 'OP20-24 ', '平行度检测', NULL, 24, 1, 1, 2, 'ST24-平行度检测', NULL, NULL, NULL, NULL, NULL);
INSERT INTO `base_process_step_config` VALUES (29, 'OP20-25', '收口外径检测', NULL, 25, 1, 1, 2, 'ST25-收口外径检测', NULL, NULL, NULL, NULL, NULL);
INSERT INTO `base_process_step_config` VALUES (30, 'OP20-27', '高度检测', NULL, 27, 1, 1, 2, 'ST27-高度检测', NULL, NULL, NULL, NULL, NULL);
INSERT INTO `base_process_step_config` VALUES (31, 'OP30-01', '产品上料', NULL, 1, 1, 1, 3, 'ST1-产品上料', '2022-08-17 19:53:42', NULL, 'developer', NULL, NULL);
INSERT INTO `base_process_step_config` VALUES (32, 'OP30-05', '电极针校正', NULL, 5, 1, 1, 3, 'ST5-电极针校正', '2022-08-17 19:55:30', NULL, 'developer', NULL, NULL);
INSERT INTO `base_process_step_config` VALUES (33, 'OP30-06', '短路组件装配', NULL, 6, 1, 1, 3, 'ST6-短路组件装配', '2022-08-17 19:56:05', NULL, 'developer', NULL, NULL);
INSERT INTO `base_process_step_config` VALUES (34, 'OP30-08', '桥路组件测高', NULL, 8, 1, 1, 3, 'ST8-桥路组件测高', '2022-08-17 19:56:39', NULL, 'developer', NULL, NULL);
INSERT INTO `base_process_step_config` VALUES (35, 'OP30-09', '桥路测试', NULL, 9, 1, 1, 3, 'ST9-桥路测试', '2022-08-17 19:57:12', NULL, 'developer', NULL, NULL);
INSERT INTO `base_process_step_config` VALUES (36, 'OP30-10', '循环线', NULL, 10, 1, 1, 3, 'ST10-循环线', '2022-08-17 19:58:24', NULL, 'developer', NULL, NULL);
INSERT INTO `base_process_step_config` VALUES (37, 'OP30-11', '绝缘电测', NULL, 11, 1, 1, 3, 'ST11-绝缘电测', '2022-08-17 19:57:45', NULL, 'developer', NULL, NULL);
INSERT INTO `base_process_step_config` VALUES (38, 'OP30-12', '短路检测', NULL, 12, 1, 1, 3, 'ST12-短路检测', '2022-08-17 19:58:52', NULL, 'developer', NULL, NULL);
INSERT INTO `base_process_step_config` VALUES (39, 'OP30-13', '一维码刻印', NULL, 13, 1, 1, 3, 'ST13-一维码刻印', '2022-08-17 19:59:28', NULL, 'developer', NULL, NULL);
INSERT INTO `base_process_step_config` VALUES (40, 'OP30-14', '外观终检', NULL, 14, 1, 1, 3, 'ST14-外观终检', '2022-08-17 19:59:28', NULL, 'developer', NULL, NULL);
INSERT INTO `base_process_step_config` VALUES (41, 'OP30-15', '二维码刻印', NULL, 15, 1, 1, 3, 'ST15-二维码刻印', '2022-08-17 19:59:28', NULL, 'developer', NULL, NULL);
INSERT INTO `base_process_step_config` VALUES (42, 'OP30-16', '二维码扫码', NULL, 16, 1, 1, 3, 'ST16-二维码扫码', '2022-08-17 20:00:10', NULL, 'developer', NULL, NULL);
INSERT INTO `base_process_step_config` VALUES (43, 'OP30-17', '不良品下料', NULL, 17, 1, 1, 3, 'ST17-不良品下料', '2022-08-17 20:00:44', NULL, 'developer', NULL, NULL);
INSERT INTO `base_process_step_config` VALUES (44, 'OP30-19', '良品下料', NULL, 19, 1, 1, 3, 'ST19-良品下料', '2022-08-17 20:01:34', NULL, 'developer', NULL, NULL);
INSERT INTO `base_process_step_config` VALUES (45, 'OP30-20', '料仓', NULL, 20, 1, 1, 3, 'ST20-料仓', '2022-08-17 20:02:09', NULL, 'developer', NULL, NULL);

-- ----------------------------
-- Table structure for base_route_config
-- ----------------------------
DROP TABLE IF EXISTS `base_route_config`;
CREATE TABLE `base_route_config`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Code` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Name` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `VersionSecondId` int(11) NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of base_route_config
-- ----------------------------
INSERT INTO `base_route_config` VALUES (1, '59-1', '59-1工艺', 1, NULL, '2023-03-06 11:39:11', NULL, 'developer', NULL, NULL);

-- ----------------------------
-- Table structure for base_route_processstep_config
-- ----------------------------
DROP TABLE IF EXISTS `base_route_processstep_config`;
CREATE TABLE `base_route_processstep_config`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `RouteId` int(11) NOT NULL,
  `ProcessStepId` int(11) NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 45 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of base_route_processstep_config
-- ----------------------------
INSERT INTO `base_route_processstep_config` VALUES (1, 1, 1, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (2, 1, 2, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (3, 1, 3, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (4, 1, 4, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (5, 1, 5, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (6, 1, 6, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (7, 1, 7, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (8, 1, 8, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (9, 1, 9, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (10, 1, 10, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (11, 1, 11, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (12, 1, 12, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (13, 1, 13, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (14, 1, 14, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (15, 1, 15, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (16, 1, 16, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (17, 1, 17, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (18, 1, 18, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (19, 1, 19, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (20, 1, 20, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (21, 1, 21, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (22, 1, 22, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (23, 1, 23, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (24, 1, 24, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (25, 1, 25, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (26, 1, 26, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (27, 1, 27, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (28, 1, 28, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (29, 1, 29, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (30, 1, 30, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (31, 1, 31, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (32, 1, 32, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (33, 1, 33, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (34, 1, 34, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (35, 1, 35, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (36, 1, 36, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (37, 1, 37, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (38, 1, 38, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (39, 1, 39, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (40, 1, 40, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (41, 1, 41, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (42, 1, 42, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (43, 1, 43, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);
INSERT INTO `base_route_processstep_config` VALUES (44, 1, 44, NULL, '2023-05-04 17:44:08', NULL, 'developer', NULL, NULL);

-- ----------------------------
-- Table structure for base_version_attribute_config
-- ----------------------------
DROP TABLE IF EXISTS `base_version_attribute_config`;
CREATE TABLE `base_version_attribute_config`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Code` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Name` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `AttributeType` int(11) NOT NULL COMMENT '属性类型',
  `ValueType` int(11) NOT NULL COMMENT '值类型',
  `Value` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Target` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '映射目标',
  `SecondId` int(11) NOT NULL,
  `StepId` int(11) NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 29 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of base_version_attribute_config
-- ----------------------------
INSERT INTO `base_version_attribute_config` VALUES (1, 'PFSY', '配方索引', 1, 1, '1', 'ChangedRecipeNo', 1, 1, '当前型号对应PLC的配方索引', '2023-03-06 11:38:14', NULL, 'developer', NULL, NULL);
INSERT INTO `base_version_attribute_config` VALUES (2, 'YWM-GSDH', '公司代号', 1, 4, 'QH', 'Undefined', 1, 39, NULL, '2023-03-06 11:38:14', '2023-05-09 15:17:01', 'developer', 'developer', NULL);
INSERT INTO `base_version_attribute_config` VALUES (3, 'YWM-CPMC', '产品名称', 1, 4, '059', 'Undefined', 1, 39, NULL, '2023-03-06 11:38:14', '2023-05-17 10:42:19', 'developer', 'developer', NULL);
INSERT INTO `base_version_attribute_config` VALUES (4, 'YWM-CPXH', '产品型号', 1, 4, '1', 'Undefined', 1, 39, NULL, '2023-03-06 11:38:14', '2023-05-19 18:28:11', 'developer', 'developer', NULL);
INSERT INTO `base_version_attribute_config` VALUES (5, 'YWM-YH', '年号', 1, 4, '{yy}', 'Undefined', 1, 39, '年号必须用{}包起来，使用c#中解析方式', '2023-03-06 11:38:14', '2023-05-09 15:17:18', 'developer', 'developer', NULL);
INSERT INTO `base_version_attribute_config` VALUES (6, 'YWM-CPPH', '产品批号', 1, 4, 'A12', 'Undefined', 1, 39, '', '2023-03-06 11:38:14', '2023-05-09 15:17:25', 'developer', 'developer', NULL);
INSERT INTO `base_version_attribute_config` VALUES (7, 'YWM-CXH', '产线号', 1, 4, 'E', 'Undefined', 1, 39, NULL, '2023-03-06 11:38:14', '2023-05-09 15:17:32', 'developer', 'developer', NULL);
INSERT INTO `base_version_attribute_config` VALUES (8, 'YWM-CPXLH', '产品序列号', 1, 4, '{5}', 'Undefined', 1, 39, '用{}包起来，里面数字表示需要的位数', '2023-03-06 11:38:14', '2023-05-09 15:17:38', 'developer', 'developer', NULL);
INSERT INTO `base_version_attribute_config` VALUES (9, 'GK-LARSER', '管壳镭射', 1, 4, 'B-JG-GK-EWM', 'Undefined', 1, 41, '管壳二维码镭射', '2023-03-06 11:38:14', '2023-05-09 15:17:53', 'developer', 'developer', NULL);
INSERT INTO `base_version_attribute_config` VALUES (10, 'EWMLX', '二维码类型', 1, 4, '类型Ⅰ', 'Undefined', 1, 41, '二维码有两种类型29位(类型Ⅰ)和24位(类型Ⅱ)和固定码(类型Ⅲ)', '2023-03-06 11:38:14', '2023-05-17 10:02:29', 'developer', 'developer', NULL);
INSERT INTO `base_version_attribute_config` VALUES (11, 'EWM-1-SCRQ', '生产日期', 1, 4, '{yyMMdd}', 'Undefined', 1, 41, NULL, '2023-03-06 11:38:14', '2023-05-09 15:18:06', 'developer', 'developer', NULL);
INSERT INTO `base_version_attribute_config` VALUES (12, 'EWM-1-XLH', '序列号', 1, 4, '{5}', 'Undefined', 1, 41, NULL, '2023-03-06 11:38:14', '2023-05-09 15:18:10', 'developer', 'developer', NULL);
INSERT INTO `base_version_attribute_config` VALUES (13, 'EWM-1-CPXH', '产品型号', 1, 4, 'E0591', 'Undefined', 1, 41, '线号+产品型号(4位)例：E0591', '2023-03-06 11:38:14', '2023-05-22 09:44:35', 'developer', 'developer', NULL);
INSERT INTO `base_version_attribute_config` VALUES (14, 'EWM-1-KHDM', '客户代码', 1, 4, '2589634591597', 'Undefined', 1, 41, '13位客户代码', '2023-03-06 11:38:14', '2023-05-09 15:18:21', 'developer', 'developer', NULL);
INSERT INTO `base_version_attribute_config` VALUES (15, 'EWM-1-FCZFC', '防错字符串', 1, 4, '05901', 'Undefined', 1, 41, '产品代号（3 位数字）＋型号（2 位数字）', '2023-03-06 11:38:14', '2023-05-17 10:43:07', 'developer', 'developer', NULL);
INSERT INTO `base_version_attribute_config` VALUES (16, 'EWM-2-SCRQ', '生产日期', 1, 4, '{yyDDdd}', 'Undefined', 1, 41, NULL, '2023-03-06 11:38:14', '2023-05-09 15:19:08', 'developer', 'developer', NULL);
INSERT INTO `base_version_attribute_config` VALUES (17, 'EWM-2-XLH', '序列号', 1, 4, '{5}', 'Undefined', 1, 41, '序列号用{}包起来，里面数字表示长度', '2023-03-06 11:38:14', '2023-05-09 15:19:01', 'developer', 'developer', NULL);
INSERT INTO `base_version_attribute_config` VALUES (18, 'EWM-2-CXH', '产线号', 1, 4, 'E1', 'Undefined', 1, 41, NULL, '2023-03-06 11:38:14', '2023-05-09 15:18:55', 'developer', 'developer', NULL);
INSERT INTO `base_version_attribute_config` VALUES (19, 'EWM-2-LJH', '零件号', 1, 4, '34199291B', 'Undefined', 1, 41, NULL, '2023-03-06 11:38:14', '2023-05-09 15:18:34', 'developer', 'developer', NULL);
INSERT INTO `base_version_attribute_config` VALUES (20, 'EWM-2-GSDM', '公司代码', 1, 4, 'QH', 'Undefined', 1, 41, NULL, '2023-03-06 11:38:14', '2023-05-09 15:18:39', 'developer', 'developer', NULL);
INSERT INTO `base_version_attribute_config` VALUES (21, 'EWM-2-FCZFC', '防错字符串', 1, 4, '05901', 'Undefined', 1, 41, '产品代号（3 位数字）＋型号（2 位数字）', '2023-03-06 11:38:14', '2023-05-09 15:18:48', 'developer', 'developer', NULL);
INSERT INTO `base_version_attribute_config` VALUES (22, 'EWM-3-GDM', '固定码', 1, 4, '05901', 'Undefined', 1, 41, NULL, '2023-05-16 10:14:12', NULL, 'developer', NULL, NULL);
INSERT INTO `base_version_attribute_config` VALUES (23, 'Material-DZ', '底座', 1, 4, '1853A1012|4,1&3', 'Undefined', 1, 1, '物料编码|分隔符个数,料号位置&批号位置', '2023-03-06 11:38:14', NULL, 'developer', NULL, NULL);
INSERT INTO `base_version_attribute_config` VALUES (24, 'Material-NO', '内O', 1, 4, '185201031|2,0&1', 'Undefined', 1, 2, '物料编码|分隔符个数,料号位置&批号位置', '2023-03-06 11:38:14', NULL, 'developer', NULL, NULL);
INSERT INTO `base_version_attribute_config` VALUES (25, 'Material-DHG', '点火管', 1, 4, '1008A0006|3,0&1', 'Undefined', 1, 4, '物料编码|分隔符个数,料号位置&批号位置', '2023-03-06 11:38:14', NULL, 'developer', NULL, NULL);
INSERT INTO `base_version_attribute_config` VALUES (26, 'Material-WO', '外O', 1, 4, '185201041|5,1&3', 'Undefined', 1, 9, '物料编码|分隔符个数,料号位置&批号位置', '2023-03-06 11:38:14', '2023-05-04 17:49:10', 'developer', 'developer', NULL);
INSERT INTO `base_version_attribute_config` VALUES (27, 'Material-GK', '管壳', 1, 4, '601701011|5,1&3', 'Undefined', 1, 13, '物料编码|分隔符个数,料号位置&批号位置', '2023-03-06 11:38:14', '2023-05-09 15:19:25', 'developer', 'developer', NULL);
INSERT INTO `base_version_attribute_config` VALUES (28, 'Material-DLZJ', '短路组件', 1, 4, '700201001|5,1&3', 'Undefined', 1, 33, '物料编码|分隔符个数,料号位置&批号位置', '2023-03-06 11:38:14', '2023-05-09 15:19:37', 'developer', 'developer', NULL);

-- ----------------------------
-- Table structure for base_version_primary_config
-- ----------------------------
DROP TABLE IF EXISTS `base_version_primary_config`;
CREATE TABLE `base_version_primary_config`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Code` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Name` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of base_version_primary_config
-- ----------------------------
INSERT INTO `base_version_primary_config` VALUES (1, '59', '59号MGG', NULL, '2023-03-06 11:37:52', '2023-05-10 13:02:29', 'developer', 'developer', NULL);

-- ----------------------------
-- Table structure for base_version_second_config
-- ----------------------------
DROP TABLE IF EXISTS `base_version_second_config`;
CREATE TABLE `base_version_second_config`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Code` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Name` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `VersionPrimaryId` int(11) NOT NULL,
  `RouteId` int(11) NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of base_version_second_config
-- ----------------------------
INSERT INTO `base_version_second_config` VALUES (1, 'Ⅰ', '1型', 1, 1, NULL, '2023-03-06 11:38:14', NULL, 'developer', NULL, NULL);

-- ----------------------------
-- Table structure for beckhoff_event_log
-- ----------------------------
DROP TABLE IF EXISTS `beckhoff_event_log`;
CREATE TABLE `beckhoff_event_log`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Idx` int(11) NOT NULL,
  `Name` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ReadLabel` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ReadLen` smallint(6) NOT NULL,
  `ReadClassName` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `WirteLabel` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `WirteLen` smallint(6) NOT NULL,
  `WirteClassName` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `TriggerCompleted` bit(1) NOT NULL,
  `SequenceIDR` int(11) NOT NULL,
  `SequenceIDW` int(11) NOT NULL,
  `ObjR` text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ObjW` text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `StartTime` datetime NOT NULL,
  `EndTime` datetime NOT NULL,
  `SpanTime` double NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of beckhoff_event_log
-- ----------------------------

-- ----------------------------
-- Table structure for data_op10
-- ----------------------------
DROP TABLE IF EXISTS `data_op10`;
CREATE TABLE `data_op10`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TaskId` int(11) NOT NULL COMMENT '工单ID',
  `AppendPartCode` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '底座逻辑编码',
  `Batch` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '批次',
  `PartOK` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '主产品结果',
  `OP10_ST1_DizuoHigh` float NULL DEFAULT NULL COMMENT '底座高度',
  `OP10_ST1_DizuoHighOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '底座高度结果',
  `OP10_ST2_NeiODia` float NULL DEFAULT NULL COMMENT '内O型圈直径',
  `OP10_ST3_NeiOThick` float NULL DEFAULT NULL COMMENT '内O厚度',
  `OP10_ST3_NeiOThickOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '内O厚度结果',
  `OP10_ST5_ST6_ST` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '发火体工位',
  `OP10_ST5_ST6_FahuotiPres` float NULL DEFAULT NULL COMMENT '发火体收口压力',
  `OP10_ST5_ST6_FahuotiPressOk` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '发火体收口压力结果',
  `OP10_ST7_FahuotiHigh` float NULL DEFAULT NULL COMMENT '发火体收口高度',
  `OP10_ST7_FahuotiHighOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '发火体收口高度结果',
  `OP10_ST10_WaiOThick` float NULL DEFAULT NULL COMMENT '外O型圈厚度',
  `OP10_ST10_WaiOThickOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '外O型圈厚度结果',
  `OP10_ST11_CCDOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '顶部相机检测结果',
  `OP10_ST12_CCDOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '侧面相机检测结果',
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of data_op10
-- ----------------------------

-- ----------------------------
-- Table structure for data_op10_tmp
-- ----------------------------
DROP TABLE IF EXISTS `data_op10_tmp`;
CREATE TABLE `data_op10_tmp`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TaskId` int(11) NOT NULL COMMENT '工单ID',
  `AppendPartCode` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '底座逻辑编码',
  `Batch` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '批次',
  `PartOK` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '主产品结果',
  `OP10_ST1_DizuoHigh` float NULL DEFAULT NULL COMMENT '底座高度',
  `OP10_ST1_DizuoHighOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '底座高度结果',
  `OP10_ST2_NeiODia` float NULL DEFAULT NULL COMMENT '内O型圈直径',
  `OP10_ST3_NeiOThick` float NULL DEFAULT NULL COMMENT '内O厚度',
  `OP10_ST3_NeiOThickOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '内O厚度结果',
  `OP10_ST5_ST6_ST` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '发火体工位',
  `OP10_ST5_ST6_FahuotiPres` float NULL DEFAULT NULL COMMENT '发火体收口压力',
  `OP10_ST5_ST6_FahuotiPressOk` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '发火体收口压力结果',
  `OP10_ST7_FahuotiHigh` float NULL DEFAULT NULL COMMENT '发火体收口高度',
  `OP10_ST7_FahuotiHighOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '发火体收口高度结果',
  `OP10_ST10_WaiOThick` float NULL DEFAULT NULL COMMENT '外O型圈厚度',
  `OP10_ST10_WaiOThickOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '外O型圈厚度结果',
  `OP10_ST11_CCDOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '顶部相机检测结果',
  `OP10_ST12_CCDOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '侧面相机检测结果',
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of data_op10_tmp
-- ----------------------------

-- ----------------------------
-- Table structure for data_quality
-- ----------------------------
DROP TABLE IF EXISTS `data_quality`;
CREATE TABLE `data_quality`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TaskId` int(11) NOT NULL COMMENT '工单ID',
  `PartCode` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '管壳逻辑编码',
  `Barcode` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '字符编码',
  `Batch` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '批次',
  `PartOK` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '产品结果',
  `OP10_ST1_DizuoHigh` float NULL DEFAULT NULL COMMENT '底座高度',
  `OP10_ST1_DizuoHighOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '底座高度结果',
  `OP10_ST3_NeiOThick` float NULL DEFAULT NULL COMMENT '内O厚度',
  `OP10_ST3_NeiOThickOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '内O厚度结果',
  `OP10_ST5_ST6_ST` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '发火体工位',
  `OP10_ST5_ST6_FahuotiPres` float NULL DEFAULT NULL COMMENT '发火体收口压力',
  `OP10_ST5_ST6_FahuotiPressOk` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '发火体收口压力结果',
  `OP10_ST7_FahuotiHigh` float NULL DEFAULT NULL COMMENT '发火体收口高度',
  `OP10_ST7_FahuotiHighOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '发火体收口高度结果',
  `OP10_ST10_WaiOThick` float NULL DEFAULT NULL COMMENT '外O型圈厚度',
  `OP10_ST10_WaiOThickOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '外O型圈厚度结果',
  `OP10_ST11_CCDOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '顶部相机检测结果',
  `OP10_ST12_CCDOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '侧面相机检测结果',
  `OP20_ST2_TubeHigh` float NULL DEFAULT NULL COMMENT '管壳高度',
  `OP20_ST2_TubeHighOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '管壳高度结果',
  `OP20_ST3_TubeWeigh` float NULL DEFAULT NULL COMMENT '管壳重量',
  `OP20_ST3_TubeWeighOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '管壳重量结果',
  `OP20_ST5_RecipeNo` int(11) NULL DEFAULT NULL COMMENT '一次加配方编号',
  `OP20_ST5_WeighNo` int(11) NULL DEFAULT NULL COMMENT '一次加药称编号',
  `OP20_ST5_PowerWeigh1` float NULL DEFAULT NULL COMMENT '一次加药重量',
  `OP20_ST5_Temperature` float NULL DEFAULT NULL COMMENT '一次加药机温度',
  `OP20_ST5_Humidity` float NULL DEFAULT NULL COMMENT '一次加药机湿度',
  `OP20_ST5_AddTime` float NULL DEFAULT NULL COMMENT '一次加药机加药时间',
  `OP20_ST7_PowerWeigh1` float NULL DEFAULT NULL COMMENT '一次加药重量',
  `OP20_ST7_PowerWeigh1OK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '一次加药重量结果',
  `OP20_ST9_RecipeNo` int(11) NULL DEFAULT NULL COMMENT '二次加配方编号',
  `OP20_ST9_WeighNo` int(11) NULL DEFAULT NULL COMMENT '二次加药称编号',
  `OP20_ST9_PowerWeigh2` float NULL DEFAULT NULL COMMENT '二次加药重量',
  `OP20_ST9_Temperature` float NULL DEFAULT NULL COMMENT '二次加药机温度',
  `OP20_ST9_Humidity` float NULL DEFAULT NULL COMMENT '二次加药机湿度',
  `OP20_ST9_AddTime` float NULL DEFAULT NULL COMMENT '二次加药机加药时间',
  `OP20_ST12_PowerWeigh2` float NULL DEFAULT NULL COMMENT '二次加药重量',
  `OP20_ST12_PowerWeigh2OK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '二次加药重量结果',
  `AppendPartCode` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '副产品编码',
  `OP20_ST19_CCDOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT 'CCD结果',
  `OP20_ST20_DJZHigh` float NULL DEFAULT NULL COMMENT '电极针高度',
  `OP20_ST20_DJZHighOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '电极针结果',
  `OP20_ST22_MGGpress` float NULL DEFAULT NULL COMMENT '成品收口压力',
  `OP20_ST22_MGGpressOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '成品收口压力结果',
  `OP20_ST24_CorrectAngle` float NULL DEFAULT NULL COMMENT '平行度',
  `OP20_ST24_CorrectAngleOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '平行度结果',
  `OP20_ST25_MggDia` float NULL DEFAULT NULL COMMENT '成品收口外径',
  `OP20_ST25_MggDiaOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '成品收口外径结果',
  `OP20_ST27_MggPressHigh` float NULL DEFAULT NULL COMMENT '成品收口高度',
  `OP20_ST27_MggPressHighOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '成品收口高度结果',
  `OP20_ST27_MggTotalHigh` float NULL DEFAULT NULL COMMENT '总高',
  `OP20_ST27_MggTotalHighOk` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '总高结果',
  `OP30_ST6_Pressure` float NULL DEFAULT NULL COMMENT '短路夹压力',
  `OP30_ST6_PressureOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '短路夹压力结果',
  `OP30_ST8_High` float NULL DEFAULT NULL COMMENT '短路夹高度',
  `OP30_ST8_HighOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '短路夹高度果',
  `OP30_ST9_Machine` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '桥路检测仪器',
  `OP30_ST9_BridgeR` float NULL DEFAULT NULL COMMENT '桥路电阻',
  `OP30_ST9_BridgeROK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '桥路电阻结果',
  `OP30_ST11_Machine` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '绝缘检测仪器',
  `OP30_ST11_InslationR` float NULL DEFAULT NULL COMMENT '绝缘电阻',
  `OP30_ST11_InslationRROK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '绝缘电阻结果',
  `OP30_ST12_Machine` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '短路检测仪器',
  `OP30_ST12_ShortR` float NULL DEFAULT NULL COMMENT '短路电阻',
  `OP30_ST12_ShortROK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '短路电阻结果',
  `OP30_ST13_Machine` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '激光机',
  `OP30_ST13_Barcode` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '一维码刻印',
  `OP30_ST14_Barcode` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '扫出一维码',
  `OP30_ST14_DLJ` float NULL DEFAULT NULL COMMENT '短路夹间隙',
  `OP30_ST14_DLJOk` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '相机短路夹拍照结果',
  `OP30_ST15_PartCode` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '刻印二维码',
  `OP30_ST15_Machine` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '激光机',
  `OP30_ST16_PartCode` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '扫出二维码',
  `OP30_ST16_PartCodeOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '扫出二维码结果',
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of data_quality
-- ----------------------------

-- ----------------------------
-- Table structure for data_quality_tmp
-- ----------------------------
DROP TABLE IF EXISTS `data_quality_tmp`;
CREATE TABLE `data_quality_tmp`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TaskId` int(11) NOT NULL COMMENT '工单ID',
  `PartCode` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '管壳逻辑编码',
  `Barcode` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '字符编码',
  `Batch` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '批次',
  `PartOK` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '产品结果',
  `OP10_ST1_DizuoHigh` float NULL DEFAULT NULL COMMENT '底座高度',
  `OP10_ST1_DizuoHighOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '底座高度结果',
  `OP10_ST2_NeiODia` float NULL DEFAULT NULL COMMENT '内O型圈直径',
  `OP10_ST3_NeiOThick` float NULL DEFAULT NULL COMMENT '内O厚度',
  `OP10_ST3_NeiOThickOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '内O厚度结果',
  `OP10_ST5_ST6_ST` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '发火体工位',
  `OP10_ST5_ST6_FahuotiPres` float NULL DEFAULT NULL COMMENT '发火体收口压力',
  `OP10_ST5_ST6_FahuotiPressOk` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '发火体收口压力结果',
  `OP10_ST7_FahuotiHigh` float NULL DEFAULT NULL COMMENT '发火体收口高度',
  `OP10_ST7_FahuotiHighOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '发火体收口高度结果',
  `OP10_ST10_WaiOThick` float NULL DEFAULT NULL COMMENT '外O型圈厚度',
  `OP10_ST10_WaiOThickOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '外O型圈厚度结果',
  `OP10_ST11_CCDOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '顶部相机检测结果',
  `OP10_ST12_CCDOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '侧面相机检测结果',
  `OP20_ST2_TubeHigh` float NULL DEFAULT NULL COMMENT '管壳高度',
  `OP20_ST2_TubeHighOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '管壳高度结果',
  `OP20_ST3_TubeWeigh` float NULL DEFAULT NULL COMMENT '管壳重量',
  `OP20_ST3_TubeWeighOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '管壳重量结果',
  `OP20_ST5_RecipeNo` int(11) NULL DEFAULT NULL COMMENT '一次加配方编号',
  `OP20_ST5_WeighNo` int(11) NULL DEFAULT NULL COMMENT '一次加药称编号',
  `OP20_ST5_PowerWeigh1` float NULL DEFAULT NULL COMMENT '一次加药重量',
  `OP20_ST5_Temperature` float NULL DEFAULT NULL COMMENT '一次加药机温度',
  `OP20_ST5_Humidity` float NULL DEFAULT NULL COMMENT '一次加药机湿度',
  `OP20_ST5_AddTime` float NULL DEFAULT NULL COMMENT '一次加药机加药时间',
  `OP20_ST7_PowerWeigh1` float NULL DEFAULT NULL COMMENT '一次加药重量',
  `OP20_ST7_PowerWeigh1OK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '一次加药重量结果',
  `OP20_ST9_RecipeNo` int(11) NULL DEFAULT NULL COMMENT '二次加配方编号',
  `OP20_ST9_WeighNo` int(11) NULL DEFAULT NULL COMMENT '二次加药称编号',
  `OP20_ST9_PowerWeigh2` float NULL DEFAULT NULL COMMENT '二次加药重量',
  `OP20_ST9_Temperature` float NULL DEFAULT NULL COMMENT '二次加药机温度',
  `OP20_ST9_Humidity` float NULL DEFAULT NULL COMMENT '二次加药机湿度',
  `OP20_ST9_AddTime` float NULL DEFAULT NULL COMMENT '二次加药机加药时间',
  `OP20_ST12_PowerWeigh2` float NULL DEFAULT NULL COMMENT '二次加药重量',
  `OP20_ST12_PowerWeigh2OK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '二次加药重量结果',
  `AppendPartCode` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '副产品编码',
  `OP20_ST19_CCDOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT 'CCD结果',
  `OP20_ST20_DJZHigh` float NULL DEFAULT NULL COMMENT '电极针高度',
  `OP20_ST20_DJZHighOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '电极针结果',
  `OP20_ST22_MGGpress` float NULL DEFAULT NULL COMMENT '成品收口压力',
  `OP20_ST22_MGGpressOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '成品收口压力结果',
  `OP20_ST24_CorrectAngle` float NULL DEFAULT NULL COMMENT '平行度',
  `OP20_ST24_CorrectAngleOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '平行度结果',
  `OP20_ST25_MggDia` float NULL DEFAULT NULL COMMENT '成品收口外径',
  `OP20_ST25_MggDiaOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '成品收口外径结果',
  `OP20_ST27_MggPressHigh` float NULL DEFAULT NULL COMMENT '成品收口高度',
  `OP20_ST27_MggPressHighOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '成品收口高度结果',
  `OP20_ST27_MggTotalHigh` float NULL DEFAULT NULL COMMENT '总高',
  `OP20_ST27_MggTotalHighOk` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '总高结果',
  `OP30_ST6_Pressure` float NULL DEFAULT NULL COMMENT '短路夹压力',
  `OP30_ST6_PressureOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '短路夹压力结果',
  `OP30_ST8_High` float NULL DEFAULT NULL COMMENT '短路夹高度',
  `OP30_ST8_HighOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '短路夹高度果',
  `OP30_ST9_Machine` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '桥路检测仪器',
  `OP30_ST9_BridgeR` float NULL DEFAULT NULL COMMENT '桥路电阻',
  `OP30_ST9_BridgeROK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '桥路电阻结果',
  `OP30_ST11_Machine` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '绝缘检测仪器',
  `OP30_ST11_InslationR` float NULL DEFAULT NULL COMMENT '绝缘电阻',
  `OP30_ST11_InslationRROK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '绝缘电阻结果',
  `OP30_ST12_Machine` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '短路检测仪器',
  `OP30_ST12_ShortR` float NULL DEFAULT NULL COMMENT '短路电阻',
  `OP30_ST12_ShortROK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '短路电阻结果',
  `OP30_ST13_Machine` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '激光机',
  `OP30_ST13_Barcode` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '一维码刻印',
  `OP30_ST14_Barcode` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '扫出一维码',
  `OP30_ST14_DLJ` float NULL DEFAULT NULL COMMENT '短路夹间隙',
  `OP30_ST14_DLJOk` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '相机短路夹拍照结果',
  `OP30_ST15_PartCode` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '刻印二维码',
  `OP30_ST15_Machine` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '激光机',
  `OP30_ST16_PartCode` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '扫出二维码',
  `OP30_ST16_PartCodeOK` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '扫出二维码结果',
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of data_quality_tmp
-- ----------------------------

-- ----------------------------
-- Table structure for device_carrier_config
-- ----------------------------
DROP TABLE IF EXISTS `device_carrier_config`;
CREATE TABLE `device_carrier_config`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Code` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Part01` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '第一个位置产品SN',
  `Part02` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '第二个位置产品SN',
  `OkCount` int(11) NOT NULL,
  `NgCount` int(11) NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 63 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of device_carrier_config
-- ----------------------------
INSERT INTO `device_carrier_config` VALUES (1, 'OP10_01#', 'OP10_01#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:19:26', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (2, 'OP10_02#', 'OP10_02#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:19:52', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (3, 'OP10_03#', 'OP10_03#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:20:16', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (4, 'OP10_04#', 'OP10_04#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:20:34', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (5, 'OP10_05#', 'OP10_05#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:23:06', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (6, 'OP10_06#', 'OP10_06#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:23:27', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (7, 'OP10_07#', 'OP10_07#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:23:50', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (8, 'OP10_08#', 'OP10_08#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:24:14', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (9, 'OP10_09#', 'OP10_09#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:24:37', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (10, 'OP10_10#', 'OP10_10#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:24:50', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (11, 'OP10_11#', 'OP10_11#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:25:06', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (12, 'OP10_12#', 'OP10_12#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:25:18', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (13, 'OP10_13#', 'OP10_13#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:25:28', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (14, 'OP10_14#', 'OP10_14#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:25:46', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (15, 'OP10_15#', 'OP10_15#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:26:02', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (16, 'OP20_01#', 'OP20_01#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:26:41', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (17, 'OP20_02#', 'OP20_02#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:26:54', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (18, 'OP20_03#', 'OP20_03#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:27:05', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (19, 'OP20_04#', 'OP20_04#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:27:21', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (20, 'OP20_05#', 'OP20_05#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:27:35', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (21, 'OP20_06#', 'OP20_06#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:27:49', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (22, 'OP20_07#', 'OP20_07#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:28:03', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (23, 'OP20_08#', 'OP20_08#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:28:16', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (24, 'OP20_09#', 'OP20_09#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:28:50', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (25, 'OP20_10#', 'OP20_10#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:29:15', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (26, 'OP20_11#', 'OP20_11#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:29:32', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (27, 'OP20_12#', 'OP20_12#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:29:50', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (28, 'OP20_13#', 'OP20_13#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:30:10', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (29, 'OP20_14#', 'OP20_14#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:30:21', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (30, 'OP20_15#', 'OP20_15#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:30:43', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (31, 'OP20_16#', 'OP20_16#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:30:53', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (32, 'OP20_17#', 'OP20_17#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:31:27', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (33, 'OP20_18#', 'OP20_18#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:31:43', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (34, 'OP20_19#', 'OP20_19#', 'GK-000000000000038899', 'GK-000000000000038900', 0, 0, NULL, '2023-03-06 14:31:53', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (35, 'OP20_20#', 'OP20_20#', 'GK-000000000000038901', 'GK-000000000000038902', 0, 0, NULL, '2023-03-06 14:32:16', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (36, 'OP20_21#', 'OP20_21#', 'GK-000000000000038903', 'GK-000000000000038904', 0, 0, NULL, '2023-03-06 14:32:34', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (37, 'OP20_22#', 'OP20_22#', 'GK-000000000000038905', 'GK-000000000000038906', 0, 0, NULL, '2023-03-06 14:32:47', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (38, 'OP20_23#', 'OP20_23#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:32:57', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (39, 'OP20_24#', 'OP20_24#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:33:08', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (40, 'OP20_25#', 'OP20_25#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:33:25', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (41, 'OP20_26#', 'OP20_26#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:33:37', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (42, 'OP20_27#', 'OP20_27#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:33:47', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (43, 'OP30_01#', 'OP30_01#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:34:07', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (44, 'OP30_02#', 'OP30_02#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:34:24', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (45, 'OP30_03#', 'OP30_03#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:34:36', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (46, 'OP30_04#', 'OP30_04#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:34:47', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (47, 'OP30_05#', 'OP30_05#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:35:00', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (48, 'OP30_06#', 'OP30_06#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:35:12', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (49, 'OP30_07#', 'OP30_07#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:35:23', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (50, 'OP30_08#', 'OP30_08#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:35:33', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (51, 'OP30_09#', 'OP30_09#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:35:59', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (52, 'OP30_10#', 'OP30_10#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:36:17', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (53, 'OP30_11#', 'OP30_11#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:36:28', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (54, 'OP30_12#', 'OP30_12#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:36:36', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (55, 'OP30_13#', 'OP30_13#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:36:55', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (56, 'OP30_14#', 'OP30_14#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:37:14', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (57, 'OP30_15#', 'OP30_15#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:37:29', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (58, 'OP30_16#', 'OP30_16#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:40:57', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (59, 'OP30_17#', 'OP30_17#', NULL, NULL, 0, 0, NULL, '2023-03-06 14:41:07', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (60, 'OP30_18#', 'OP30_18#', NULL, NULL, 0, 0, NULL, '2023-03-10 16:19:48', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (61, 'OP30_19#', 'OP30_19#', NULL, NULL, 0, 0, NULL, '2023-03-10 16:20:23', '2023-05-18 08:39:43', 'developer', 'developer', NULL);
INSERT INTO `device_carrier_config` VALUES (62, 'OP20_28#', 'OP20_28#', NULL, NULL, 0, 0, NULL, '2023-03-10 16:40:43', '2023-05-18 08:39:43', 'developer', 'developer', NULL);

-- ----------------------------
-- Table structure for log_eapevent
-- ----------------------------
DROP TABLE IF EXISTS `log_eapevent`;
CREATE TABLE `log_eapevent`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PlcEventLogId` int(11) NULL DEFAULT NULL,
  `PlcName` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Ip` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `StationName` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `EventName` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `StartTime` datetime NULL DEFAULT NULL,
  `SpanTime` double NULL DEFAULT NULL,
  `ResultCode` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Content` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Message` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of log_eapevent
-- ----------------------------

-- ----------------------------
-- Table structure for log_siemensevent
-- ----------------------------
DROP TABLE IF EXISTS `log_siemensevent`;
CREATE TABLE `log_siemensevent`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PlcEventLogId` int(11) NULL DEFAULT NULL,
  `PlcName` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Ip` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `StationName` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `EventName` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `StartTime` datetime NULL DEFAULT NULL,
  `SpanTime` double NULL DEFAULT NULL,
  `ResultCode` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Content` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Message` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of log_siemensevent
-- ----------------------------

-- ----------------------------
-- Table structure for machine_quickwearpart_config
-- ----------------------------
DROP TABLE IF EXISTS `machine_quickwearpart_config`;
CREATE TABLE `machine_quickwearpart_config`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Code` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '编号',
  `Name` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '名称',
  `ChangedTime` datetime NOT NULL COMMENT '跟换时间',
  `EarlyWarningTime` int(11) NOT NULL COMMENT '预警时间',
  `WarningTime` int(11) NOT NULL COMMENT '报警时间',
  `DeductCount` int(11) NOT NULL COMMENT '扣除次数',
  `Unit` int(11) NOT NULL COMMENT '时间单位',
  `UseCount` int(11) NOT NULL COMMENT '使用次数',
  `EarlyWarningCount` int(11) NOT NULL COMMENT '预警次数，易损件预警时开始工单会出现提示，可继续下发工单',
  `WarningCount` int(11) NOT NULL COMMENT '报警次数，易损件报警时开始工单会出现错误，不可下发工单',
  `Condition` int(11) NOT NULL COMMENT '条件',
  `Status` int(11) NOT NULL COMMENT '状态',
  `ST` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '对应工位',
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of machine_quickwearpart_config
-- ----------------------------

-- ----------------------------
-- Table structure for machine_warning_log
-- ----------------------------
DROP TABLE IF EXISTS `machine_warning_log`;
CREATE TABLE `machine_warning_log`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TaskId` int(11) NULL DEFAULT NULL COMMENT '订单号',
  `OP` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '机台编号',
  `No` int(11) NULL DEFAULT NULL COMMENT '错误列表中排序',
  `Code` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '错误代码',
  `StationName` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '工位名称',
  `TargetName` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '目标名称',
  `WarningText` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '报警错误详情',
  `StartTime` datetime NULL DEFAULT NULL COMMENT '报警开始时间',
  `EndTime` datetime NULL DEFAULT NULL COMMENT '报警结束时间',
  `ElapsedTime` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '报警耗时(s)',
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of machine_warning_log
-- ----------------------------

-- ----------------------------
-- Table structure for material_scan_log
-- ----------------------------
DROP TABLE IF EXISTS `material_scan_log`;
CREATE TABLE `material_scan_log`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '物料名称',
  `Code` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '物料编号',
  `Batch` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '物料批次',
  `Content` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '内容',
  `ScanTime` datetime NOT NULL COMMENT '扫码时间',
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of material_scan_log
-- ----------------------------

-- ----------------------------
-- Table structure for product_boxcode_updatelog
-- ----------------------------
DROP TABLE IF EXISTS `product_boxcode_updatelog`;
CREATE TABLE `product_boxcode_updatelog`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Idx` int(11) NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of product_boxcode_updatelog
-- ----------------------------

-- ----------------------------
-- Table structure for product_recipe_config
-- ----------------------------
DROP TABLE IF EXISTS `product_recipe_config`;
CREATE TABLE `product_recipe_config`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `RecipeNo` int(11) NOT NULL COMMENT '配方编号',
  `Name` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '配方名称',
  `SecondId` int(11) NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of product_recipe_config
-- ----------------------------
INSERT INTO `product_recipe_config` VALUES (1, 1, '配方01', 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_config` VALUES (2, 1, '配方02', 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_config` VALUES (3, 1, '配方03', 1, NULL, NULL, NULL, NULL, NULL, NULL);

-- ----------------------------
-- Table structure for product_recipe_material_config
-- ----------------------------
DROP TABLE IF EXISTS `product_recipe_material_config`;
CREATE TABLE `product_recipe_material_config`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Value` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `RecipeConfigId` int(11) NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 151 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of product_recipe_material_config
-- ----------------------------
INSERT INTO `product_recipe_material_config` VALUES (1, '物料名称01', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (2, '物料名称02', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (3, '物料名称03', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (4, '物料名称04', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (5, '物料名称05', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (6, '物料名称06', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (7, '物料名称07', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (8, '物料名称08', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (9, '物料名称09', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (10, '物料名称10', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (11, '物料名称01', NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (12, '物料名称02', NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (13, '物料名称03', NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (14, '物料名称04', NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (15, '物料名称05', NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (16, '物料名称06', NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (17, '物料名称07', NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (18, '物料名称08', NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (19, '物料名称09', NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (20, '物料名称10', NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (21, '物料名称01', NULL, 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (22, '物料名称02', NULL, 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (23, '物料名称03', NULL, 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (24, '物料名称04', NULL, 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (25, '物料名称05', NULL, 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (26, '物料名称06', NULL, 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (27, '物料名称07', NULL, 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (28, '物料名称08', NULL, 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (29, '物料名称09', NULL, 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (30, '物料名称10', NULL, 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (31, '物料名称01', NULL, 4, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (32, '物料名称02', NULL, 4, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (33, '物料名称03', NULL, 4, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (34, '物料名称04', NULL, 4, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (35, '物料名称05', NULL, 4, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (36, '物料名称06', NULL, 4, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (37, '物料名称07', NULL, 4, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (38, '物料名称08', NULL, 4, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (39, '物料名称09', NULL, 4, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (40, '物料名称10', NULL, 4, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (41, '物料名称01', NULL, 5, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (42, '物料名称02', NULL, 5, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (43, '物料名称03', NULL, 5, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (44, '物料名称04', NULL, 5, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (45, '物料名称05', NULL, 5, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (46, '物料名称06', NULL, 5, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (47, '物料名称07', NULL, 5, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (48, '物料名称08', NULL, 5, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (49, '物料名称09', NULL, 5, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (50, '物料名称10', NULL, 5, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (51, '物料名称01', NULL, 6, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (52, '物料名称02', NULL, 6, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (53, '物料名称03', NULL, 6, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (54, '物料名称04', NULL, 6, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (55, '物料名称05', NULL, 6, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (56, '物料名称06', NULL, 6, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (57, '物料名称07', NULL, 6, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (58, '物料名称08', NULL, 6, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (59, '物料名称09', NULL, 6, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (60, '物料名称10', NULL, 6, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (61, '物料名称01', NULL, 7, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (62, '物料名称02', NULL, 7, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (63, '物料名称03', NULL, 7, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (64, '物料名称04', NULL, 7, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (65, '物料名称05', NULL, 7, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (66, '物料名称06', NULL, 7, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (67, '物料名称07', NULL, 7, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (68, '物料名称08', NULL, 7, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (69, '物料名称09', NULL, 7, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (70, '物料名称10', NULL, 7, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (71, '物料名称01', NULL, 8, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (72, '物料名称02', NULL, 8, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (73, '物料名称03', NULL, 8, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (74, '物料名称04', NULL, 8, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (75, '物料名称05', NULL, 8, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (76, '物料名称06', NULL, 8, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (77, '物料名称07', NULL, 8, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (78, '物料名称08', NULL, 8, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (79, '物料名称09', NULL, 8, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (80, '物料名称10', NULL, 8, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (81, '物料名称01', NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (82, '物料名称02', NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (83, '物料名称03', NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (84, '物料名称04', NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (85, '物料名称05', NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (86, '物料名称06', NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (87, '物料名称07', NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (88, '物料名称08', NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (89, '物料名称09', NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (90, '物料名称10', NULL, 9, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (91, '物料名称01', NULL, 10, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (92, '物料名称02', NULL, 10, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (93, '物料名称03', NULL, 10, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (94, '物料名称04', NULL, 10, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (95, '物料名称05', NULL, 10, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (96, '物料名称06', NULL, 10, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (97, '物料名称07', NULL, 10, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (98, '物料名称08', NULL, 10, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (99, '物料名称09', NULL, 10, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (100, '物料名称10', NULL, 10, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (101, '物料名称01', NULL, 11, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (102, '物料名称02', NULL, 11, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (103, '物料名称03', NULL, 11, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (104, '物料名称04', NULL, 11, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (105, '物料名称05', NULL, 11, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (106, '物料名称06', NULL, 11, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (107, '物料名称07', NULL, 11, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (108, '物料名称08', NULL, 11, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (109, '物料名称09', NULL, 11, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (110, '物料名称10', NULL, 11, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (111, '物料名称01', NULL, 12, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (112, '物料名称02', NULL, 12, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (113, '物料名称03', NULL, 12, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (114, '物料名称04', NULL, 12, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (115, '物料名称05', NULL, 12, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (116, '物料名称06', NULL, 12, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (117, '物料名称07', NULL, 12, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (118, '物料名称08', NULL, 12, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (119, '物料名称09', NULL, 12, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (120, '物料名称10', NULL, 12, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (121, '物料名称01', NULL, 13, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (122, '物料名称02', NULL, 13, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (123, '物料名称03', NULL, 13, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (124, '物料名称04', NULL, 13, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (125, '物料名称05', NULL, 13, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (126, '物料名称06', NULL, 13, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (127, '物料名称07', NULL, 13, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (128, '物料名称08', NULL, 13, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (129, '物料名称09', NULL, 13, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (130, '物料名称10', NULL, 13, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (131, '物料名称01', NULL, 14, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (132, '物料名称02', NULL, 14, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (133, '物料名称03', NULL, 14, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (134, '物料名称04', NULL, 14, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (135, '物料名称05', NULL, 14, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (136, '物料名称06', NULL, 14, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (137, '物料名称07', NULL, 14, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (138, '物料名称08', NULL, 14, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (139, '物料名称09', NULL, 14, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (140, '物料名称10', NULL, 14, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (141, '物料名称01', NULL, 15, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (142, '物料名称02', NULL, 15, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (143, '物料名称03', NULL, 15, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (144, '物料名称04', NULL, 15, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (145, '物料名称05', NULL, 15, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (146, '物料名称06', NULL, 15, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (147, '物料名称07', NULL, 15, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (148, '物料名称08', NULL, 15, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (149, '物料名称09', NULL, 15, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_material_config` VALUES (150, '物料名称10', NULL, 15, NULL, NULL, NULL, NULL, NULL, NULL);

-- ----------------------------
-- Table structure for product_recipe_st_config
-- ----------------------------
DROP TABLE IF EXISTS `product_recipe_st_config`;
CREATE TABLE `product_recipe_st_config`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'ST名称',
  `RecipeConfigId` int(11) NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 61 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of product_recipe_st_config
-- ----------------------------
INSERT INTO `product_recipe_st_config` VALUES (1, 'ST01', 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (2, 'ST02', 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (3, 'ST03', 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (4, 'ST04', 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (5, 'ST05', 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (6, 'ST06', 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (7, 'ST07', 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (8, 'ST08', 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (9, 'ST09', 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (10, 'ST10', 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (11, 'ST11', 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (12, 'ST12', 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (13, 'ST13', 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (14, 'ST14', 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (15, 'ST15', 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (16, 'ST16', 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (17, 'ST17', 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (18, 'ST18', 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (19, 'ST19', 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (20, 'ST20', 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (21, 'ST01', 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (22, 'ST02', 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (23, 'ST05', 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (24, 'ST07', 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (25, 'ST10', 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (26, 'ST12', 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (27, 'ST13', 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (28, 'ST14', 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (29, 'ST15', 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (30, 'ST18', 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (31, 'ST19', 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (32, 'ST20 ', 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (33, 'ST21', 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (34, 'ST22', 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (35, 'ST24', 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (36, 'ST25', 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (37, 'ST27', 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (38, '预留1', 2, 'false', NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (39, '预留2', 2, 'false', NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (40, '预留3', 2, 'false', NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (41, 'ST01', 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (42, 'ST02', 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (43, 'ST03', 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (44, 'ST04', 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (45, 'ST05', 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (46, 'ST06', 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (47, 'ST07', 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (48, 'ST08', 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (49, 'ST09', 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (50, 'ST10', 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (51, 'ST11', 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (52, 'ST12', 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (53, 'ST13', 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (54, 'ST14', 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (55, 'ST15', 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (56, 'ST16', 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (57, 'ST17', 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (58, 'ST18', 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (59, 'ST19', 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_config` VALUES (60, 'ST20', 3, NULL, NULL, NULL, NULL, NULL, NULL);

-- ----------------------------
-- Table structure for product_recipe_st_parameter_config
-- ----------------------------
DROP TABLE IF EXISTS `product_recipe_st_parameter_config`;
CREATE TABLE `product_recipe_st_parameter_config`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Value` float NOT NULL,
  `RecipeSTId` int(11) NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 601 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of product_recipe_st_parameter_config
-- ----------------------------
INSERT INTO `product_recipe_st_parameter_config` VALUES (1, '底座高度上限(mm)', 13.05, 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (2, '底座高度下限(mm)', 12.85, 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (3, '参数3', 0, 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (4, '参数4', 0, 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (5, '参数5', 0, 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (6, '参数6', 0, 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (7, '参数7', 0, 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (8, '参数8', 0, 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (9, '参数9', 0, 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (10, '参数10', 0, 1, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (11, '相机作业编号', 1, 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (12, '参数1', 0, 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (13, '参数2', 0, 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (14, '参数3', 0, 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (15, '参数4', 0, 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (16, '参数5', 0, 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (17, '参数6', 0, 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (18, '参数7', 0, 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (19, '参数8', 0, 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (20, '参数9', 0, 2, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (21, '内O厚度上限(mm)', 1.8, 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (22, '内O厚度下限(mm)', 1.3, 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (23, '参数2', 0, 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (24, '参数3', 0, 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (25, '参数4', 0, 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (26, '参数5', 0, 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (27, '参数6', 0, 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (28, '参数7', 0, 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (29, '参数8', 0, 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (30, '参数9', 0, 3, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (31, '电点火管颜色', 3, 4, '下拉框', NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (32, '参数1', 0, 4, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (33, '参数2', 0, 4, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (34, '参数3', 0, 4, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (35, '参数4', 0, 4, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (36, '参数5', 0, 4, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (37, '参数6', 0, 4, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (38, '参数7', 0, 4, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (39, '参数8', 0, 4, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (40, '参数9', 0, 4, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (41, '参数0', 0, 5, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (42, '参数1', 0, 5, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (43, '参数2', 0, 5, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (44, '参数3', 0, 5, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (45, '参数4', 0, 5, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (46, '参数5', 0, 5, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (47, '参数6', 0, 5, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (48, '参数7', 0, 5, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (49, '参数8', 0, 5, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (50, '参数9', 0, 5, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (51, '收口压力上限(N)', 10000, 6, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (52, '收口压力下限(N)', 4500, 6, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (53, '收口保压时间(s)', 1, 6, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (54, '设置收口压力(N)', 5000, 6, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (55, '参数4', 0, 6, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (56, '参数5', 0, 6, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (57, '参数6', 0, 6, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (58, '参数7', 0, 6, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (59, '参数8', 0, 6, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (60, '参数9', 0, 6, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (61, '参数0', 0, 7, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (62, '参数1', 0, 7, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (63, '参数2', 0, 7, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (64, '参数3', 0, 7, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (65, '参数4', 0, 7, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (66, '参数5', 0, 7, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (67, '参数6', 0, 7, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (68, '参数7', 0, 7, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (69, '参数8', 0, 7, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (70, '参数9', 0, 7, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (71, '参数0', 0, 8, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (72, '参数1', 0, 8, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (73, '参数2', 0, 8, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (74, '参数3', 0, 8, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (75, '参数4', 0, 8, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (76, '参数5', 0, 8, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (77, '参数6', 0, 8, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (78, '参数7', 0, 8, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (79, '参数8', 0, 8, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (80, '参数9', 0, 8, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (81, '发火体收口高度上限(mm)', 12.8, 9, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (82, '发火体收口高度下限(mm)', 12.2, 9, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (83, '参数2', 0, 9, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (84, '参数3', 0, 9, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (85, '参数4', 0, 9, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (86, '参数5', 0, 9, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (87, '参数6', 0, 9, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (88, '参数7', 0, 9, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (89, '参数8', 0, 9, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (90, '参数9', 0, 9, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (91, '参数0', 0, 10, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (92, '参数1', 0, 10, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (93, '参数2', 0, 10, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (94, '参数3', 0, 10, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (95, '参数4', 0, 10, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (96, '参数5', 0, 10, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (97, '参数6', 0, 10, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (98, '参数7', 0, 10, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (99, '参数8', 0, 10, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (100, '参数9', 0, 10, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (101, '外O型圈厚度上限(mm)', 1, 11, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (102, '外O型圈厚度下限(mm)', 0.4, 11, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (103, '参数2', 0, 11, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (104, '参数3', 0, 11, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (105, '参数4', 0, 11, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (106, '参数5', 0, 11, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (107, '参数6', 0, 11, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (108, '参数7', 0, 11, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (109, '参数8', 0, 11, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (110, '参数9', 0, 11, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (111, '左相机作业编号', 3, 12, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (112, '右相机作业编号', 3, 12, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (113, '参数2', 0, 12, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (114, '参数3', 0, 12, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (115, '参数4', 0, 12, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (116, '参数5', 0, 12, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (117, '参数6', 0, 12, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (118, '参数7', 0, 12, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (119, '参数8', 0, 12, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (120, '参数9', 0, 12, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (121, '参数0', 0, 13, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (122, '参数1', 0, 13, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (123, '参数2', 0, 13, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (124, '参数3', 0, 13, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (125, '参数4', 0, 13, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (126, '参数5', 0, 13, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (127, '参数6', 0, 13, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (128, '参数7', 0, 13, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (129, '参数8', 0, 13, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (130, '参数9', 0, 13, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (131, '参数0', 0, 14, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (132, '参数1', 0, 14, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (133, '参数2', 0, 14, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (134, '参数3', 0, 14, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (135, '参数4', 0, 14, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (136, '参数5', 0, 14, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (137, '参数6', 0, 14, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (138, '参数7', 0, 14, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (139, '参数8', 0, 14, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (140, '参数9', 0, 14, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (141, '参数0', 0, 15, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (142, '参数1', 0, 15, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (143, '参数2', 0, 15, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (144, '参数3', 0, 15, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (145, '参数4', 0, 15, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (146, '参数5', 0, 15, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (147, '参数6', 0, 15, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (148, '参数7', 0, 15, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (149, '参数8', 0, 15, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (150, '参数9', 0, 15, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (151, '参数0', 0, 16, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (152, '参数1', 0, 16, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (153, '参数2', 0, 16, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (154, '参数3', 0, 16, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (155, '参数4', 0, 16, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (156, '参数5', 0, 16, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (157, '参数6', 0, 16, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (158, '参数7', 0, 16, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (159, '参数8', 0, 16, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (160, '参数9', 0, 16, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (161, '参数0', 0, 17, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (162, '参数1', 0, 17, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (163, '参数2', 0, 17, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (164, '参数3', 0, 17, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (165, '参数4', 0, 17, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (166, '参数5', 0, 17, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (167, '参数6', 0, 17, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (168, '参数7', 0, 17, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (169, '参数8', 0, 17, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (170, '参数9', 0, 17, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (171, '参数0', 0, 18, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (172, '参数1', 0, 18, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (173, '参数2', 0, 18, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (174, '参数3', 0, 18, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (175, '参数4', 0, 18, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (176, '参数5', 0, 18, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (177, '参数6', 0, 18, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (178, '参数7', 0, 18, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (179, '参数8', 0, 18, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (180, '参数9', 0, 18, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (181, '参数0', 0, 19, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (182, '参数1', 0, 19, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (183, '参数2', 0, 19, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (184, '参数3', 0, 19, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (185, '参数4', 0, 19, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (186, '参数5', 0, 19, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (187, '参数6', 0, 19, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (188, '参数7', 0, 19, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (189, '参数8', 0, 19, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (190, '参数9', 0, 19, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (191, '参数0', 0, 20, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (192, '参数1', 0, 20, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (193, '参数2', 0, 20, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (194, '参数3', 0, 20, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (195, '参数4', 0, 20, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (196, '参数5', 0, 20, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (197, '参数6', 0, 20, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (198, '参数7', 0, 20, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (199, '参数8', 0, 20, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (200, '参数9', 0, 20, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (201, '管壳高度上限(mm)', 22.5, 21, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (202, '管壳高度下限(mm)', 20.5, 21, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (203, '相机作业编号', 1, 21, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (204, '参数3', 0, 21, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (205, '参数4', 0, 21, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (206, '参数5', 0, 21, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (207, '参数6', 0, 21, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (208, '参数7', 0, 21, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (209, '参数8', 0, 21, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (210, '参数9', 0, 21, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (211, '空管壳重量上限(mg)', 1600, 22, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (212, '空管壳重量下限(mg)', 1400, 22, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (213, '参数2', 0, 22, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (214, '参数3', 0, 22, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (215, '参数4', 0, 22, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (216, '参数5', 0, 22, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (217, '参数6', 0, 22, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (218, '参数7', 0, 22, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (219, '参数8', 0, 22, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (220, '参数9', 0, 22, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (221, '配方编号', 1, 23, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (222, '参数1', 0, 23, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (223, '参数2', 0, 23, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (224, '参数3', 0, 23, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (225, '参数4', 0, 23, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (226, '参数5', 0, 23, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (227, '参数6', 0, 23, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (228, '参数7', 0, 23, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (229, '参数8', 0, 23, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (230, '参数9', 0, 23, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (231, '一次装药重量上限(mg)', 887, 24, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (232, '一次装药重量下限(mg)', 843, 24, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (233, '参数2', 0, 24, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (234, '参数3', 0, 24, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (235, '参数4', 0, 24, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (236, '参数5', 0, 24, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (237, '参数6', 0, 24, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (238, '参数7', 0, 24, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (239, '参数8', 0, 24, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (240, '参数9', 0, 24, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (241, '配方编号', 1, 25, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (242, '参数1', 0, 25, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (243, '参数2', 0, 25, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (244, '参数3', 0, 25, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (245, '参数4', 0, 25, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (246, '参数5', 0, 25, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (247, '参数6', 0, 25, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (248, '参数7', 0, 25, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (249, '参数8', 0, 25, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (250, '参数9', 0, 25, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (251, '二次装药重量上限(mg)', 2200, 26, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (252, '二次装药重量下限(mg)', 1400, 26, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (253, '参数2', 0, 26, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (254, '参数3', 0, 26, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (255, '参数4', 0, 26, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (256, '参数5', 0, 26, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (257, '参数6', 0, 26, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (258, '参数7', 0, 26, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (259, '参数8', 0, 26, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (260, '参数9', 0, 26, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (261, '参数0', 0, 27, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (262, '参数1', 0, 27, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (263, '参数2', 0, 27, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (264, '参数3', 0, 27, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (265, '参数4', 0, 27, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (266, '参数5', 0, 27, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (267, '参数6', 0, 27, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (268, '参数7', 0, 27, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (269, '参数8', 0, 27, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (270, '参数9', 0, 27, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (271, '参数0', 0, 28, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (272, '参数1', 0, 28, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (273, '参数2', 0, 28, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (274, '参数3', 0, 28, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (275, '参数4', 0, 28, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (276, '参数5', 0, 28, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (277, '参数6', 0, 28, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (278, '参数7', 0, 28, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (279, '参数8', 0, 28, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (280, '参数9', 0, 28, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (281, '参数0', 0, 29, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (282, '参数1', 0, 29, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (283, '参数2', 0, 29, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (284, '参数3', 0, 29, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (285, '参数4', 0, 29, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (286, '参数5', 0, 29, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (287, '参数6', 0, 29, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (288, '参数7', 0, 29, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (289, '参数8', 0, 29, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (290, '参数9', 0, 29, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (291, '参数0', 0, 30, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (292, '参数1', 0, 30, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (293, '参数2', 0, 30, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (294, '参数3', 0, 30, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (295, '参数4', 0, 30, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (296, '参数5', 0, 30, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (297, '参数6', 0, 30, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (298, '参数7', 0, 30, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (299, '参数8', 0, 30, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (300, '参数9', 0, 30, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (301, '相机作业编号', 0, 31, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (302, '参数1', 0, 31, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (303, '参数2', 0, 31, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (304, '参数3', 0, 31, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (305, '参数4', 0, 31, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (306, '参数5', 0, 31, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (307, '参数6', 0, 31, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (308, '参数7', 0, 31, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (309, '参数8', 0, 31, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (310, '参数9', 0, 31, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (311, '外O圈厚度上限(mm)', 8.5, 32, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (312, '外O圈厚度下限(mm)', 7, 32, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (313, '参数2', 0, 32, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (314, '参数3', 0, 32, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (315, '参数4', 0, 32, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (316, '参数5', 0, 32, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (317, '参数6', 0, 32, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (318, '参数7', 0, 32, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (319, '参数8', 0, 32, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (320, '参数9', 0, 32, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (321, 'MGG收口压力上限(N)', 18000, 33, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (322, 'MGG收口压力下限(N)', 12000, 33, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (323, 'MGG收口保压时间(s)', 1.5, 33, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (324, '设置收口压力(N)', 12000, 33, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (325, '参数4', 0, 33, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (326, '参数5', 0, 33, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (327, '参数6', 0, 33, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (328, '参数7', 0, 33, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (329, '参数8', 0, 33, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (330, '参数9', 0, 33, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (331, '参数0', 0, 34, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (332, '参数1', 0, 34, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (333, '参数2', 0, 34, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (334, '参数3', 0, 34, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (335, '参数4', 0, 34, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (336, '参数5', 0, 34, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (337, '参数6', 0, 34, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (338, '参数7', 0, 34, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (339, '参数8', 0, 34, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (340, '参数9', 0, 34, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (341, '平行度上限(°)', 4, 35, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (342, '平行度下限(°)', -4, 35, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (343, '相机作业编号', 1, 35, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (344, '参数3', 0, 35, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (345, '参数4', 0, 35, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (346, '参数5', 0, 35, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (347, '参数6', 0, 35, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (348, '参数7', 0, 35, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (349, '参数8', 0, 35, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (350, '参数9', 0, 35, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (351, 'MGG收口外径上限(mm)', 16.98, 36, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (352, 'MGG收口外径下限(mm)', 16.82, 36, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (353, '参数2', 0, 36, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (354, '参数3', 0, 36, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (355, '参数4', 0, 36, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (356, '参数5', 0, 36, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (357, '参数6', 0, 36, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (358, '参数7', 0, 36, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (359, '参数8', 0, 36, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (360, '参数9', 0, 36, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (361, 'MGG收口高度上限(mm)', 7.78, 37, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (362, 'MGG收口高度下限(mm)', 7.42, 37, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (363, 'MGG总高度上限(mm)', 31.8, 37, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (364, 'MGG总高度下限(mm)', 30.5, 37, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (365, '参数4', 0, 37, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (366, '参数5', 0, 37, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (367, '参数6', 0, 37, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (368, '参数7', 0, 37, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (369, '参数8', 0, 37, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (370, '参数9', 0, 37, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (371, '参数0', 0, 38, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (372, '参数1', 0, 38, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (373, '参数2', 0, 38, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (374, '参数3', 0, 38, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (375, '参数4', 0, 38, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (376, '参数5', 0, 38, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (377, '参数6', 0, 38, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (378, '参数7', 0, 38, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (379, '参数8', 0, 38, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (380, '参数9', 0, 38, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (381, '参数0', 0, 39, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (382, '参数1', 0, 39, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (383, '参数2', 0, 39, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (384, '参数3', 0, 39, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (385, '参数4', 0, 39, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (386, '参数5', 0, 39, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (387, '参数6', 0, 39, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (388, '参数7', 0, 39, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (389, '参数8', 0, 39, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (390, '参数9', 0, 39, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (391, '参数0', 0, 40, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (392, '参数1', 0, 40, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (393, '参数2', 0, 40, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (394, '参数3', 0, 40, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (395, '参数4', 0, 40, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (396, '参数5', 0, 40, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (397, '参数6', 0, 40, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (398, '参数7', 0, 40, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (399, '参数8', 0, 40, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (400, '参数9', 0, 40, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (401, '参数0', 0, 41, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (402, '参数1', 0, 41, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (403, '参数2', 0, 41, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (404, '参数3', 0, 41, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (405, '参数4', 0, 41, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (406, '参数5', 0, 41, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (407, '参数6', 0, 41, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (408, '参数7', 0, 41, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (409, '参数8', 0, 41, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (410, '参数9', 0, 41, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (411, '参数0', 0, 42, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (412, '参数1', 0, 42, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (413, '参数2', 0, 42, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (414, '参数3', 0, 42, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (415, '参数4', 0, 42, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (416, '参数5', 0, 42, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (417, '参数6', 0, 42, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (418, '参数7', 0, 42, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (419, '参数8', 0, 42, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (420, '参数9', 0, 42, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (421, '参数0', 0, 43, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (422, '参数1', 0, 43, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (423, '参数2', 0, 43, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (424, '参数3', 0, 43, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (425, '参数4', 0, 43, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (426, '参数5', 0, 43, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (427, '参数6', 0, 43, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (428, '参数7', 0, 43, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (429, '参数8', 0, 43, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (430, '参数9', 0, 43, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (431, '参数0', 0, 44, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (432, '参数1', 0, 44, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (433, '参数2', 0, 44, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (434, '参数3', 0, 44, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (435, '参数4', 0, 44, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (436, '参数5', 0, 44, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (437, '参数6', 0, 44, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (438, '参数7', 0, 44, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (439, '参数8', 0, 44, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (440, '参数9', 0, 44, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (441, '短路夹型号', 1, 45, '下拉框', NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (442, '参数1', 0, 45, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (443, '参数2', 0, 45, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (444, '参数3', 0, 45, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (445, '参数4', 0, 45, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (446, '参数5', 0, 45, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (447, '参数6', 0, 45, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (448, '参数7', 0, 45, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (449, '参数8', 0, 45, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (450, '参数9', 0, 45, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (451, '压力上限(N)', 2000, 46, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (452, '压力下限(N)', -1000, 46, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (453, '相机作业编号', 1, 46, '下拉框', NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (454, '参数3', 0, 46, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (455, '参数4', 0, 46, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (456, '参数5', 0, 46, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (457, '参数6', 0, 46, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (458, '参数7', 0, 46, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (459, '参数8', 0, 46, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (460, '参数9', 0, 46, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (461, '参数0', 0, 47, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (462, '参数1', 0, 47, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (463, '参数2', 0, 47, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (464, '参数3', 0, 47, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (465, '参数4', 0, 47, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (466, '参数5', 0, 47, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (467, '参数6', 0, 47, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (468, '参数7', 0, 47, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (469, '参数8', 0, 47, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (470, '参数9', 0, 47, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (471, '短路夹装配高度上限(mm)', 1.6, 48, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (472, '短路夹装配高度下限(mm)', 1.2, 48, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (473, '参数2', 0, 48, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (474, '参数3', 0, 48, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (475, '参数4', 0, 48, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (476, '参数5', 0, 48, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (477, '参数6', 0, 48, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (478, '参数7', 0, 48, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (479, '参数8', 0, 48, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (480, '参数9', 0, 48, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (481, 'MGG桥路电阻上限(Ω)', 2.15, 49, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (482, 'MGG桥路电阻下限(Ω)', 1.7, 49, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (483, '参数2', 0, 49, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (484, '参数3', 0, 49, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (485, '参数4', 0, 49, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (486, '参数5', 0, 49, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (487, '参数6', 0, 49, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (488, '参数7', 0, 49, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (489, '参数8', 0, 49, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (490, '参数9', 0, 49, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (491, '参数0', 0, 50, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (492, '参数1', 0, 50, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (493, '参数2', 0, 50, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (494, '参数3', 0, 50, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (495, '参数4', 0, 50, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (496, '参数5', 0, 50, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (497, '参数6', 0, 50, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (498, '参数7', 0, 50, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (499, '参数8', 0, 50, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (500, '参数9', 0, 50, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (501, 'MGG绝缘电阻上限(GΩ)', 5000, 51, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (502, 'MGG绝缘电阻下限(GΩ)', 0.1, 51, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (503, '参数2', 0, 51, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (504, '参数3', 0, 51, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (505, '参数4', 0, 51, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (506, '参数5', 0, 51, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (507, '参数6', 0, 51, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (508, '参数7', 0, 51, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (509, '参数8', 0, 51, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (510, '参数9', 0, 51, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (511, 'MGG短路电阻上限(Ω)', 0.3, 52, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (512, 'MGG短路电阻下限(Ω)', 0, 52, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (513, '参数2', 0, 52, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (514, '参数3', 0, 52, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (515, '参数4', 0, 52, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (516, '参数5', 0, 52, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (517, '参数6', 0, 52, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (518, '参数7', 0, 52, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (519, '参数8', 0, 52, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (520, '参数9', 0, 52, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (521, '打标模式', 2, 53, '下拉框', NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (522, '参数1', 0, 53, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (523, '参数2', 0, 53, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (524, '参数3', 0, 53, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (525, '参数4', 0, 53, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (526, '参数5', 0, 53, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (527, '参数6', 0, 53, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (528, '参数7', 0, 53, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (529, '参数8', 0, 53, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (530, '参数9', 0, 53, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (531, '相机作业编号', 1, 54, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (532, '参数1', 0, 54, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (533, '参数2', 0, 54, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (534, '参数3', 0, 54, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (535, '参数4', 0, 54, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (536, '参数5', 0, 54, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (537, '参数6', 0, 54, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (538, '参数7', 0, 54, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (539, '参数8', 0, 54, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (540, '参数9', 0, 54, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (541, '打码模式', 1, 55, '下拉框', NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (542, '参数1', 0, 55, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (543, '参数2', 0, 55, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (544, '参数3', 0, 55, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (545, '参数4', 0, 55, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (546, '参数5', 0, 55, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (547, '参数6', 0, 55, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (548, '参数7', 0, 55, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (549, '参数8', 0, 55, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (550, '参数9', 0, 55, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (551, '参数0', 0, 56, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (552, '参数1', 0, 56, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (553, '参数2', 0, 56, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (554, '参数3', 0, 56, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (555, '参数4', 0, 56, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (556, '参数5', 0, 56, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (557, '参数6', 0, 56, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (558, '参数7', 0, 56, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (559, '参数8', 0, 56, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (560, '参数9', 0, 56, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (561, '参数0', 0, 57, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (562, '参数1', 0, 57, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (563, '参数2', 0, 57, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (564, '参数3', 0, 57, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (565, '参数4', 0, 57, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (566, '参数5', 0, 57, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (567, '参数6', 0, 57, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (568, '参数7', 0, 57, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (569, '参数8', 0, 57, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (570, '参数9', 0, 57, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (571, '参数0', 0, 58, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (572, '参数1', 0, 58, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (573, '参数2', 0, 58, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (574, '参数3', 0, 58, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (575, '参数4', 0, 58, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (576, '参数5', 0, 58, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (577, '参数6', 0, 58, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (578, '参数7', 0, 58, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (579, '参数8', 0, 58, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (580, '参数9', 0, 58, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (581, '料盘型号', 1, 59, '下拉框', NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (582, '参数1', 0, 59, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (583, '参数2', 0, 59, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (584, '参数3', 0, 59, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (585, '参数4', 0, 59, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (586, '参数5', 0, 59, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (587, '参数6', 0, 59, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (588, '参数7', 0, 59, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (589, '参数8', 0, 59, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (590, '参数9', 0, 59, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (591, '参数0', 1, 60, '', NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (592, '参数1', 0, 60, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (593, '参数2', 0, 60, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (594, '参数3', 0, 60, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (595, '参数4', 0, 60, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (596, '参数5', 0, 60, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (597, '参数6', 0, 60, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (598, '参数7', 0, 60, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (599, '参数8', 0, 60, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `product_recipe_st_parameter_config` VALUES (600, '参数9', 0, 60, NULL, NULL, NULL, NULL, NULL, NULL);

-- ----------------------------
-- Table structure for product_task_boxcode1_config
-- ----------------------------
DROP TABLE IF EXISTS `product_task_boxcode1_config`;
CREATE TABLE `product_task_boxcode1_config`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CPDH` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '产品代号',
  `GDM` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '固定码',
  `YYR` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '年月日',
  `SL` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '本盒数量',
  `HXH` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '盒序号',
  `YRY` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '月日年',
  `Idx` int(11) NOT NULL COMMENT '下次打标流水号',
  `BoxCodeId` int(11) NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of product_task_boxcode1_config
-- ----------------------------

-- ----------------------------
-- Table structure for product_task_boxcode_config
-- ----------------------------
DROP TABLE IF EXISTS `product_task_boxcode_config`;
CREATE TABLE `product_task_boxcode_config`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Select` int(11) NOT NULL COMMENT '1 表示型号Ⅰ,2表示型号Ⅱ',
  `TaskId` int(11) NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of product_task_boxcode_config
-- ----------------------------

-- ----------------------------
-- Table structure for product_task_config
-- ----------------------------
DROP TABLE IF EXISTS `product_task_config`;
CREATE TABLE `product_task_config`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Code` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '工单代码',
  `Name` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '工单名称',
  `Batch` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '批次号',
  `BatchName` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '批次名称',
  `StartSerialNumber1` int(11) NOT NULL COMMENT '一维码起始序列号',
  `StartSerialNumber2` int(11) NOT NULL COMMENT '二维码起始序列号',
  `StartSerialNumber3` int(11) NOT NULL COMMENT '盒起始序列号',
  `Status` int(11) NOT NULL COMMENT '工单状态',
  `PlanQty` int(11) NOT NULL COMMENT '计划数量',
  `DZPutIntoQty` int(11) NOT NULL COMMENT '底座投料数',
  `DZNgQty` int(11) NOT NULL COMMENT '底座NG数',
  `GKPutIntoQty` int(11) NOT NULL COMMENT '管壳投料数',
  `GKNgQty` int(11) NOT NULL COMMENT '管壳NG数',
  `MergeQty` int(11) NOT NULL COMMENT '合并成品数',
  `OkQty` int(11) NOT NULL COMMENT '成品OK数量',
  `NgQty` int(11) NOT NULL COMMENT '成品NG数量',
  `OP10Mode` int(11) NOT NULL COMMENT 'OP10工单模式',
  `OP20Mode` int(11) NOT NULL COMMENT 'OP20工单模式',
  `OP30Mode` int(11) NOT NULL COMMENT 'OP30工单模式',
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of product_task_config
-- ----------------------------

-- ----------------------------
-- Table structure for product_task_material_config
-- ----------------------------
DROP TABLE IF EXISTS `product_task_material_config`;
CREATE TABLE `product_task_material_config`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TaskId` int(11) NOT NULL COMMENT '工单ID',
  `Name` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '原料名称',
  `Code` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '物料编码',
  `Count1` int(11) NOT NULL COMMENT '|个数1',
  `Idx1` int(11) NOT NULL COMMENT '|位置1',
  `Count2` int(11) NOT NULL COMMENT '|个数2',
  `Idx2` int(11) NOT NULL COMMENT '|位置2',
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of product_task_material_config
-- ----------------------------

-- ----------------------------
-- Table structure for product_task_material_scan_log
-- ----------------------------
DROP TABLE IF EXISTS `product_task_material_scan_log`;
CREATE TABLE `product_task_material_scan_log`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '物料名称',
  `Code` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '物料编号',
  `Batch` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '物料批次',
  `ScanTime` datetime NOT NULL COMMENT '扫码时间',
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of product_task_material_scan_log
-- ----------------------------

-- ----------------------------
-- Table structure for product_task_onecode_config
-- ----------------------------
DROP TABLE IF EXISTS `product_task_onecode_config`;
CREATE TABLE `product_task_onecode_config`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `GSDH` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '公司代号',
  `CPMC` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '产品名称',
  `CPXH` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '产品型号',
  `YH` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '年号',
  `CPPH` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '产品批号',
  `CXH` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '产线号',
  `CPXLH` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '产品序列号',
  `Idx` int(11) NOT NULL COMMENT '下次打标流水号',
  `TaskId` int(11) NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of product_task_onecode_config
-- ----------------------------

-- ----------------------------
-- Table structure for product_task_route_config
-- ----------------------------
DROP TABLE IF EXISTS `product_task_route_config`;
CREATE TABLE `product_task_route_config`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TaskId` int(11) NOT NULL,
  `RouteId` int(11) NOT NULL,
  `IsPrimary` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of product_task_route_config
-- ----------------------------

-- ----------------------------
-- Table structure for product_task_time_log
-- ----------------------------
DROP TABLE IF EXISTS `product_task_time_log`;
CREATE TABLE `product_task_time_log`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `StartTime` datetime NULL DEFAULT NULL COMMENT '订单开始时间',
  `StartRunTime` datetime NULL DEFAULT NULL COMMENT '订单开始运行时间',
  `RunTime` bigint(20) NULL DEFAULT NULL COMMENT '运行时间',
  `StartStopTime` datetime NULL DEFAULT NULL,
  `StopTime` bigint(20) NULL DEFAULT NULL,
  `StartErrTime` datetime NULL DEFAULT NULL,
  `ErrTime` bigint(20) NULL DEFAULT NULL,
  `ProductTaskId` int(11) NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '工单执行时间记录' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of product_task_time_log
-- ----------------------------

-- ----------------------------
-- Table structure for product_task_twocode1_config
-- ----------------------------
DROP TABLE IF EXISTS `product_task_twocode1_config`;
CREATE TABLE `product_task_twocode1_config`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `SCRQ` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '生产日期',
  `XLH` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '序列号',
  `CPXH` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '产品型号',
  `KHDM` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '客户代码',
  `Idx` int(11) NOT NULL COMMENT '下次打标流水号',
  `FCZFC` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '防错字符串',
  `TwoCodeId` int(11) NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of product_task_twocode1_config
-- ----------------------------

-- ----------------------------
-- Table structure for product_task_twocode2_config
-- ----------------------------
DROP TABLE IF EXISTS `product_task_twocode2_config`;
CREATE TABLE `product_task_twocode2_config`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `SCRQ` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '生产日期',
  `XLH` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '序列号',
  `CXH` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '产线号',
  `LJH` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '零件号',
  `GSDM` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '公司代码',
  `Idx` int(11) NOT NULL COMMENT '下次打标流水号',
  `FCZFC` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '防错字符串',
  `TwoCodeId` int(11) NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of product_task_twocode2_config
-- ----------------------------

-- ----------------------------
-- Table structure for product_task_twocode_config
-- ----------------------------
DROP TABLE IF EXISTS `product_task_twocode_config`;
CREATE TABLE `product_task_twocode_config`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Select` int(11) NOT NULL COMMENT '1 表示型号Ⅰ,2表示型号Ⅱ',
  `TaskId` int(11) NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of product_task_twocode_config
-- ----------------------------

-- ----------------------------
-- Table structure for productserial
-- ----------------------------
DROP TABLE IF EXISTS `productserial`;
CREATE TABLE `productserial`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `DZ_Serial` bigint(20) NOT NULL COMMENT '底座序号',
  `GK_Serial` bigint(20) NOT NULL COMMENT '管壳序号',
  `FX_Serial` bigint(20) NULL DEFAULT NULL COMMENT '返修序号',
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of productserial
-- ----------------------------
INSERT INTO `productserial` VALUES (1, 1, 1, NULL, NULL, '2023-03-07 14:50:56', '2023-05-18 10:02:28', NULL, 'developer', NULL);

-- ----------------------------
-- Table structure for sysmenu
-- ----------------------------
DROP TABLE IF EXISTS `sysmenu`;
CREATE TABLE `sysmenu`  (
  `Id` int(11) NOT NULL,
  `Name` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Navigate` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Icon` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `ParentId` int(11) NOT NULL,
  `Sort` int(11) NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of sysmenu
-- ----------------------------
INSERT INTO `sysmenu` VALUES (10, '首页', 'DashboardView', 'mdi-home', 0, 0, NULL, '2025-07-08 15:10:31', NULL, 'developer', NULL, NULL);
INSERT INTO `sysmenu` VALUES (100, '基础数据', '', 'mdi-file', 0, 0, NULL, '2023-02-03 14:12:18', NULL, 'developer', NULL, NULL);
INSERT INTO `sysmenu` VALUES (102, '用户管理', 'UserView', 'mdi-account', 100, 6, NULL, '2023-02-03 14:12:18', NULL, 'developer', NULL, NULL);
INSERT INTO `sysmenu` VALUES (103, '权限管理', 'Role', 'mdi-shield-account', 100, 6, NULL, '2023-02-03 14:12:18', NULL, 'developer', NULL, NULL);
INSERT INTO `sysmenu` VALUES (104, '型号管理', 'ProductVersion', 'mdi-car-electric', 100, 6, NULL, '2023-02-03 14:12:18', NULL, 'developer', NULL, NULL);
INSERT INTO `sysmenu` VALUES (105, '型号属性', 'VersionAttribute', 'mdi-cog-outline', 100, 6, NULL, '2023-02-03 14:12:18', NULL, 'developer', NULL, NULL);
INSERT INTO `sysmenu` VALUES (106, '工艺路线', 'ProcessRoute', 'mdi-router', 100, 6, NULL, '2023-02-03 14:12:18', NULL, 'developer', NULL, NULL);
INSERT INTO `sysmenu` VALUES (200, '产品管理', '', 'mdi-package-variant-closed-check', 0, 0, NULL, '2023-02-03 14:12:18', NULL, 'developer', NULL, NULL);
INSERT INTO `sysmenu` VALUES (201, '工单任务', 'ProductTask', 'mdi-format-list-numbered-rtl', 200, 6, NULL, '2023-02-03 14:12:18', NULL, 'developer', NULL, NULL);
INSERT INTO `sysmenu` VALUES (202, '标签监控', 'LabelView', 'mdi-barcode', 200, 6, NULL, '2023-02-03 14:12:18', NULL, 'developer', NULL, NULL);
INSERT INTO `sysmenu` VALUES (203, '产品配方', 'Recipe', 'mdi-format-list-checkbox', 200, 6, NULL, '2023-02-03 14:12:18', NULL, 'developer', NULL, NULL);
INSERT INTO `sysmenu` VALUES (400, '设备管理', '', 'mdi-steam', 0, 0, NULL, '2023-02-03 14:12:18', NULL, 'developer', NULL, NULL);
INSERT INTO `sysmenu` VALUES (401, '设备监控', 'MachineView', 'mdi-monitor-eye', 400, 6, NULL, '2023-02-03 14:12:18', NULL, 'developer', NULL, NULL);
INSERT INTO `sysmenu` VALUES (402, '实时报警', 'RTWarningView', 'mdi-bell-alert', 400, 6, NULL, '2023-02-03 14:12:18', NULL, 'developer', NULL, NULL);
INSERT INTO `sysmenu` VALUES (500, '外设管理', '', 'mdi-devices', 0, 0, NULL, '2023-02-03 14:12:18', NULL, 'developer', NULL, NULL);
INSERT INTO `sysmenu` VALUES (501, 'PLC管理', 'ConnSiemens', 'mdi-microsoft-visual-studio', 500, 6, NULL, '2023-02-03 14:12:18', NULL, 'developer', NULL, NULL);
INSERT INTO `sysmenu` VALUES (502, 'PLC事件', 'EventSiemens', 'mdi-microsoft-visual-studio', 500, 10, NULL, '2023-03-06 11:34:24', NULL, 'developer', NULL, NULL);
INSERT INTO `sysmenu` VALUES (600, '数据管理', '', 'mdi-database', 0, 0, NULL, '2023-02-03 14:12:18', NULL, 'developer', NULL, NULL);
INSERT INTO `sysmenu` VALUES (601, '数据(历史)', 'DataQualityAll', 'mdi-database-search-outline', 600, 6, NULL, '2023-02-03 14:12:18', NULL, 'developer', NULL, NULL);
INSERT INTO `sysmenu` VALUES (603, '数据(实时)', 'DataQualityAllTmp', 'mdi-database-eye-outline', 600, 6, NULL, '2023-02-03 14:12:18', NULL, 'developer', NULL, NULL);
INSERT INTO `sysmenu` VALUES (700, '版本管理', NULL, 'mdi-database', 0, 6, NULL, '2025-07-08 15:27:35', NULL, 'developer', NULL, NULL);
INSERT INTO `sysmenu` VALUES (701, '程序打包', 'ProgramPackView', 'mdi-zip-box', 700, 6, NULL, '2025-07-08 15:28:23', NULL, 'developer', NULL, NULL);

-- ----------------------------
-- Table structure for sysrole
-- ----------------------------
DROP TABLE IF EXISTS `sysrole`;
CREATE TABLE `sysrole`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Sort` int(11) NOT NULL COMMENT '1-10数字越大等级越高 10表示程序设计人员',
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of sysrole
-- ----------------------------
INSERT INTO `sysrole` VALUES (1, '开发工程师', 10, '最高权限', '2023-02-03 14:12:18', NULL, NULL, NULL, NULL);
INSERT INTO `sysrole` VALUES (2, '设备管理人员', 9, NULL, '2023-02-03 14:12:18', NULL, NULL, NULL, NULL);
INSERT INTO `sysrole` VALUES (3, '工艺人员', 8, NULL, '2023-02-03 14:12:18', NULL, NULL, NULL, NULL);
INSERT INTO `sysrole` VALUES (5, '作业员', 7, NULL, '2023-02-03 14:12:18', NULL, NULL, NULL, NULL);

-- ----------------------------
-- Table structure for sysrolemenu
-- ----------------------------
DROP TABLE IF EXISTS `sysrolemenu`;
CREATE TABLE `sysrolemenu`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `RoleId` int(11) NOT NULL,
  `MenuId` int(11) NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of sysrolemenu
-- ----------------------------

-- ----------------------------
-- Table structure for sysuser
-- ----------------------------
DROP TABLE IF EXISTS `sysuser`;
CREATE TABLE `sysuser`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `DisplayName` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Name` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `JobNumber` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '工号',
  `Password` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Department` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `RoleId` int(11) NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreatedTime` datetime NULL DEFAULT NULL,
  `UpdatedTime` datetime NULL DEFAULT NULL,
  `CreatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `UpdatedUserName` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `IsDeleted` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of sysuser
-- ----------------------------
INSERT INTO `sysuser` VALUES (1, NULL, 'developer', '1', '202CB962AC59075B964B07152D234B70', 'BU6', 1, NULL, NULL, '2025-06-27 11:13:05', NULL, 'developer', NULL);
INSERT INTO `sysuser` VALUES (2, NULL, '宋', '5414', '202CB962AC59075B964B07152D234B70', '经开', 3, NULL, '2023-05-15 15:36:41', NULL, 'developer', NULL, NULL);
INSERT INTO `sysuser` VALUES (3, NULL, '王晗飞', '50453', '202CB962AC59075B964B07152D234B70', '维修', 2, NULL, '2023-05-16 09:49:39', NULL, 'developer', NULL, NULL);
INSERT INTO `sysuser` VALUES (4, NULL, '建勋', '9527', 'C4CA4238A0B923820DCC509A6F75849B', '管理', 2, NULL, '2025-06-27 14:55:29', NULL, 'developer', NULL, NULL);

-- ----------------------------
-- Event structure for DeleteWorningLogEvent
-- ----------------------------
DROP EVENT IF EXISTS `DeleteWorningLogEvent`;
delimiter ;;
CREATE EVENT `DeleteWorningLogEvent`
ON SCHEDULE
EVERY '1' DAY STARTS '2023-05-05 16:33:15'
DO BEGIN
delete from machine_warning_log where TO_DAYS(now())-TO_DAYS(machine_warning_log.CreatedTime)>7;
END
;;
delimiter ;

-- ----------------------------
-- Event structure for test
-- ----------------------------
DROP EVENT IF EXISTS `test`;
delimiter ;;
CREATE EVENT `test`
ON SCHEDULE
EVERY '1' DAY STARTS '2023-05-06 08:33:16'
DO BEGIN
DELETE FROM log_eapevent WHERE log_eapevent.StartTime < DATE_SUB(CURDATE(),INTERVAL 30 DAY);
END
;;
delimiter ;

SET FOREIGN_KEY_CHECKS = 1;

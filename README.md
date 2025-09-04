# Avalonia.GetStartedApp 项目简介

## 项目概述

Avalonia.GetStartedApp 是一款基于 Avalonia UI 框架开发的跨平台工业应用程序，采用现代化 MVVM 架构设计，专注于工业自动化领域的数据采集、设备通信与监控管理。该项目支持 Windows 桌面端与 Web 浏览器双平台部署，具备强大的可扩展性和模块化设计。

## 核心功能

- **多平台支持**：同时支持桌面应用（Windows）和 Web 浏览器运行模式
- **工业设备通信**：集成西门子、欧姆龙、三菱、倍福等主流 PLC 通信协议
- **用户权限管理**：基于角色的访问控制(RBAC)系统，支持用户认证与权限分配
- **数据管理**：通过 SqlSugar ORM 框架实现与 MySQL 数据库的高效交互
- **实时监控**：PLC 数据实时采集与事件日志记录
- **自动更新**：集成 Velopack 框架实现应用程序自动更新
- **日志系统**：基于 Serilog 的全面日志记录与异常处理机制

## 技术架构

### 技术栈

| 类别         | 核心技术                                                                 |
|--------------|--------------------------------------------------------------------------|
| UI 框架      | Avalonia UI 11.3.3                                                      |
| MVVM 框架    | Prism + CommunityToolkit.Mvvm                                           |
| 依赖注入     | DryIoc                                                                  |
| 数据访问     | SqlSugar (MySQL)                                                        |
| 日志系统     | Serilog                                                                 |
| 通信协议     | MQTT、OPC UA、Siemens S7 Protocol                                       |
| 自动更新     | Velopack                                                                |
| 图标支持     | Material Design Icons                                                   |

### 项目结构

```
Avalonia.GetStartedApp/
├── GetStartedApp           # 主应用模块（UI、ViewModel、业务逻辑）
├── GetStartedApp.Desktop   # 桌面应用入口
├── GetStartedApp.Browser   # Web浏览器应用入口
├── GetStartedApp.SqlSugar  # 数据访问层
├── GetStartedApp.Core      # 核心工具类
└── SmartCommunicationForExcel # 工业通信组件
```

## 应用场景

- 工业自动化设备监控系统
- 生产数据采集与分析平台
- 智能制造执行系统(MES)前端
- 工业物联网(IIoT)数据网关

## 特点优势

1. **跨平台部署**：一次开发，同时支持桌面与Web端运行
2. **模块化设计**：各功能模块解耦，便于维护与扩展
3. **工业级稳定性**：完善的异常处理与日志记录机制
4. **丰富的通信接口**：支持多种工业设备协议与数据库
5. **现代化UI**：基于Avalonia的美观界面，支持主题定制

## 快速启动

### 桌面端
```bash
cd GetStartedApp.Desktop
dotnet run
```

### Web端
```bash
cd GetStartedApp.Browser
dotnet run
```

## 版权信息

本项目采用 MIT 开源协议，详情参见 LICENSE 文件。

wiki [![Ask DeepWiki](https://deepwiki.com/badge.svg)](https://deepwiki.com/Leijunshiwoer/Avalonia.GetStartedApp)
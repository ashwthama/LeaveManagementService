# LeaveManagementService

## 📘 Project Overview

This project demonstrates a microservices architecture built using ASP.NET Core Web API, featuring three key services:

1. **User Service**
2. **Leave Service**
3. **Notification Service**

All services communicate through a centralized **YARP Gateway**, ensuring secure, scalable, and efficient routing.

---

## 🧩 Microservices Description

### 🔐 User Service (`USERDB`)
- Handles user **registration** and **login**.
- Implements **role-based authentication and authorization** (e.g., Employee, Manager, HR/Admin).
- Generates **JWT tokens** upon successful login.

### 🗓️ Leave Service (`LEAVEDB`)
- Employees can submit **leave requests**.
- Managers can **approve or reject** the requests.
- HR/Admin roles can view all leave records.
- Sends notification trigger via **HTTP call** to the Notification Service (through YARP Gateway).

### 📬 Notification Service (RabbitMQ enabled)
- Listens for **leave status updates** via HTTP request from Leave Service.
- Uses **RabbitMQ** for asynchronous message queuing.
- Sends **email/SMS notifications** to users (Manager/Employee) based on the leave decision.

---

## 🌐 YARP Gateway

- Routes all HTTP requests to their respective services based on predefined paths.
- Acts as the single entry point for external clients.
- Provides **load balancing, routing, and security** between services.

---

## 🔄 Data Flow

1. User logs in via Gateway → Authenticates via User Service → Receives JWT.
2. Employee submits leave request via Gateway → Forwarded to Leave Service.
3. Manager updates leave status → Leave Service sends HTTP request to Notification Service via Gateway.
4. Notification Service processes the request asynchronously via RabbitMQ and sends the alert.

---

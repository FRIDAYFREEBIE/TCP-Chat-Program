# TCP Chat Program (Client–Server)

### 🎮 Third Toy Project

---

## 📌 Project Overview

* **Project Type**: Console-Based Chat Program
* **Network Model**: TCP Client–Server Architecture
* **Team Size**: Solo Development
* **Goal**: To understand C# network programming, learn the structure of TCP communication, and understand client handling in a multithreaded environment

---

## 🔑 Key Technologies

* **C#**

  * Core programming language used for both the server and client implementation

* **TCP Networking**

  * Implemented reliable client-server communication using TCP sockets
  * Managed message transmission between multiple clients through the server

* **Multithreading**

  * Assigned a dedicated thread to each connected client
  * Separated message sending and receiving into two independent threads
  * Enabled simultaneous message transmission and reception

* **Synchronization**

  * Used `lock` to safely manage shared resources in a multithreaded environment
  * Prevented race conditions caused by concurrent thread access

---

## 🤔 What I Learned

* Gained a clear understanding of the TCP communication flow (Client → Server → Client).

* Experienced synchronization issues in a multithreaded environment and learned the importance of thread safety using `lock`.

* Learned how concurrent access to shared resources can cause unexpected problems and how synchronization mechanisms can resolve them.

* Improved understanding of server-client architecture and real-time communication handling.

---

## 📄 Project Resources

**Project Video (YouTube)**
<p align="center">
  <a href="https://youtu.be/pCu02KphBsg">
    <img src="https://img.youtube.com/vi/pCu02KphBsg/maxresdefault.jpg" width="500">
  </a>
</p>

[TCP Chat Blog Post](https://fridayfreebie.tistory.com/category/%ED%94%84%EB%A1%9C%EC%A0%9D%ED%8A%B8/TCP%20Chat%20%28Network%29)

---

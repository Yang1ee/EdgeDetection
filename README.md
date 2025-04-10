# EdgeDetection
Robust C# framework designed to perform image edge detection using two industry-standard algorithms – the Sobel and Prewitt operators

## 📌 Overview

This project is a C# implementation of an **image edge detection algorithm** using either the **Sobel** or **Prewitt** operator. The user can select the operator type before processing an input grayscale image. The project demonstrates core concepts in image processing, clean architecture, and unit testing in C#.

---

## 🚀 Features

- Selectable edge detection operators: **Sobel** or **Prewitt**
- Grayscale image input and output
- Clean and modular architecture
- Unit tests for core logic and operator selection
- UML class diagram included for architectural clarity

---

## 📘 UML Class Diagram
This UML diagram shows how the Edge Detection WPF application is structured. It includes:
- The `IEdgeDetectionOperator` interface and its implementations.
- A `Processor` that uses the strategy pattern.
- A `MainWindow` UI entry point.
- A `UnitTests` class to ensure logic correctness.

![UML Diagram](UMLDiagram/ClassDiagram.png)



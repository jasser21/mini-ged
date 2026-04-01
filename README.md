Installation & Setup Guide

This guide will help you set up and run a Vue 3 frontend and .NET 9 backend project from zero.

1. Prerequisites

Ensure the following software is installed:

Node.js & npm (for Vue 3)
Recommended: Node.js ≥ 18

Verify installation:

node -v
npm -v
.NET 9 SDK (for backend)
Download: https://dotnet.microsoft.com/en-us/download

Verify installation:

dotnet --version
IDE/Editor (optional but recommended)
VS Code, Visual Studio, JetBrains Rider, etc.
2. Clone the Project

Open a terminal and run:

git clone <your-repo-url>
cd <project-root>

Replace <your-repo-url> with your repository link.

3. Backend Setup (.NET 9)

Its always better to just open the backend solution (.sln file) with visual studio and just go from there.
Navigate to the backend folder:
cd backend
Restore NuGet dependencies:
dotnet restore
Run the backend API:
dotnet run

By default, the backend will be available at https://localhost:5001 or http://localhost:5000.

4. Frontend Setup (Vue 3)
Open a new terminal, navigate to the frontend folder:
cd frontend
Install dependencies:
npm install
Run the development server:
npm run dev

The frontend will usually run at http://localhost:5173. The port may vary; check the terminal output.

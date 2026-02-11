BookMyShow Clone - Full Stack Web Application

A professional-grade movie booking platform built with a React frontend and a .NET Core backend. This project features a secure authentication system and a mandatory location-based showtime filtering logic.

🚀 Key Features
Secure Authentication: Fully functional Login and Registration system.

Mandatory City Selection: Users must select a State and City in the Navbar to view available theatres and showtimes.

Dynamic Showtime Filtering: The app strictly filters theatres based on the selected city (e.g., Mumbai shows PVR Phoenix, Pune shows Inox Forum).

Real-time Seat Booking: Interactive seat selection and booking summary flow.

🛠️ Tech Stack
Frontend: React.js, Tailwind CSS, Axios, React Router.

Backend: .NET Core Web API, Entity Framework Core.

Database: PostgreSQL (pgAdmin).

🏁 Getting Started

1. Database Setup (PostgreSQL)
Before running the app, ensure your database is configured:

Open pgAdmin and create a database named bookmyshow_db.

Run the following SQL to set up the cities and theatres:

SQL
INSERT INTO city (city_id, city_name, state_id) VALUES (1, 'Mumbai', 1), (2, 'Pune', 1);
INSERT INTO theatre (theatre_id, theatre_name, city_id) VALUES (1, 'PVR Phoenix', 1), (2, 'Inox Forum', 2);
2. Backend Setup (.NET)
Navigate to the backend folder: cd generated-dotnet-app.

Update the connection string in appsettings.json with your PostgreSQL credentials.

Run the application: dotnet run.

3. Frontend Setup (React)
Navigate to the frontend folder: cd frontend.

Install dependencies: npm install.

Start the development server: npm run dev
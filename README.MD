# Prerequisites
- .NET 6 SDK or later
- Visual Studio or Visual Studio Code

# Installation
git clone <repository-url>
cd <project-directory>
dotnet restore
dotnet run

# Usage
- `dotnet run` to run the application

The project runs on http://localhost:5000 by default. You can access the Swagger UI at http://localhost:5000/swagger

# Assumptions
The backend returns data without any missing fields.
The data format complies with the agreed-upon structure.
Authentication is not required to call the backend API.
Data transmission does not need to be encrypted.

# Trade-offs
To simplify the code, exception handling is only implemented in the API endpoint for posting note.

# Estimated Time
0.5 hour
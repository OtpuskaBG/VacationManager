# VacationManager

**VacationManager** is a role-based vacation management system built with Blazor. It supports user roles such as CEO and Employees, and provides features like team management, project assignments, and vacation tracking with a clean and intuitive UI.

## 🌟 Features

- 🔐 **Role-Based Access**: Different permissions for CEO, Team Leads, and Developers.
- 👥 **Team Management**: Create, edit, and manage teams and their members.
- 📁 **Project Management**: Assign projects to teams, update project status, and soft-delete projects.
- 🗓️ **Vacation Requests**: Employees can request vacations, and managers can approve or deny them.
- 🧠 **Domain-Driven Design**: Rich entity relationships with custom services for handling logic.
- 🔄 **Soft Delete Logic**: Projects are unlinked from teams instead of being permanently deleted.
- ✅ **Authentication Context**: Tracks the logged-in user and scopes actions accordingly.

## 🏗️ Architecture

- **Frontend**: Blazor Server (C#)
- **Backend**: ASP.NET Core
- **Services**:
  - `TeamService`
  - `ProjectService`
- **Repositories**: Generic `IRepository<T>` for data access
- **Authentication**: Custom `IAuthenticationContext`

## 📦 Installation

1. **Clone the repository**

```bash
git clone https://github.com/OtpuskaBG/VacationManager.git
cd VacationManager

# School Management System API

## Overview
This ASP.NET Web API provides CRUD operations for managing teachers and students in a school database. The system includes both API endpoints and MVC controllers for web interface integration.

## Features

### Teacher Management
- List all teachers with optional search
- Get teacher details by ID
- Add new teachers
- Update existing teachers
- Delete teachers

### Student Management
- List all students
- Get student details by ID
- Add new students
- Update student records
- Remove students

## API Endpoints

### Teacher API
| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/TeacherData/ListTeachers/{SearchKey?}` | List all teachers (optional search) |
| `GET` | `/api/TeacherData/FindTeacher/{id}` | Get teacher by ID |
| `POST` | `/api/TeacherData/AddTeacher` | Add new teacher |
| `PUT` | `/api/TeacherData/UpdateTeacher/{id}` | Update teacher |
| `DELETE` | `/api/TeacherData/DeleteTeacher/{id}` | Delete teacher |

### Student API
| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/StudentPage/ListStudents` | List all students |
| `GET` | `/api/StudentPage/FindStudent/{id}` | Get student by ID |
| `POST` | `/api/StudentPage/AddStudent` | Add new student |
| `PUT` | `/api/StudentPage/UpdateStudent/{id}` | Update student |
| `DELETE` | `/api/StudentPage/DeleteStudent/{id}` | Delete student |

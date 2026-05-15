# J&T Express PH - Logistics Web Application

A full-stack web application for J&T Express Philippines, featuring service management, branch locator, rate calculator, and parcel tracking.

## 🚀 Tech Stack

### Backend
- **.NET 9** - Web API
- **Entity Framework Core** - ORM with SQLite
- **JWT Authentication** - Secure user authentication
- **Clean Architecture** - Feature-based organization

### Frontend
- **React 18** - UI library
- **TypeScript** - Type safety
- **Vite** - Build tool
- **React Router v6** - Navigation
- **Axios** - HTTP client
- **CSS Modules** - Scoped styling

## 📁 Project Structure

```
COMMISSION_1/
├── JTExpress.Api/              # Backend API
│   └── JTExpress.Api/
│       ├── Common/             # Shared utilities
│       ├── Features/           # Feature modules
│       │   ├── Auth/
│       │   ├── Services/
│       │   ├── Branches/
│       │   ├── Rates/
│       │   └── Tracking/
│       ├── Data/               # Database context
│       └── Program.cs
│
├── jt-express-web/             # Frontend React app
│   └── src/
│       ├── api/                # API client
│       ├── components/         # Shared components
│       ├── features/           # Feature pages
│       │   ├── home/
│       │   ├── services/
│       │   ├── branches/
│       │   ├── rates/
│       │   ├── tracking/
│       │   └── auth/
│       ├── context/            # React context
│       ├── styles/             # Global styles
│       └── types/              # TypeScript types
│
└── RULE/                       # Architecture docs
    ├── JT_Express_PH_Dev_Workflow.md
    ├── JT_Express_PH_UI_Guide.md
    └── JT_Express_PH_Production_Guide.md
```

## 🛠️ Setup Instructions

### Prerequisites
- Node.js 18+ and npm
- .NET 9 SDK
- Git

### Backend Setup

1. Navigate to the API directory:
```bash
cd JTExpress.Api/JTExpress.Api
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Run database migrations:
```bash
dotnet ef database update
```

4. Start the API:
```bash
dotnet run
```

The API will be available at `http://localhost:5000`
Swagger UI: `http://localhost:5000/swagger`

### Frontend Setup

1. Navigate to the web directory:
```bash
cd jt-express-web
```

2. Install dependencies:
```bash
npm install
```

3. Create `.env` file:
```env
VITE_API_BASE_URL=http://localhost:5000/api
```

4. Start the development server:
```bash
npm run dev
```

The app will be available at `http://localhost:5173`

## 🔑 Features

### Public Features
- **Home Page** - Hero section with quick access cards
- **Services** - View available delivery services
- **Branches** - Find branches by region with contact info
- **Rates** - View rate table and calculate shipping costs
- **Tracking** - Track parcels by tracking number

### Admin Features (Authentication Required)
- **Service Management** - Create, update, delete services
- **User Authentication** - Login/signup functionality

## 📡 API Endpoints

### Public Endpoints
```
GET    /api/services              - List all services
GET    /api/branches?region=      - List branches (optional filter)
GET    /api/rates                 - List all rate zones
GET    /api/rates/calculate       - Calculate shipping rate
GET    /api/tracking/{number}     - Track parcel
POST   /api/auth/register         - Register new user
POST   /api/auth/login            - Login user
```

### Protected Endpoints (Requires JWT)
```
POST   /api/services              - Create service
PUT    /api/services/{id}         - Update service
DELETE /api/services/{id}         - Delete service
```

## 🎨 Design System

### Colors
- **Primary:** `#E31837` (J&T Red)
- **Text:** `#1A1A1A` (Near Black)
- **Background:** `#F5F5F5` (Light Gray)
- **Surface:** `#FFFFFF` (White)

### Typography
- **Font:** Inter, system-ui, sans-serif
- **Scale:** 12px - 30px

### Spacing
- **Base Unit:** 4px (0.25rem)
- **Scale:** 4px, 8px, 12px, 16px, 20px, 24px, 32px, 40px, 48px, 64px

## 🏗️ Architecture Principles

### Backend
1. **Feature-based organization** - Each feature is self-contained
2. **Repository pattern** - Data access abstraction
3. **Service layer** - Business logic separation
4. **Thin controllers** - Minimal logic in controllers
5. **Dependency injection** - All services registered in DI container

### Frontend
1. **Component-based** - Reusable UI components
2. **Custom hooks** - Data fetching logic
3. **CSS Modules** - Scoped styling
4. **Type safety** - TypeScript for all code
5. **Separation of concerns** - Logic separated from presentation

## 🧪 Testing

### Run Backend Tests
```bash
cd JTExpress.Api
dotnet test
```

### Run Frontend Tests
```bash
cd jt-express-web
npm test
```

### Build for Production
```bash
# Backend
dotnet publish -c Release

# Frontend
npm run build
```

## 📱 Responsive Design

The application is fully responsive with breakpoints:
- **Mobile:** < 640px
- **Tablet:** 768px - 1024px
- **Desktop:** > 1024px

## ♿ Accessibility

- Semantic HTML structure
- ARIA labels on interactive elements
- Keyboard navigation support
- Focus indicators
- Screen reader friendly
- Color contrast compliance (WCAG 2.1 AA)

## 🔒 Security

- JWT token authentication
- Password hashing (BCrypt)
- CORS configuration
- Input validation
- SQL injection prevention (EF Core parameterized queries)
- XSS protection

## 📝 Environment Variables

### Backend (`appsettings.json`)
```json
{
  "Cors": {
    "AllowedOrigins": ["http://localhost:5173"]
  },
  "Jwt": {
    "Key": "your-secret-key",
    "Issuer": "JTExpressAPI",
    "Audience": "JTExpressClient"
  }
}
```

### Frontend (`.env`)
```env
VITE_API_BASE_URL=http://localhost:5000/api
```

## 🚀 Deployment

### Backend Deployment
1. Update `appsettings.json` for production
2. Configure production database connection
3. Set up HTTPS
4. Deploy to hosting service (Azure, AWS, etc.)

### Frontend Deployment
1. Update `.env.production` with production API URL
2. Run `npm run build`
3. Deploy `dist/` folder to static hosting (Vercel, Netlify, etc.)

## 📚 Documentation

- **Dev Workflow:** `RULE/JT_Express_PH_Dev_Workflow.md`
- **UI Guide:** `RULE/JT_Express_PH_UI_Guide.md`
- **Production Guide:** `RULE/JT_Express_PH_Production_Guide.md`
- **Task List:** `TASK_LIST.md`
- **Progress:** `PROGRESS.md`

## 🤝 Contributing

This is a school project. Follow the architecture guidelines in the `RULE/` directory.

### Code Style
- Backend: Follow C# conventions, use `var` for local variables
- Frontend: Use functional components, TypeScript strict mode
- CSS: Use CSS custom properties, mobile-first approach

### Commit Messages
- Use conventional commits format
- Examples: `feat:`, `fix:`, `docs:`, `style:`, `refactor:`

## 📄 License

This is a school project for educational purposes.

## 👥 Team

School Project - J&T Express PH Web Application

## 📞 Support

For issues or questions, refer to the documentation in the `RULE/` directory.

---

**Built with ❤️ for J&T Express Philippines**

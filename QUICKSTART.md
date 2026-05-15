# Quick Start Guide

Get the J&T Express PH application running in 5 minutes.

---

## Prerequisites

- Node.js 18+ ([Download](https://nodejs.org/))
- .NET 9 SDK ([Download](https://dotnet.microsoft.com/download))
- Git

---

## 🚀 Quick Setup

### 1. Start Backend (Terminal 1)

```bash
# Navigate to API directory
cd C:\Users\Administrator\Pictures\COMMISSION_1\JTExpress.Api\JTExpress.Api

# Restore packages (first time only)
dotnet restore

# Run migrations (first time only)
dotnet ef database update

# Start API
dotnet run
```

✅ Backend running at: `http://localhost:5000`  
📚 Swagger UI: `http://localhost:5000/swagger`

### 2. Start Frontend (Terminal 2)

```bash
# Navigate to web directory
cd C:\Users\Administrator\Pictures\COMMISSION_1\jt-express-web

# Install dependencies (first time only)
npm install

# Start dev server
npm run dev
```

✅ Frontend running at: `http://localhost:5173`

---

## 🎯 Test the Application

### 1. Open Browser
Navigate to: `http://localhost:5173`

### 2. Try These Features

**Public Features:**
- ✅ Browse services
- ✅ Find branches (filter by region)
- ✅ Calculate shipping rates
- ✅ Track parcel (try: `JT123456789PH`)

**Admin Features (requires login):**
1. Click "Login" in navbar
2. Register a new account
3. Login with credentials
4. Go to Services page
5. Click "+ Add Service" to create/edit/delete

---

## 📝 Sample Data

### Tracking Numbers
- `JT123456789PH` - Out for Delivery
- `JT987654321PH` - Delivered

### Rate Zones
- Metro Manila
- Luzon
- Visayas
- Mindanao

### Branch Regions
- All
- Metro Manila
- Luzon
- Visayas
- Mindanao

---

## 🔧 Common Issues

### Backend won't start
```bash
# Check if port 5000 is in use
netstat -ano | findstr :5000

# Kill process if needed
taskkill /PID <process_id> /F

# Or change port in launchSettings.json
```

### Frontend won't start
```bash
# Clear node_modules and reinstall
rm -rf node_modules package-lock.json
npm install

# Or change port
npm run dev -- --port 3000
```

### Database issues
```bash
# Delete database and recreate
rm jtexpress.db
dotnet ef database update
```

### CORS errors
- Ensure backend is running on port 5000
- Check `appsettings.json` has `http://localhost:5173` in CORS
- Check frontend `.env` has `VITE_API_BASE_URL=http://localhost:5000/api`

---

## 📂 Project Structure

```
COMMISSION_1/
├── JTExpress.Api/          # Backend (.NET 9)
│   └── JTExpress.Api/
│       ├── Features/       # API endpoints
│       ├── Data/           # Database
│       └── Program.cs      # Entry point
│
├── jt-express-web/         # Frontend (React)
│   └── src/
│       ├── features/       # Pages
│       ├── components/     # Shared components
│       └── api/            # API client
│
└── RULE/                   # Documentation
```

---

## 🎨 Key URLs

| Service | URL |
|---------|-----|
| Frontend | http://localhost:5173 |
| Backend API | http://localhost:5000 |
| Swagger UI | http://localhost:5000/swagger |

---

## 📚 Documentation

- **Full README:** `README.md`
- **Architecture:** `RULE/JT_Express_PH_Dev_Workflow.md`
- **UI Guide:** `RULE/JT_Express_PH_UI_Guide.md`
- **Task List:** `TASK_LIST.md`
- **Progress:** `PROGRESS.md`
- **Deployment:** `DEPLOYMENT.md`

---

## 🛠️ Development Commands

### Backend
```bash
# Run API
dotnet run

# Run with watch (auto-reload)
dotnet watch run

# Build
dotnet build

# Run tests
dotnet test

# Create migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update
```

### Frontend
```bash
# Start dev server
npm run dev

# Build for production
npm run build

# Preview production build
npm run preview

# Type check
npm run type-check

# Lint
npm run lint
```

---

## 🔑 Default Credentials

No default users - register a new account:
1. Go to http://localhost:5173
2. Click "Login" → "Sign up"
3. Create account
4. Login with your credentials

---

## 🎯 What to Test

### Basic Flow
1. ✅ Home page loads
2. ✅ Navigate to Services
3. ✅ Navigate to Branches (try filtering)
4. ✅ Navigate to Rates (calculate a rate)
5. ✅ Navigate to Tracking (track `JT123456789PH`)

### Admin Flow
1. ✅ Register account
2. ✅ Login
3. ✅ Go to Services
4. ✅ Add new service
5. ✅ Edit service
6. ✅ Delete service
7. ✅ Logout

---

## 💡 Tips

- **Hot Reload:** Both backend and frontend support hot reload
- **Swagger:** Use Swagger UI to test API endpoints directly
- **DevTools:** Open browser DevTools to see network requests
- **Database:** SQLite database file is `jtexpress.db` in API directory
- **Logs:** Check terminal output for errors

---

## 🆘 Need Help?

1. Check terminal output for errors
2. Check browser console for frontend errors
3. Check Swagger UI to test API directly
4. Review documentation in `RULE/` directory
5. Check `PROGRESS.md` for known issues

---

## ✅ Success Checklist

- [ ] Backend running on port 5000
- [ ] Frontend running on port 5173
- [ ] Home page loads
- [ ] Can navigate between pages
- [ ] Can track a parcel
- [ ] Can calculate rates
- [ ] Can filter branches
- [ ] Can register/login
- [ ] Can manage services (when logged in)

---

**Ready to go!** 🚀

Open `http://localhost:5173` and start exploring!

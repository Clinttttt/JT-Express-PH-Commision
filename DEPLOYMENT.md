# Deployment Checklist

## Pre-Deployment

### Backend
- [ ] Update `appsettings.json` with production values
- [ ] Configure production database connection string
- [ ] Set strong JWT secret key
- [ ] Enable HTTPS
- [ ] Configure CORS for production domain
- [ ] Remove development logging
- [ ] Set up health check endpoint
- [ ] Configure rate limiting
- [ ] Set up monitoring/logging (Application Insights, Serilog)
- [ ] Run security scan
- [ ] Test all API endpoints in production-like environment

### Frontend
- [ ] Update `.env.production` with production API URL
- [ ] Remove all `console.log` statements
- [ ] Run `npm run build` successfully
- [ ] Test production build locally (`npm run preview`)
- [ ] Verify no TypeScript errors
- [ ] Check bundle size (< 500KB recommended)
- [ ] Optimize images (compress hero.png)
- [ ] Add meta tags for SEO
- [ ] Add favicon
- [ ] Test all routes work with production build

### Testing
- [ ] Run full regression test suite
- [ ] Test authentication flow end-to-end
- [ ] Test all CRUD operations
- [ ] Test form validations
- [ ] Test error handling
- [ ] Test on Chrome, Firefox, Safari, Edge
- [ ] Test on mobile devices (iOS, Android)
- [ ] Test on tablet
- [ ] Run Lighthouse audit (score > 90)
- [ ] Test with screen reader (NVDA/JAWS)
- [ ] Test keyboard navigation

## Deployment

### Backend Deployment (Example: Azure App Service)
```bash
# Build for production
dotnet publish -c Release -o ./publish

# Deploy to Azure (using Azure CLI)
az webapp up --name jt-express-api --resource-group jt-express-rg
```

### Frontend Deployment (Example: Vercel)
```bash
# Install Vercel CLI
npm i -g vercel

# Deploy
cd jt-express-web
vercel --prod
```

### Alternative: Netlify
```bash
# Build
npm run build

# Deploy dist/ folder via Netlify CLI or drag-and-drop
```

## Post-Deployment

### Verification
- [ ] API is accessible at production URL
- [ ] Swagger UI is disabled in production
- [ ] Frontend loads correctly
- [ ] All pages render without errors
- [ ] API calls work from frontend
- [ ] Authentication works
- [ ] HTTPS is enforced
- [ ] CORS is configured correctly
- [ ] Error pages display properly (404, 500)

### Monitoring
- [ ] Set up uptime monitoring
- [ ] Configure error tracking (Sentry, Application Insights)
- [ ] Set up performance monitoring
- [ ] Configure log aggregation
- [ ] Set up alerts for critical errors

### Documentation
- [ ] Update README with production URLs
- [ ] Document deployment process
- [ ] Create runbook for common issues
- [ ] Document rollback procedure

## Environment Configuration

### Backend Production Settings
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Cors": {
    "AllowedOrigins": ["https://your-production-domain.com"]
  },
  "ConnectionStrings": {
    "DefaultConnection": "your-production-db-connection-string"
  },
  "Jwt": {
    "Key": "your-strong-secret-key-min-32-chars",
    "Issuer": "JTExpressAPI",
    "Audience": "JTExpressClient",
    "ExpiryMinutes": 60
  }
}
```

### Frontend Production Settings
```env
VITE_API_BASE_URL=https://api.your-domain.com/api
```

## Rollback Plan

### If deployment fails:
1. Revert to previous version
2. Check logs for errors
3. Verify configuration
4. Test in staging environment
5. Redeploy when issue is resolved

### Backend Rollback
```bash
# Azure App Service
az webapp deployment slot swap --name jt-express-api --resource-group jt-express-rg --slot staging --target-slot production
```

### Frontend Rollback
```bash
# Vercel
vercel rollback
```

## Performance Optimization

### Backend
- [ ] Enable response compression
- [ ] Configure caching headers
- [ ] Use connection pooling
- [ ] Enable output caching for static data
- [ ] Optimize database queries

### Frontend
- [ ] Enable code splitting
- [ ] Lazy load routes
- [ ] Optimize images (WebP format)
- [ ] Enable browser caching
- [ ] Minify CSS/JS
- [ ] Use CDN for static assets

## Security Checklist

- [ ] HTTPS enforced
- [ ] Security headers configured (CSP, X-Frame-Options, etc.)
- [ ] Rate limiting enabled
- [ ] SQL injection protection verified
- [ ] XSS protection enabled
- [ ] CSRF protection for state-changing operations
- [ ] Sensitive data not logged
- [ ] API keys/secrets in environment variables
- [ ] Database backups configured
- [ ] Regular security updates scheduled

## DNS Configuration

### Example DNS Records
```
A     @              -> Backend IP
CNAME www            -> Frontend domain
CNAME api            -> Backend domain
```

## SSL/TLS Certificate

- [ ] SSL certificate installed
- [ ] Certificate auto-renewal configured
- [ ] HTTPS redirect enabled
- [ ] HSTS header configured

## Backup Strategy

### Database
- [ ] Daily automated backups
- [ ] Backup retention policy (30 days)
- [ ] Test restore procedure
- [ ] Off-site backup storage

### Application
- [ ] Source code in version control
- [ ] Tagged releases
- [ ] Deployment artifacts stored

## Maintenance

### Regular Tasks
- [ ] Monitor error logs weekly
- [ ] Review performance metrics monthly
- [ ] Update dependencies quarterly
- [ ] Security audit annually
- [ ] Database optimization quarterly

## Support

### Contact Information
- **Technical Lead:** [Name/Email]
- **DevOps:** [Name/Email]
- **Support:** [Email/Phone]

### Escalation Path
1. Check logs and monitoring
2. Review recent deployments
3. Contact technical lead
4. Escalate to senior developer if needed

---

**Last Updated:** May 15, 2026
**Version:** 1.0.0

# J&T Express PH — UI Standards Guide
> This is a school project. Keep the UI clean, readable, and professional.
> No over-decoration. No excessive color. No shadows on everything.

---

## Core Philosophy

The goal is a UI that looks like a real logistics company's website — not a portfolio project. That means restraint: one brand color used intentionally, neutral backgrounds, clear typography, and enough whitespace to breathe.

**Three rules that cover most decisions:**
1. Use the brand red for one thing per component — not three.
2. If you're unsure whether to add a visual element, leave it out.
3. Readable text at rest, feedback on interaction. That's it.

---

## File Structure: `.tsx` + `.module.css`

Every component has exactly two files — the component and its scoped styles. They live in the same folder.

```
src/
├── components/
│   └── shared/
│       ├── Navbar/
│       │   ├── Navbar.tsx
│       │   └── Navbar.module.css
│       ├── Footer/
│       │   ├── Footer.tsx
│       │   └── Footer.module.css
│       ├── LoadingSpinner/
│       │   ├── LoadingSpinner.tsx
│       │   └── LoadingSpinner.module.css
│       └── ErrorMessage/
│           ├── ErrorMessage.tsx
│           └── ErrorMessage.module.css
├── features/
│   ├── home/
│   │   ├── HomePage.tsx
│   │   └── HomePage.module.css
│   ├── services/
│   │   ├── ServicesPage.tsx
│   │   ├── ServicesPage.module.css
│   │   └── hooks/useServices.ts
│   ├── rates/
│   │   ├── RatesPage.tsx
│   │   ├── RatesPage.module.css
│   │   └── hooks/useRates.ts
│   ├── tracking/
│   │   ├── TrackingPage.tsx
│   │   ├── TrackingPage.module.css
│   │   └── hooks/useTracking.ts
│   └── branches/
│       ├── BranchesPage.tsx
│       ├── BranchesPage.module.css
│       └── hooks/useBranches.ts
└── styles/
    └── globals.css     ← design tokens + reset + shared layout classes
```

**Why CSS Modules over Tailwind for this project:**
- Class names are scoped — no accidental overrides
- No config file to maintain
- Styles are readable next to the component they affect
- You write real CSS, which professors can evaluate clearly

---

## Design Tokens — `src/styles/globals.css`

All colors, spacing, and font sizes are defined here as CSS custom properties. Components import nothing — they just use the variables. This is the single source of truth for the design.

```css
/* src/styles/globals.css */

:root {
  /* Brand colors */
  --color-primary:       #E31837;   /* J&T red — use sparingly */
  --color-primary-dark:  #B5102A;   /* hover state for red elements */
  --color-text:          #1A1A1A;   /* body text — near-black */
  --color-text-muted:    #6B7280;   /* secondary text, labels, captions */
  --color-text-light:    #9CA3AF;   /* placeholders, disabled states */

  /* Surfaces */
  --color-bg:            #F5F5F5;   /* page background */
  --color-surface:       #FFFFFF;   /* cards, inputs, modal backgrounds */
  --color-border:        #E5E7EB;   /* default border */
  --color-border-focus:  #E31837;   /* input focus ring */

  /* Status — kept muted, not neon */
  --color-success-bg:    #F0FDF4;
  --color-success-text:  #166534;
  --color-warning-bg:    #FFFBEB;
  --color-warning-text:  #92400E;
  --color-info-bg:       #EFF6FF;
  --color-info-text:     #1E40AF;
  --color-neutral-bg:    #F3F4F6;
  --color-neutral-text:  #4B5563;
  --color-error-bg:      #FEF2F2;
  --color-error-text:    #991B1B;

  /* Typography */
  --font-family:         'Inter', system-ui, -apple-system, sans-serif;
  --font-size-xs:        0.75rem;   /* 12px — captions, badges */
  --font-size-sm:        0.875rem;  /* 14px — secondary text */
  --font-size-base:      1rem;      /* 16px — body */
  --font-size-lg:        1.125rem;  /* 18px — card titles */
  --font-size-xl:        1.25rem;   /* 20px — section subheadings */
  --font-size-2xl:       1.5rem;    /* 24px — page headings */
  --font-size-3xl:       1.875rem;  /* 30px — hero headings */

  /* Spacing scale */
  --space-1:   0.25rem;   /* 4px */
  --space-2:   0.5rem;    /* 8px */
  --space-3:   0.75rem;   /* 12px */
  --space-4:   1rem;      /* 16px */
  --space-5:   1.25rem;   /* 20px */
  --space-6:   1.5rem;    /* 24px */
  --space-8:   2rem;      /* 32px */
  --space-10:  2.5rem;    /* 40px */
  --space-12:  3rem;      /* 48px */
  --space-16:  4rem;      /* 64px */

  /* Border radius */
  --radius-sm:   0.375rem;   /* 6px — inputs, small elements */
  --radius-md:   0.5rem;     /* 8px — buttons */
  --radius-lg:   0.75rem;    /* 12px — cards */
  --radius-xl:   1rem;       /* 16px — large cards */
  --radius-full: 9999px;     /* pill badges, filter buttons */

  /* Shadows — use only on cards and dropdowns */
  --shadow-sm:   0 1px 2px 0 rgb(0 0 0 / 0.05);
  --shadow-md:   0 4px 6px -1px rgb(0 0 0 / 0.07), 0 2px 4px -2px rgb(0 0 0 / 0.07);

  /* Layout */
  --content-width:   1200px;
  --content-padding: var(--space-4);
}

/* Reset */
*, *::before, *::after {
  box-sizing: border-box;
  margin: 0;
  padding: 0;
}

html {
  font-size: 16px;
  -webkit-text-size-adjust: 100%;
}

body {
  font-family: var(--font-family);
  font-size: var(--font-size-base);
  color: var(--color-text);
  background-color: var(--color-bg);
  line-height: 1.6;
  -webkit-font-smoothing: antialiased;
}

img, svg {
  display: block;
  max-width: 100%;
}

button {
  font-family: inherit;
  cursor: pointer;
  border: none;
  background: none;
}

input, select, textarea {
  font-family: inherit;
  font-size: inherit;
}

a {
  color: inherit;
  text-decoration: none;
}

/* App shell layout */
.app-shell {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}

.app-main {
  flex: 1;
}

/* Reusable page wrapper — apply to every page's root element */
.page-container {
  max-width: var(--content-width);
  margin: 0 auto;
  padding: var(--space-12) var(--content-padding);
}

.page-header {
  margin-bottom: var(--space-8);
}

.page-title {
  font-size: var(--font-size-2xl);
  font-weight: 700;
  color: var(--color-text);
  margin-bottom: var(--space-2);
}

.page-subtitle {
  font-size: var(--font-size-sm);
  color: var(--color-text-muted);
}
```

Import this once at the top of `src/main.tsx`:

```tsx
import "./styles/globals.css";
```

---

## Component Examples

### `LoadingSpinner`

```tsx
// LoadingSpinner.tsx
import styles from "./LoadingSpinner.module.css";

interface Props {
  message?: string;
}

export default function LoadingSpinner({ message = "Loading..." }: Props) {
  return (
    <div className={styles.wrapper}>
      <div className={styles.spinner} />
      <p className={styles.message}>{message}</p>
    </div>
  );
}
```

```css
/* LoadingSpinner.module.css */
.wrapper {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: var(--space-16) var(--space-4);
  gap: var(--space-3);
}

.spinner {
  width: 2rem;
  height: 2rem;
  border: 3px solid var(--color-border);
  border-top-color: var(--color-primary);
  border-radius: 50%;
  animation: spin 0.7s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

.message {
  font-size: var(--font-size-sm);
  color: var(--color-text-muted);
}
```

---

### `ErrorMessage`

```tsx
// ErrorMessage.tsx
import styles from "./ErrorMessage.module.css";

interface Props {
  message: string;
  onRetry?: () => void;
}

export default function ErrorMessage({ message, onRetry }: Props) {
  return (
    <div className={styles.wrapper}>
      <p className={styles.message}>{message}</p>
      {onRetry && (
        <button className={styles.retryBtn} onClick={onRetry}>
          Try again
        </button>
      )}
    </div>
  );
}
```

```css
/* ErrorMessage.module.css */
.wrapper {
  background-color: var(--color-error-bg);
  border: 1px solid #FECACA;
  border-radius: var(--radius-lg);
  padding: var(--space-4) var(--space-5);
}

.message {
  font-size: var(--font-size-sm);
  color: var(--color-error-text);
  font-weight: 500;
}

.retryBtn {
  margin-top: var(--space-2);
  font-size: var(--font-size-xs);
  color: var(--color-error-text);
  text-decoration: underline;
  background: none;
  border: none;
  cursor: pointer;
  padding: 0;
}

.retryBtn:hover {
  opacity: 0.75;
}
```

---

### `Navbar`

```tsx
// Navbar.tsx
import { Link, NavLink } from "react-router-dom";
import styles from "./Navbar.module.css";

const NAV_LINKS = [
  { to: "/services",  label: "Services"     },
  { to: "/rates",     label: "Rates"        },
  { to: "/tracking",  label: "Track Parcel" },
  { to: "/branches",  label: "Branches"     },
] as const;

export default function Navbar() {
  return (
    <nav className={styles.nav}>
      <div className={styles.inner}>
        <Link to="/" className={styles.brand}>
          J&amp;T Express <span className={styles.brandSub}>PH</span>
        </Link>
        <ul className={styles.links}>
          {NAV_LINKS.map(({ to, label }) => (
            <li key={to}>
              <NavLink
                to={to}
                className={({ isActive }) =>
                  isActive ? `${styles.link} ${styles.linkActive}` : styles.link
                }
              >
                {label}
              </NavLink>
            </li>
          ))}
        </ul>
      </div>
    </nav>
  );
}
```

```css
/* Navbar.module.css */
.nav {
  background-color: var(--color-primary);
  color: #fff;
  position: sticky;
  top: 0;
  z-index: 100;
  box-shadow: var(--shadow-sm);
}

.inner {
  max-width: var(--content-width);
  margin: 0 auto;
  padding: var(--space-3) var(--content-padding);
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.brand {
  font-size: var(--font-size-lg);
  font-weight: 800;
  letter-spacing: -0.02em;
  color: #fff;
}

.brandSub {
  font-size: var(--font-size-xs);
  font-weight: 400;
  opacity: 0.7;
  margin-left: var(--space-1);
}

.links {
  display: flex;
  gap: var(--space-6);
  list-style: none;
}

.link {
  font-size: var(--font-size-sm);
  font-weight: 500;
  color: rgba(255, 255, 255, 0.85);
  transition: color 0.15s;
}

.link:hover {
  color: #fff;
}

.linkActive {
  color: #fff;
  text-decoration: underline;
  text-underline-offset: 4px;
}
```

---

### `Footer`

```tsx
// Footer.tsx
import { Link } from "react-router-dom";
import styles from "./Footer.module.css";

export default function Footer() {
  return (
    <footer className={styles.footer}>
      <div className={styles.inner}>
        <div className={styles.brand}>
          <p className={styles.brandName}>J&amp;T Express PH</p>
          <p className={styles.tagline}>Fast. Reliable. Nationwide.</p>
        </div>
        <div className={styles.links}>
          <p className={styles.linksTitle}>Quick Links</p>
          <Link to="/services">Services</Link>
          <Link to="/rates">Rates</Link>
          <Link to="/tracking">Track Parcel</Link>
          <Link to="/branches">Branches</Link>
        </div>
        <div className={styles.contact}>
          <p className={styles.linksTitle}>Contact</p>
          <p>Hotline: 1800-108-600</p>
          <p>Email: support@jtexpress.ph</p>
          <p>Mon–Sat, 8AM–6PM</p>
        </div>
      </div>
      <div className={styles.bottom}>
        <p>&copy; {new Date().getFullYear()} J&amp;T Express Philippines. School Project.</p>
      </div>
    </footer>
  );
}
```

```css
/* Footer.module.css */
.footer {
  background-color: var(--color-text);
  color: rgba(255, 255, 255, 0.75);
  margin-top: auto;
}

.inner {
  max-width: var(--content-width);
  margin: 0 auto;
  padding: var(--space-10) var(--content-padding);
  display: grid;
  grid-template-columns: 2fr 1fr 1fr;
  gap: var(--space-8);
}

.brandName {
  font-size: var(--font-size-base);
  font-weight: 700;
  color: #fff;
  margin-bottom: var(--space-2);
}

.tagline {
  font-size: var(--font-size-sm);
}

.linksTitle {
  font-size: var(--font-size-xs);
  text-transform: uppercase;
  letter-spacing: 0.08em;
  color: rgba(255,255,255,0.4);
  margin-bottom: var(--space-3);
}

.links {
  display: flex;
  flex-direction: column;
  gap: var(--space-2);
  font-size: var(--font-size-sm);
}

.links a:hover {
  color: #fff;
}

.contact {
  display: flex;
  flex-direction: column;
  gap: var(--space-2);
  font-size: var(--font-size-sm);
}

.bottom {
  border-top: 1px solid rgba(255,255,255,0.08);
  padding: var(--space-4) var(--content-padding);
  text-align: center;
  font-size: var(--font-size-xs);
  color: rgba(255,255,255,0.3);
  max-width: var(--content-width);
  margin: 0 auto;
}

/* Mobile */
@media (max-width: 640px) {
  .inner {
    grid-template-columns: 1fr;
    gap: var(--space-6);
  }
}
```

---

## Page Layout Pattern

Every page follows the same wrapper structure. Use the global `.page-container` and `.page-header` classes from `globals.css`, then define page-specific styles in the module file.

```tsx
// Any page — template
import styles from "./SomePage.module.css";

export default function SomePage() {
  return (
    <div className="page-container">
      <div className="page-header">
        <h1 className="page-title">Page Title</h1>
        <p className="page-subtitle">A short description of this page.</p>
      </div>

      <div className={styles.content}>
        {/* Page-specific content here */}
      </div>
    </div>
  );
}
```

```css
/* SomePage.module.css — only page-specific overrides go here */
.content {
  /* grid, flex, or whatever this page needs */
}
```

---

## Card Pattern

Cards are the most reused visual element. One consistent style for all of them.

```css
/* In the relevant page's .module.css */
.card {
  background-color: var(--color-surface);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-xl);
  padding: var(--space-6);
  box-shadow: var(--shadow-sm);
  transition: box-shadow 0.15s, border-color 0.15s;
}

.card:hover {
  box-shadow: var(--shadow-md);
  border-color: var(--color-primary);
}
```

**Do not:** give cards a colored background, gradient, or multiple box-shadows. The hover border-color change is enough visual feedback.

---

## Button Pattern

```css
/* Primary button — use for the main action on a section */
.btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  padding: var(--space-3) var(--space-6);
  font-size: var(--font-size-sm);
  font-weight: 600;
  border-radius: var(--radius-md);
  transition: background-color 0.15s, opacity 0.15s;
  cursor: pointer;
  border: none;
}

.btnPrimary {
  background-color: var(--color-primary);
  color: #fff;
}

.btnPrimary:hover {
  background-color: var(--color-primary-dark);
}

.btnPrimary:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

/* Outline button — for filter tabs, secondary actions */
.btnOutline {
  background-color: var(--color-surface);
  color: var(--color-text-muted);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-full);  /* pill shape for filter tabs */
}

.btnOutline:hover {
  border-color: var(--color-primary);
  color: var(--color-text);
}

.btnOutlineActive {
  background-color: var(--color-primary);
  color: #fff;
  border-color: var(--color-primary);
}
```

---

## Input Pattern

```css
.input {
  width: 100%;
  padding: var(--space-3) var(--space-4);
  font-size: var(--font-size-sm);
  color: var(--color-text);
  background-color: var(--color-surface);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-sm);
  outline: none;
  transition: border-color 0.15s;
}

.input:focus {
  border-color: var(--color-border-focus);
  box-shadow: 0 0 0 3px rgb(227 24 55 / 0.08);
}

.input::placeholder {
  color: var(--color-text-light);
}

.label {
  display: block;
  font-size: var(--font-size-sm);
  font-weight: 500;
  color: var(--color-text);
  margin-bottom: var(--space-2);
}
```

---

## Status Badge Pattern

Used on the tracking page. A small pill showing a status — color-coded but muted.

```tsx
// In TrackingPage.tsx
const STATUS_STYLES: Record<string, string> = {
  "Delivered":        styles.statusDelivered,
  "Out for Delivery": styles.statusOutForDelivery,
  "In Transit":       styles.statusInTransit,
  "Arrived at Hub":   styles.statusArrivedAtHub,
  "Parcel Picked Up": styles.statusPickedUp,
};

// Usage
<span className={`${styles.badge} ${STATUS_STYLES[result.status] ?? styles.statusPickedUp}`}>
  {result.status}
</span>
```

```css
/* TrackingPage.module.css */
.badge {
  display: inline-block;
  padding: var(--space-1) var(--space-3);
  border-radius: var(--radius-full);
  font-size: var(--font-size-xs);
  font-weight: 600;
}

.statusDelivered        { background-color: var(--color-success-bg); color: var(--color-success-text); }
.statusOutForDelivery   { background-color: var(--color-info-bg);    color: var(--color-info-text);    }
.statusInTransit        { background-color: var(--color-warning-bg); color: var(--color-warning-text); }
.statusArrivedAtHub     { background-color: #F5F3FF;                 color: #5B21B6;                   }
.statusPickedUp         { background-color: var(--color-neutral-bg); color: var(--color-neutral-text); }
```

---

## Grid Layouts

```css
/* 3-col responsive grid (Services page) */
.grid3 {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: var(--space-6);
}

/* 2-col responsive grid (Branches page) */
.grid2 {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: var(--space-6);
}

@media (max-width: 1024px) {
  .grid3 { grid-template-columns: repeat(2, 1fr); }
}

@media (max-width: 640px) {
  .grid3, .grid2 { grid-template-columns: 1fr; }
}
```

---

## Table Pattern

Used on the Rates page.

```css
/* RatesPage.module.css */
.tableWrapper {
  overflow-x: auto;
  border: 1px solid var(--color-border);
  border-radius: var(--radius-lg);
  box-shadow: var(--shadow-sm);
  margin-bottom: var(--space-12);
}

.table {
  width: 100%;
  border-collapse: collapse;
  font-size: var(--font-size-sm);
}

.table thead {
  background-color: var(--color-primary);
  color: #fff;
}

.table th {
  padding: var(--space-3) var(--space-4);
  text-align: left;
  font-weight: 600;
}

.table td {
  padding: var(--space-3) var(--space-4);
}

.table tbody tr:nth-child(even) {
  background-color: var(--color-bg);
}

.table tbody tr:nth-child(odd) {
  background-color: var(--color-surface);
}
```

---

## Color Rules — What Goes Where

| Element | Color |
|---------|-------|
| Primary button, active filter pill, table header | `--color-primary` (red) |
| Hover state on primary | `--color-primary-dark` |
| Page background | `--color-bg` (light gray) |
| Cards, inputs, table rows | `--color-surface` (white) |
| Body text | `--color-text` (near-black) |
| Labels, captions, secondary text | `--color-text-muted` |
| Borders | `--color-border` |
| Focus rings | `--color-border-focus` (red at low opacity) |
| Status badges | The muted status tokens only |

**Do not use red for:** body text, borders, backgrounds, icon fills, or more than one element on the same card.

**Do not use more than 2 colors per component** (not counting white/black/gray).

---

## Typography Rules

- Page titles: `font-size: var(--font-size-2xl)`, `font-weight: 700`
- Section headings inside pages: `font-size: var(--font-size-xl)`, `font-weight: 600`
- Card titles: `font-size: var(--font-size-lg)`, `font-weight: 600`
- Body text, descriptions: `font-size: var(--font-size-base)` or `var(--font-size-sm)`, `font-weight: 400`
- Labels, captions, badges: `font-size: var(--font-size-xs)`, sometimes uppercase with letter-spacing

**Do not:** use more than 3 font sizes on a single page. Do not make body text bold — only labels and headings are bold.

---

## Spacing Rules

- Use the spacing scale — do not write arbitrary `px` values.
- Between a label and its input: `var(--space-2)`
- Between form fields: `var(--space-4)` or `var(--space-5)`
- Between cards in a grid: `var(--space-6)`
- Page top/bottom padding: `var(--space-12)`
- Between the page header and content: `var(--space-8)`

---

## What to Avoid

- **Gradients** — not used anywhere in this project
- **Animations beyond transitions** — the spinner is the only animation
- **Multiple shadow levels on the same element** — one shadow, used only on cards
- **Colored text on colored backgrounds** (except white on red for buttons/headers)
- **Decorative borders inside cards** — the card border is enough
- **Emojis as decorative icons in navigation** — only in data (service cards use them because the data says so)
- **Font weights above 800** — 700 is the max for headings
- **Centered body text** — only center short standalone captions or empty-state messages

---

## Responsive Breakpoints

| Breakpoint | Width | What changes |
|------------|-------|-------------|
| Mobile (default) | `< 640px` | Single column layouts, stacked footer |
| Tablet | `≥ 768px` | 2-column grids |
| Desktop | `≥ 1024px` | 3-column grids, full nav |

Write mobile-first: default styles are for mobile, `@media (min-width: ...)` adds wider layouts.

```css
/* Mobile first */
.grid {
  display: grid;
  grid-template-columns: 1fr;
  gap: var(--space-4);
}

@media (min-width: 768px) {
  .grid { grid-template-columns: repeat(2, 1fr); }
}

@media (min-width: 1024px) {
  .grid { grid-template-columns: repeat(3, 1fr); }
}
```

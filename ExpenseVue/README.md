# ExpenseVue - Expense Tracker Frontend

The frontend component of the Expense Tracker application, built with Vue.js.

## Technologies

- Vue.js 3.5.x with Composition API
- TypeScript for type safety
- Chart.js for visualizations
- Axios for API communication
- Tailwind CSS & DaisyUI for styling
- Vite as the build tool

## Prerequisites

- Node.js (v16+)
- npm or yarn
- ExpenseApi running (see backend README)

## Setup

1. Navigate to the frontend directory:

   ```
   cd ExpenseVue
   ```

2. Install dependencies:

   ```
   npm install
   ```

3. Create a `.env` file based on the `.env.example` template:

   ```
   cp .env.example .env
   ```

4. Update the API URL in the `.env` file if needed:
   ```
   VITE_API_URL=http://localhost:5111/api
   ```

## Running the Application

### Development Mode

Start the development server:

```
npm run dev
```

The application will be available at [http://localhost:5173](http://localhost:5173).

### Production Build

To create a production build:

```
npm run build
```

The built files will be in the `dist` directory, which can be served using any static file server.

Preview the production build locally:

```
npm run preview
```

## Project Structure

- `src/` - Source code
  - `api/` - API integration code
  - `assets/` - Static assets
  - `components/` - Vue components
  - `services/` - Business logic services
  - `types/` - TypeScript type definitions
  - `utils/` - Utility functions
  - `App.vue` - Root component
  - `main.ts` - Application entry point

## Key Features

- Dashboard with summary cards and transaction list
- Income/expense tracking with categories
- Time-based filtering (day, month, year)
- Data visualization with Chart.js
- Responsive design

## Environment Variables

| Variable     | Description            | Default                   |
| ------------ | ---------------------- | ------------------------- |
| VITE_API_URL | URL of the backend API | http://localhost:5111/api |

## Development Notes

- The application uses a proxy configuration for local development to avoid CORS issues
- TypeScript is used throughout for better type safety and development experience
- Chart.js components are registered globally in `main.ts`

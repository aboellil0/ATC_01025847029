API Endpoints
Authentication Controller
POST /api/auth/register      - Register a new user
POST /api/auth/login         - Login user
Events Controller
GET    /api/events           - Get all events
GET    /api/events/{id}      - Get event by ID
GET    /api/events/search    - Search events
POST   /api/events           - Create event (Admin)
PUT    /api/events/{id}      - Update event (Admin)
DELETE /api/events/{id}      - Delete event (Admin)
Bookings Controller
GET    /api/bookings             - Get current user's bookings
GET    /api/bookings/{id}        - Get booking by ID
POST   /api/bookings             - Create a booking
DELETE /api/bookings/{id}        - Cancel a booking
GET    /api/bookings/event/{id}  - Check if user has booked an event
Categories Controller
GET    /api/categories           - Get all categories
GET    /api/categories/{id}      - Get category by ID
POST   /api/categories           - Create category (Admin)
PUT    /api/categories/{id}      - Update category (Admin)
DELETE /api/categories/{id}      - Delete category (Admin)
Admin Controller
GET    /api/admin/users          - Get all users (Admin)
PUT    /api/admin/users/{id}/role - Update user role (Admin)
GET    /api/admin/bookings       - Get all bookings (Admin)
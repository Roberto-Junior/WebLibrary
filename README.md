# Library Management Website

### Login restrictions ("Usuário", "Administrador" and "Gerente"):
The "Usuário" login can/has:
   - Reserve a book from a book list.
   - Cancel your reservation.
   - Only have an active reservation (it is not possible to reserve several books).
   - Filter available books by genre, author or title.
   - Look at your current reservation and your reservation history.
   - Change your registered name and password.
   
The "Administrador" can/has:
   - Filter all books by genre, author or title.
   - Add, edit or remove books.
   - List all users.
   - Release reservations and returns.
   - Change your registered name and password.

The "Gerente" can/has:
   - All admin features.
   - Access to all users' booking history.
   - Create accounts with any type of authorization ("Usuário", "Administrador" or "Gerente").


### Default behavior (Seed data):
It was added:
- 3 types of authorization: "Usuário", "Administrador" and "Gerente".
- a default user with "Gerente" type authorization (Email: "gerente@hotmail.com", Password: "123456").
- 12 default books.
